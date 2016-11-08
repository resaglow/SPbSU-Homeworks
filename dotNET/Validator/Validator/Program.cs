using System;

namespace RegexpValidator
{
    internal sealed class Validator
    {
        private const string emailPattern = @"^[a-zA-Z_]([.]?[\w\-\d]){0,30}[@]([\w\d]{1,}[\.])+(([a-zA-Z]{2,4}|marketing|sales|support|abuse|security|postmaster|hostmaster|usenet|webmaster|museum))$";
        private const string zipCodeRusPattern = @"^([\d]){6}$";
        private const string zipCodeUSPattern = @"^([\d]){5}[-]([\d]){4}$";
        private const string phonePattern = @"^((\+7|8)( )?(\()?([\d]){3}(\))?( )?)?([\d]){3}(-| )?([\d]){2}(-| )?([\d]){2}$";
        private const string emergencyPhone = @"^((01|02|03|04)(0)?|(9)?(01|02|03|04))$";

        private static readonly System.Text.RegularExpressions.Regex emailRegexp = new System.Text.RegularExpressions.Regex(emailPattern);
        private static readonly System.Text.RegularExpressions.Regex zipCodeRusRegexp = new System.Text.RegularExpressions.Regex(zipCodeRusPattern);
        private static readonly System.Text.RegularExpressions.Regex zipCodeUSRegexp = new System.Text.RegularExpressions.Regex(zipCodeUSPattern);
        private static readonly System.Text.RegularExpressions.Regex phoneRegexp = new System.Text.RegularExpressions.Regex(phonePattern);
        private static readonly System.Text.RegularExpressions.Regex emergencyPhoneRegexp = new System.Text.RegularExpressions.Regex(emergencyPhone);

        private const string msgValidEmail = "Input is a valid email address.";
        private const string msgValidZip = "Input is a valid zip code.";
        private const string msgValidPhone = "Input is a valid phone number.";
        private const string msgInvalidInput = "Input in neither email not zip code nor phone number.";

        public static string Validate(string input)
            =>
                input == null ? null
                : emailRegexp.IsMatch(input) ? msgValidEmail
                : zipCodeRusRegexp.IsMatch(input) || zipCodeUSRegexp.IsMatch(input) ? msgValidZip
                : phoneRegexp.IsMatch(input) || emergencyPhoneRegexp.IsMatch(input) ? msgValidPhone
                : msgInvalidInput;
    }

    internal sealed class Program
    {
        private const string descriptionOutput = "Email/zip-code/phone-number validator. Pass empty string to exit.";
        private const string askStringOutput = "Enter string to recognize:";

        static void Main()
        {
            Console.WriteLine(descriptionOutput);
            string input = null;
            do
            {
                Console.WriteLine(askStringOutput);
                input = Console.ReadLine();
                if (input.Equals(""))
                {
                    return;
                }
                Console.WriteLine(Validator.Validate(input));
            } while (true);
        }
    }
}
