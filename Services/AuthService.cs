using SafeVault.Data;
using SafeVault.DTOs;
using SafeVault.Models;
using SafeVault.Security;

namespace SafeVault.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public AuthService(UserRepository userRepository, PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public ApplicationUser Register(RegisterRequest request)
        {
            var safeUsername = InputSanitizer.SanitizeUsername(request.Username);

            var existingUser = _userRepository.GetByUsername(safeUsername);
            if (existingUser != null)
                throw new InvalidOperationException("User already exists.");

            var user = new ApplicationUser
            {
                Username = safeUsername,
                Role = string.IsNullOrWhiteSpace(request.Role) ? "user" : request.Role.Trim().ToLowerInvariant()
            };

            user.PasswordHash = _passwordService.HashPassword(user, request.Password);
            return _userRepository.Add(user);
        }

        public ApplicationUser? Authenticate(LoginRequest request)
        {
            var safeUsername = InputSanitizer.SanitizeUsername(request.Username);

            var user = _userRepository.GetByUsername(safeUsername);
            if (user == null)
                return null;

            var isValid = _passwordService.VerifyPassword(user, user.PasswordHash, request.Password);
            return isValid ? user : null;
        }
    }
}
