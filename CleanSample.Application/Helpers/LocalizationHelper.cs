using System.Globalization;

namespace CleanSample.Application.Helpers;

/// <summary>
/// Helper utility to dynamically resolve localized strings based on the current request culture.
/// </summary>
public static class LocalizationHelper
{
    /// <summary>
    /// Returns the localized string based on CultureInfo.CurrentUICulture.
    /// If Arabic is requested and ar is non-empty, returns ar (otherwise falls back to en).
    /// If English is requested and en is non-empty, returns en (otherwise falls back to ar).
    /// </summary>
    public static string? Localize(string? en, string? ar)
    {
        var currentLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var isArabic = currentLang.Equals("ar", StringComparison.OrdinalIgnoreCase);

        if (isArabic)
        {
            return !string.IsNullOrWhiteSpace(ar) ? ar : en;
        }

        return !string.IsNullOrWhiteSpace(en) ? en : ar;
    }
}
