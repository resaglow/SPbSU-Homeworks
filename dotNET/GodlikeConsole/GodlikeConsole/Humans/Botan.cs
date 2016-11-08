using System;

namespace GodlikeConsole.Humans
{
    internal sealed class Botan : Student
    {
        internal double AverageMark { get; }

        protected override ConsoleColor FontColor()
          => ConsoleColor.Cyan;

        internal Botan(string name, int age, Sex gender, string middleName, double averageMark)
          : base(name, age, gender, middleName)
        {
            if (averageMark < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            AverageMark = averageMark;
        }

        internal override void Print()
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Botan {Name} {MiddleName} of age {Age} has GPA {AverageMark:N2}");
            Console.ForegroundColor = foregroundColor;
        }
    }
}
