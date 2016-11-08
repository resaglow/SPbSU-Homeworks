using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using GodlikeConsole.Humans;
using GodlikeConsole.Properties;

namespace GodlikeConsole
{
    static class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            var day = DateTime.Now.DayOfWeek;
            if (day == DayOfWeek.Sunday)
            {
                Console.WriteLine(Resources.HolySunday);
            }
            else
            {
                Console.WriteLine(Resources.Welcome);
                Console.WriteLine(Resources.InputInvitation);

                var amount = 0;
                do
                {
                    Console.WriteLine(Resources.InputRetry);
                    int.TryParse(Console.ReadLine(), out amount);
                } while (amount < 1);

                var god = new God();
                var humans = new List<Human>(god.GenerateHumans(amount));

                PrintHumans(humans);

                Thread.Sleep(1000);

                var pairs = new List<Human>();
                foreach (var human in humans)
                {
                    pairs.Add(god.CreatePair(human));
                }

                Console.SetCursorPosition(0, Console.CursorTop - humans.Count * 2 + 1);
                PrintPairs(pairs);
                Console.SetCursorPosition(0, Console.CursorTop - 1);

                DumpMoneyAmount(god.GetTotalMoney());
            }
        }

        static void PrintHumans(List<Human> humans)
        {
            if (humans == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var human in humans)
            {
                human.Print();
                Console.WriteLine();
            }
        }

        static void PrintPairs(List<Human> pairs)
        {
            if (pairs == null)
            {
                throw new ArgumentNullException();
            }

            ConsoleColor background = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            foreach (Human pair in pairs)
            {
                pair.Print();
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }
            Console.BackgroundColor = background;
        }

        static void DumpMoneyAmount(double amount)
        {
            const string logFilename = "log.txt";

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            File.WriteAllText(logFilename, String.Format("${0:N2}", amount));
            Console.WriteLine(Resources.MoneyLogged);
        }
    }
}
