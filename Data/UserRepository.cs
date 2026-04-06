using SafeVault.Models;

namespace SafeVault.Data
{
    public class UserRepository
    {
        private readonly List<ApplicationUser> _users = new();
        private int _nextId = 1;

        public ApplicationUser? GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public ApplicationUser Add(ApplicationUser user)
        {
            user.Id = _nextId++;
            _users.Add(user);
            return user;
        }

        public IReadOnlyList<ApplicationUser> GetAll()
        {
            return _users.AsReadOnly();
        }
    }
}
