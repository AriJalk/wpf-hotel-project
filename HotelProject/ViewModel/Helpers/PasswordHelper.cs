using HotelProject.Model.DbClasses;

namespace HotelProject.ViewModel.Helpers
{
    public static class PasswordHelper
    {
        public static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public static string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool ValidatePassword(string password, User user)
        {
            if (BCrypt.Net.BCrypt.Verify(HashPassword(password, user.PasswordSalt), user.HashedPassword))
                return true;
            return false;
        }
    }
}
