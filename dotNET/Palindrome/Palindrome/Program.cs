using System;

namespace Palindrome
{
    internal sealed class PalindromeChecker
    {
        private const string ignoredSymbols = " .,:;!?()\"\'_-";
        private static bool IsDelimiter(char chr)
            => ignoredSymbols.Contains(chr.ToString());

        public static bool IsPalindrome(string word)
        {
            if (word == null)
            {
                return false;
            }

            int start = 0;
            int end = word.Length - 1;
            while (start < end)
            {
                while (IsDelimiter(word[start]) && start < end)
                {
                    start++;
                }
                while (IsDelimiter(word[end]) && start < end)
                {
                    end--;
                }
                if (char.ToLower(word[start]) != char.ToLower(word[end]))
                {
                    return false;
                }
                start++;
                end--;
            }
            return true;
        }
    }

    internal sealed class Program
    {
        private const string descriptionOutput = "Palindrome checker. Pass empty string to stop.";
        private const string askStringOutput = "Enter string: ";
        private const string positiveResultOutput = "Input is a palindrome.";
        private const string negativeResultOutput = "Input is not a palindrome.";

        static void Main()
        {
            Console.WriteLine(descriptionOutput);
            while (true)
            {
                Console.WriteLine(askStringOutput);
                var inputStr = Console.ReadLine();
                if (inputStr.Equals(""))
                {
                    break;
                }
                else
                {
                    if (PalindromeChecker.IsPalindrome(inputStr))
                    {
                        Console.WriteLine(positiveResultOutput);
                    }
                    else
                    {
                        Console.WriteLine(negativeResultOutput);
                    }
                }
            }
        }
    }
}
