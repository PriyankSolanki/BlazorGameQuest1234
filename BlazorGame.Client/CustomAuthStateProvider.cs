using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private readonly ILogger<CustomAuthStateProvider> _logger;

    public CustomAuthStateProvider(ITokenService tokenService, ILogger<CustomAuthStateProvider> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }
    
    public void NotifyUserAuthentication(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claims = new List<Claim>(jwt.Claims);

        // même logique pour les rôles ici
        var realmAccess = jwt.Claims.FirstOrDefault(c => c.Type == "realm_access")?.Value;
        if (!string.IsNullOrEmpty(realmAccess))
        {
            try
            {
                using var doc = JsonDocument.Parse(realmAccess);
                if (doc.RootElement.TryGetProperty("roles", out var rolesElement))
                {
                    foreach (var r in rolesElement.EnumerateArray())
                    {
                        var roleName = r.GetString();
                        if (!string.IsNullOrWhiteSpace(roleName))
                        {
                            claims.Add(new Claim(ClaimTypes.Role, roleName));
                        }
                    }
                }
            }
            catch { }
        }

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }
    
    public void Logout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenService.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(_anonymous);

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwt;

        try
        {
            jwt = handler.ReadJwtToken(token);
        }
        catch
        {
            await _tokenService.RemoveTokenAsync();
            return new AuthenticationState(_anonymous);
        }

        if (jwt.ValidTo < DateTime.UtcNow)
        {
            await _tokenService.RemoveTokenAsync();
            return new AuthenticationState(_anonymous);
        }

        // On part des claims existants
        var claims = new List<Claim>(jwt.Claims);

        // 👉 Rôles client Keycloak : resource_access.blazorGame.roles
        AddClientRolesFromResourceAccess(jwt, claims, "blazorGame");

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }


    private static void AddClientRolesFromResourceAccess(
        JwtSecurityToken jwt,
        List<Claim> claims,
        string clientId)
    {
        var resourceAccessJson = jwt.Claims.FirstOrDefault(c => c.Type == "resource_access")?.Value;
        if (string.IsNullOrEmpty(resourceAccessJson))
            return;

        try
        {
            using var doc = JsonDocument.Parse(resourceAccessJson);
            var root = doc.RootElement;

            // resource_access.blazorGame.roles
            if (root.TryGetProperty(clientId, out var clientElement) &&
                clientElement.TryGetProperty("roles", out var rolesElement))
            {
                foreach (var r in rolesElement.EnumerateArray())
                {
                    var roleName = r.GetString();
                    if (!string.IsNullOrWhiteSpace(roleName))
                    {
                        // 👉 Claim de rôle compréhensible par [Authorize(Roles = "...")]
                        claims.Add(new Claim(ClaimTypes.Role, roleName));
                    }
                }
            }
        }
        catch
        {
            // si parsing JSON foire, on ignore
        }
    }
}
