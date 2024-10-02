using System.Text.RegularExpressions;

namespace Shared.Services
{
    public static class RegexValidator
    {
        public static bool IsValidEmail(string email)
        {
            //string complexEmailPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            // Simple email regex pattern
            string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
