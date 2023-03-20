using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Sales.Web.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(3000);
            var anonimous = new ClaimsIdentity();
            var JohanUser = new ClaimsIdentity(new List<Claim>
        {
            new Claim("FirstName", "Johan"),
            new Claim("LastName", "Daza"),
            new Claim(ClaimTypes.Name, "Jodaza95@yopmail.com"),
            new Claim(ClaimTypes.Role, "Admin")
        },
        authenticationType: "test");


            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(JohanUser)));
        }
    }
}
