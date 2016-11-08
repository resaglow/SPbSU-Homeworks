using System;

namespace GodlikeConsole.Humans
{
    internal class Parent : Human
    {
        internal int AmountChildren { get; }

        protected override ConsoleColor FontColor()
          => ConsoleColor.Yellow;

        public Parent(string name, int age, Sex gender, int amountChildren) : base(name, age, gender)
        {
            if (amountChildren < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            AmountChildren = amountChildren;
        }

        internal override void Print()
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Parent {Name} of age {Age} has {AmountChildren} children");
            Console.ForegroundColor = foregroundColor;
        }
    }
}
