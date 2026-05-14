using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WebsiteTour.Services
{
    public class SlugService
    {
        public string BuildSlug(string input)
        {
            var lower = input.Trim().ToLowerInvariant();

            var normalized = lower
                .Replace("đ", "d")
                .Normalize(NormalizationForm.FormD);

            var cleaned = new string(normalized
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            var slug = Regex.Replace(cleaned, @"[^a-z0-9]+", "-").Trim('-');

            return string.IsNullOrWhiteSpace(slug) ? "n-a" : slug;
        }
    }
}