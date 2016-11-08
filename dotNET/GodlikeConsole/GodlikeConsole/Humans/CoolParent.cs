using System;

namespace GodlikeConsole.Humans
{
    internal sealed class CoolParent : Parent
    {
        internal double AmountMoney { get; }

        protected override ConsoleColor FontColor()
          => ConsoleColor.Green;

        internal CoolParent(string name, int age, Sex gender, int amountChildren, double amountMoney)
          : base(name, age, gender, amountChildren)
        {
            if (amountMoney < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            AmountMoney = amountMoney;
        }

        internal override void Print()
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("Cool Parent {0} {1} of age {2} has {3} children, ", Name, Sex, Age, AmountChildren);

            ConsoleColor background = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("${0:N2}", AmountMoney);
            Console.BackgroundColor = background;
            Console.WriteLine(" money");

            Console.ForegroundColor = foregroundColor;
        }
    }
}
