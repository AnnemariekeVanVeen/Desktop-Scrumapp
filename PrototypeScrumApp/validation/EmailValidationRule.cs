using System;
using System.Globalization;
using System.Net.Mail;
using System.Windows.Controls;

namespace PrototypeScrumApp.validation
{
    internal class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                new MailAddress(value.ToString());
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Please enter a valid email.");
            }

            return new ValidationResult(true, null);
        }
    }
}