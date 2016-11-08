using System;
using GodlikeConsole.Humans;

namespace GodlikeConsole.Helpers
{
    internal static class RandomDataProvider
    {
        private static readonly Random Random = new Random();

        private const int AmountHumanTypes = 4;
        private const int AmountHumanFemaleTypes = 2;
        private const int AmountGenders = 2;

        private const int MinStudentAge = 18;
        private const int MaxStudentAge = 36;
        private const int MinParentAge = 50;
        private const int MaxParentAge = 75;
        private const int MaxAmountChildren = 5;
        private const int MinAverageMark = 4;
        private const int MaxAverageMark = 6;
        private const int MaxMoneyValue = 10000;

        private static readonly string[] MaleNames = { "Владимир", "Антон", "Леонид", "Олег" };
        private static readonly string[] FemaleNames = { "Елизавета", "Юлия", "Валерия", "Диана" };

        internal static Human RandomHuman(Sex sex)
        {
            Human human;
            var amountTypes = sex == Sex.Male ? AmountHumanTypes : AmountHumanFemaleTypes;
            var type = (HumanType)Random.Next(amountTypes);
            switch (type)
            {
                case HumanType.Student:
                    human = RandomStudent(sex);
                    break;
                case HumanType.Botan:
                    human = RandomBotan(sex);
                    break;
                case HumanType.CoolParent:
                    human = RandomCoolParent();
                    break;
                case HumanType.Parent:
                    human = RandomParent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return human;
        }

        internal static Sex RandomGender()
        {
            Array values = Enum.GetValues(typeof(Sex));
            return (Sex)values.GetValue(Random.Next(values.Length));
        }

        internal static string RandomName(Sex gender)
          => gender == Sex.Male ? RandomMaleName() : RandomFemaleName();

        internal static int RandomStudentAge()
          => Random.Next(MinStudentAge, MaxStudentAge);

        internal static int RandomParentAge()
          => Random.Next(MinParentAge, MaxParentAge);

        internal static int RandomAmountChildren()
          => Random.Next(MaxAmountChildren);

        private static Student RandomStudent(Sex gender)
          => new Student(RandomName(gender), RandomStudentAge(), gender, RandomMiddleName(gender));

        private static Parent RandomParent()
          => new Parent(RandomName(Sex.Male), RandomParentAge(), Sex.Male, RandomAmountChildren());

        private static Botan RandomBotan(Sex gender)
          => new Botan(RandomName(gender), RandomStudentAge(), gender, RandomMiddleName(gender), RandomAverageMark());

        private static CoolParent RandomCoolParent()
          => new CoolParent(RandomName(Sex.Male), RandomParentAge(), Sex.Male, RandomAmountChildren(), RandomAmountMoney());

        private static string RandomMaleName()
          => MaleNames[Random.Next(MaleNames.Length)];
        
        private static string RandomFemaleName()
          => FemaleNames[Random.Next(MaleNames.Length)];

        private static string RandomMiddleName(Sex gender)
          => gender == Sex.Male ? RandomMaleMiddleName() : RandomFemaleMiddleName();

        private static string RandomMaleMiddleName()
          => NameHelper.GetChildMiddleName(MaleNames[Random.Next(MaleNames.Length)], Sex.Male);

        private static string RandomFemaleMiddleName()
          => NameHelper.GetChildMiddleName(MaleNames[Random.Next(MaleNames.Length)], Sex.Female);

        private static double RandomAverageMark()
          => (double)Random.Next(MinAverageMark * 100 + 1, MaxAverageMark * 100) / 100;

        private static int RandomAmountMoney()
          => Random.Next(MaxMoneyValue);
    }
}
