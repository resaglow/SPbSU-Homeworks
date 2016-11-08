using System;

namespace GodlikeConsole.Humans
{
    internal class Human
    {
        internal string Name { get; private set; }
        protected int Age { get; private set; }
        protected Sex Sex { get; private set; }

        protected virtual ConsoleColor FontColor()
          => ConsoleColor.Gray;

        protected Human(string name, int age, Sex sex)
        {
            if (string.IsNullOrEmpty(name) || age < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            Name = name;
            Age = age;
            Sex = sex;
        }

        internal virtual void Print()
            => Console.WriteLine("Human {0} {1} of age {2}", Name, Sex, Age);
    }

    internal enum Sex
    {
        Male,
        Female
    }

    internal enum HumanType
    {
        Student,
        Botan,
        CoolParent,
        Parent
    }
}
