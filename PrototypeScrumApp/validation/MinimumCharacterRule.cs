using System.Globalization;
using System.Windows.Controls;

namespace PrototypeScrumApp.validation
{
    internal class MinimumCharacterRule : ValidationRule
    {
        public int MinimumCharacters { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"Use atleast {MinimumCharacters} characters.");
            return new ValidationResult(true, null);
        }
    }
}
