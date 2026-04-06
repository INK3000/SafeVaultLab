using System.Security.Claims;
using NUnit.Framework;

namespace SafeVault.Tests
{
    [TestFixture]
    public class AuthorizationTests
    {
        [Test]
        public void AdminUser_ShouldHaveAdminRole()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin1"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            Assert.That(principal.IsInRole("admin"), Is.True);
        }

        [Test]
        public void NormalUser_ShouldNotHaveAdminRole()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "user1"),
                new Claim(ClaimTypes.Role, "user")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            Assert.That(principal.IsInRole("admin"), Is.False);
        }

        [Test]
        public void UserRole_ShouldAllowUserAreaAccess()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "user2"),
                new Claim(ClaimTypes.Role, "user")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            Assert.That(principal.IsInRole("user"), Is.True);
        }
    }
}
