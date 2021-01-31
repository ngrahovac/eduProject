using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
            return int.Parse(claims[1].ToString().Split("nameidentifier:")[1].Replace(" ", "")); //nameidentifier represents user ID
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

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
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
            return claims;
        }
    }
}