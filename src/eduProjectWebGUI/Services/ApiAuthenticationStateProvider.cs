using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Services
{
    /*
    AuthenticationStateProvider is the underlying service used by the AuthorizeView component and CascadingAuthenticationState 
    component to get the authentication state. (abstract class)

    The AuthorizeView component selectively displays UI content depending on whether the user is authorized.
    This approach is useful when you only need to display data for the user and don't need to use the user's identity in procedural logic.

    You don't typically use AuthenticationStateProvider directly. Use the AuthorizeView component or Task<AuthenticationState> approaches.

    If the app requires a custom provider, implement AuthenticationStateProvider and override GetAuthenticationStateAsync.

    Reference: https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-5.0#authentication
    */

    //More: https://www.pragimtech.com/blog/blazor/authorization-in-blazor/

    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        /*
        The GetAuthenticationStateAsync method is called by the CascadingAuthenticationState component to determine if the current user is
        authenticated or not.

        If there is a token, we retrieve it and set the default authorization header for the HttpClient. We then return a new AuthenticationState
        with a new claims principal containing the claims from the token.

        Reference: https://chrissainty.com/securing-your-blazor-apps-authentication-with-clientside-blazor-using-webapi-aspnet-core-identity/
        */
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
        }

        //Helper method
        public void MarkUserAsAuthenticated(string email)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState); //fires AuthenticationStateChanges event
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
                else
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                
                keyValuePairs.Remove(ClaimTypes.Role);

                //if something around here breaks, this call was originally outside of this if branch, if this information is of any value
                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }

            //_____________________________________________
            keyValuePairs.TryGetValue(ClaimTypes.NameIdentifier, out object id);
            if (id != null)
            {
                if (id.ToString().Trim().StartsWith("["))
                {
                    var parsedId = JsonSerializer.Deserialize<string[]>(id.ToString());
                    foreach (var parId in parsedId)
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, parId));
                }
                else
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
                keyValuePairs.Remove(ClaimTypes.NameIdentifier);
                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }
            //_____________________________________________

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
