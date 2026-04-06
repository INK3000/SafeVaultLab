using System;
using NUnit.Framework;
using SafeVault.Security;

namespace SafeVault.Tests
{
    [TestFixture]
    public class TestInputValidation
    {
        [Test]
        public void SanitizeUsername_ValidInput_ReturnsTrimmedUsername()
        {
            var result = InputSanitizer.SanitizeUsername("john_doe");

            Assert.That(result, Is.EqualTo("john_doe"));
        }

        [Test]
        public void TestForSQLInjectionInUsername_ShouldThrowException()
        {
            var maliciousInput = "' OR 1=1 --";

            Assert.Throws<ArgumentException>(() =>
            {
                InputSanitizer.SanitizeUsername(maliciousInput);
            });
        }

        [Test]
        public void TestForXSSInUsername_ShouldThrowException()
        {
            var maliciousInput = "<script>alert('xss')</script>";

            Assert.Throws<ArgumentException>(() =>
            {
                InputSanitizer.SanitizeUsername(maliciousInput);
            });
        }

        [Test]
        public void SanitizeEmail_ValidInput_ReturnsEmail()
        {
            var result = InputSanitizer.SanitizeEmail("user@example.com");

            Assert.That(result, Is.EqualTo("user@example.com"));
        }

        [Test]
        public void SanitizeEmail_InvalidEmail_ShouldThrowException()
        {
            var invalidEmail = "bad-email@@example";

            Assert.Throws<ArgumentException>(() =>
            {
                InputSanitizer.SanitizeEmail(invalidEmail);
            });
        }

        [Test]
        public void EncodeForHtml_ShouldEncodeScriptTags()
        {
            var maliciousInput = "<script>alert('xss')</script>";

            var encoded = InputSanitizer.EncodeForHtml(maliciousInput);

            Assert.That(encoded, Does.Not.Contain("<script>"));
            Assert.That(encoded, Does.Contain("&lt;script&gt;"));
        }
    }
}
