using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace eduProjectWebAPI.Services
{
    public static class CustomJwtHandler
    {
        public static int? ExtractUserId(this HttpRequest request)
        {
            // TEST ONLY
            return 1;
            /*
            string jwt = "";
            var headerValues = request.Headers.Values;

            foreach (var value in headerValues)
                if (value.ToString().StartsWith("Bearer"))
                {
                    jwt = value.ToString().Split("Bearer")[1].Replace(" ", "");
                    break;
                }

            if (jwt == string.Empty)
                return null;

            var claims = ParseClaimsFromJwt(jwt).ToArray();
            return int.Parse(claims[1].ToString().Split("nameidentifier:")[1].Replace(" ", "")); //nameidentifier represents user ID
            */
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
