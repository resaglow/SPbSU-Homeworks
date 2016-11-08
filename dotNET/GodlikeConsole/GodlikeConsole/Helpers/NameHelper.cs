using System;
using GodlikeConsole.Humans;

namespace GodlikeConsole.Helpers
{
    internal static class NameHelper
    {
        private const int LengthMiddleNameSuffix = 4;
        private const string MiddleNameMaleSuffix = "ович";
        private const string MiddleNameFemaleSuffix = "овна";

        internal static string GetFatherName(string childMiddleName)
        {
            if (string.IsNullOrEmpty(childMiddleName))
            {
                throw new ArgumentNullException();
            }
            return childMiddleName.Substring(0, childMiddleName.Length - LengthMiddleNameSuffix);
        }

        internal static string GetChildMiddleName(string fatherName, Sex sex)
        {
            if (string.IsNullOrEmpty(fatherName))
            {
                throw new ArgumentNullException();
            }
            return fatherName + (
                sex == Sex.Male
                ? MiddleNameMaleSuffix
                : MiddleNameFemaleSuffix);
        }
    }
}
