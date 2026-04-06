using System;
using System.Net;
using System.Text.RegularExpressions;

namespace SafeVault.Security
{
    public static class InputSanitizer
    {
        private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9_]{3,50}$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public static string SanitizeUsername(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Username is required.");

            var trimmed = input.Trim();

            if (!UsernameRegex.IsMatch(trimmed))
                throw new ArgumentException("Username contains invalid characters.");

            return trimmed;
        }

        public static string SanitizeEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Email is required.");

            var trimmed = input.Trim();

            if (!EmailRegex.IsMatch(trimmed))
                throw new ArgumentException("Invalid email format.");

            return trimmed;
        }

        public static string EncodeForHtml(string input)
        {
            return WebUtility.HtmlEncode(input ?? string.Empty);
        }
    }
}
