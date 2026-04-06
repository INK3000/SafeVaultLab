using NUnit.Framework;
using SafeVault.Data;
using SafeVault.DTOs;
using SafeVault.Security;
using SafeVault.Services;

namespace SafeVault.Tests
{
    [TestFixture]
    public class AuthServiceTests
    {
        private UserRepository _userRepository = null!;
        private PasswordService _passwordService = null!;
        private AuthService _authService = null!;

        [SetUp]
        public void Setup()
        {
            _userRepository = new UserRepository();
            _passwordService = new PasswordService();
            _authService = new AuthService(_userRepository, _passwordService);
        }

        [Test]
        public void Register_ShouldHashPassword_AndStoreUser()
        {
            var request = new RegisterRequest
            {
                Username = "alice",
                Password = "Password123!",
                Role = "user"
            };

            var user = _authService.Register(request);

            Assert.That(user.Username, Is.EqualTo("alice"));
            Assert.That(user.PasswordHash, Is.Not.EqualTo("Password123!"));
            Assert.That(user.Role, Is.EqualTo("user"));
        }

        [Test]
        public void Authenticate_WithValidCredentials_ShouldReturnUser()
        {
            _authService.Register(new RegisterRequest
            {
                Username = "bob",
                Password = "SecurePass1!",
                Role = "user"
            });

            var user = _authService.Authenticate(new LoginRequest
            {
                Username = "bob",
                Password = "SecurePass1!"
            });

            Assert.That(user, Is.Not.Null);
            Assert.That(user!.Username, Is.EqualTo("bob"));
        }

        [Test]
        public void Authenticate_WithInvalidPassword_ShouldReturnNull()
        {
            _authService.Register(new RegisterRequest
            {
                Username = "charlie",
                Password = "CorrectPassword",
                Role = "user"
            });

            var user = _authService.Authenticate(new LoginRequest
            {
                Username = "charlie",
                Password = "WrongPassword"
            });

            Assert.That(user, Is.Null);
        }

        [Test]
        public void Register_DuplicateUsername_ShouldThrowException()
        {
            _authService.Register(new RegisterRequest
            {
                Username = "david",
                Password = "Password123",
                Role = "user"
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                _authService.Register(new RegisterRequest
                {
                    Username = "david",
                    Password = "AnotherPassword",
                    Role = "admin"
                });
            });
        }
    }
}
