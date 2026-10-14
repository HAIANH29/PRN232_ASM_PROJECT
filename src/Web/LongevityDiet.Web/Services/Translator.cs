using System.Globalization;
using Microsoft.Extensions.Localization;

namespace LongevityDiet.Web.Services;

// Marker type for the shared, dictionary-based resources.
public sealed class SharedResource;

/// <summary>
/// Looks up UI text by its English wording. Unknown text is returned as-is, so English
/// stays the fallback and only Vietnamese needs a dictionary entry.
/// </summary>
public sealed class Translator
{
    public static bool IsVietnamese =>
        CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("vi", StringComparison.OrdinalIgnoreCase);

    // Culture used only to print dates (month and day names); parsing always stays invariant.
    public static CultureInfo FormatCulture =>
        IsVietnamese ? CultureInfo.GetCultureInfo("vi-VN") : CultureInfo.InvariantCulture;

    public static string Lookup(string text)
    {
        if (!IsVietnamese || string.IsNullOrEmpty(text))
        {
            return text;
        }

        return Translations.Vi.TryGetValue(text.Trim(), out var translated)
            ? Preserve(text, translated)
            : text;
    }

    public string this[string text] => Lookup(text);

    // Platform content (guidelines, foods, recipes, ...) stored in English; admin-added content is shown as entered.
    public string C(string? text) => ContentTranslations.Translate(text);

    // Quantities such as "3/4 cup cooked" or "to taste".
    public string Q(string? quantity) => ContentTranslations.TranslateQuantity(quantity);

    public string F(string text, params object?[] args) =>
        string.Format(CultureInfo.CurrentCulture, Lookup(text), args);

    // Keep any leading/trailing whitespace of the original so layout spacing is unchanged.
    private static string Preserve(string original, string translated)
    {
        var start = original.Length - original.TrimStart().Length;
        var end = original.Length - original.TrimEnd().Length;
        return original[..start] + translated + original[^end..];
    }
}

// Lets MVC data annotations (display names, validation messages) use the same dictionary.
public sealed class DictionaryStringLocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource) => new DictionaryStringLocalizer();

    public IStringLocalizer Create(string baseName, string location) => new DictionaryStringLocalizer();

    private sealed class DictionaryStringLocalizer : IStringLocalizer
    {
        public LocalizedString this[string name] => Get(name);

        public LocalizedString this[string name, params object[] arguments] =>
            new(name, string.Format(CultureInfo.CurrentCulture, Translator.Lookup(name), arguments), false);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
            Translations.Vi.Select(pair => new LocalizedString(pair.Key, pair.Value, false));

        private static LocalizedString Get(string name)
        {
            var value = Translator.Lookup(name);
            return new LocalizedString(name, value, resourceNotFound: ReferenceEquals(value, name));
        }
    }
}

// Gives the built-in data annotation attributes an English message template, so the same dictionary
// can translate "The {0} field is required." and friends (otherwise the framework default is never localized).
public sealed class LocalizedValidationAttributeAdapterProvider : Microsoft.AspNetCore.Mvc.DataAnnotations.IValidationAttributeAdapterProvider
{
    private static readonly Dictionary<Type, string> Defaults = new()
    {
        [typeof(System.ComponentModel.DataAnnotations.RequiredAttribute)] = "The {0} field is required.",
        [typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute)] = "The field {0} must be a string or array type with a maximum length of '{1}'.",
        [typeof(System.ComponentModel.DataAnnotations.MinLengthAttribute)] = "The field {0} must be a string or array type with a minimum length of '{1}'.",
        [typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute)] = "The field {0} must be a string with a maximum length of {1}.",
        [typeof(System.ComponentModel.DataAnnotations.RangeAttribute)] = "The field {0} must be between {1} and {2}.",
        [typeof(System.ComponentModel.DataAnnotations.EmailAddressAttribute)] = "The {0} field is not a valid e-mail address.",
    };

    private readonly Microsoft.AspNetCore.Mvc.DataAnnotations.ValidationAttributeAdapterProvider inner = new();

    public Microsoft.AspNetCore.Mvc.DataAnnotations.IAttributeAdapter? GetAttributeAdapter(
        System.ComponentModel.DataAnnotations.ValidationAttribute attribute,
        IStringLocalizer? stringLocalizer)
    {
        if (string.IsNullOrEmpty(attribute.ErrorMessage) && Defaults.TryGetValue(attribute.GetType(), out var template))
        {
            attribute.ErrorMessage = template;
        }

        return inner.GetAttributeAdapter(attribute, stringLocalizer);
    }
}

// Labels for properties without [Display]: "SourceChapter" becomes "Source chapter", then goes through the dictionary.
public sealed class HumanizedDisplayNameProvider : Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.IDisplayMetadataProvider
{
    public void CreateDisplayMetadata(Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DisplayMetadataProviderContext context)
    {
        if (context.Key.MetadataKind != Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.ModelMetadataKind.Property ||
            context.Key.Name is not { Length: > 0 } name ||
            context.DisplayMetadata.DisplayName is not null)
        {
            return;
        }

        var humanized = System.Text.RegularExpressions.Regex.Replace(name, "(?<=[a-z0-9])(?=[A-Z])", " ");
        humanized = humanized[..1] + humanized[1..].ToLowerInvariant();
        context.DisplayMetadata.DisplayName = () => Translator.Lookup(humanized);
    }
}
