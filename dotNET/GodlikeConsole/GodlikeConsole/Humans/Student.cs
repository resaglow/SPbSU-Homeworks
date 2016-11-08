using System;

namespace GodlikeConsole.Humans
{
    internal class Student : Human
    {
        internal string MiddleName { get; }

        protected override ConsoleColor FontColor()
            => ConsoleColor.Blue;

        internal Student(string name, int age, Sex gender, string middleName) : base(name, age, gender)
        {
            if (string.IsNullOrEmpty(middleName))
            {
                throw new ArgumentOutOfRangeException();
            }
            MiddleName = middleName;
        }

        internal override void Print()
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Student {Name} {MiddleName} of age {Age}");
            Console.ForegroundColor = foregroundColor;
        }
    }
}
