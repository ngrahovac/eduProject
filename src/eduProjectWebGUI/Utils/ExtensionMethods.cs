using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace eduProjectWebGUI.Utils
{
    public static class ExtensionMethods
    {
        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }

        public static string QueryString(this NavigationManager navigationManager, string key)
        {
            return navigationManager.QueryString()[key];
        }

        public static async Task<int?> ExtractUserId(this ILocalStorageService localStorage)
        {
            var token = await localStorage.GetItemAsStringAsync("authToken");

            if (token == null)
                return null;

            var claims = ParseClaimsFromJwt(token).ToArray();

            //old value: claims[1] - caused index out of the bounds exception because role claim is added
            return int.Parse(claims[2].ToString().Split("nameidentifier:")[1].Replace(" ", "")); //nameidentifier represents user ID
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }

        public static ICollection<Claim> ParseClaimsFromJwt(string jwt)
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
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            //_____________________________________________
            //This part of code extracts ID. It is the same code above, copy-pasted. Refactoring needed.
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
            return claims.ToList();
        }

        public static bool IsSuccessCode(this HttpStatusCode code)
        {
            return (int)code < 400;
        }

        public static bool ShouldRedirectTo404(this HttpStatusCode code)
        {
            return code == HttpStatusCode.NotFound || code == HttpStatusCode.Unauthorized || code == HttpStatusCode.Forbidden;
        }
        public static string GetMessage(this HttpStatusCode code)
        {
            string message = "";

            switch (code)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.NoContent:
                    message = "Akcija je uspiješno izvršena";
                    break;
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    message = "Stranica ne postoji";
                    break;
                case HttpStatusCode.BadRequest:
                    message = "Došlo je do greške prilikom slanja zahtjeva. Molimo pokušajte ponovo.";
                    break;
                case HttpStatusCode.InternalServerError:
                    message = "Desila se greška prilikom obrade zahtjeva. Molimo pokušajte kasnije ili kontaktirajte administratora";
                    break;
            }

            return message;
        }
    }
}