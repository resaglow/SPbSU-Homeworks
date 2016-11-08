using System;
using System.Collections.Generic;
using GodlikeConsole.Humans;
using GodlikeConsole.Helpers;

namespace GodlikeConsole
{
    internal sealed class God : IGod
    {
        private List<Human> Humans { get; } = new List<Human>();

        public Human CreateHuman()
            => CreateHuman(RandomDataProvider.RandomGender());

        public Human CreateHuman(Sex gender)
            => RandomDataProvider.RandomHuman(gender);

        public Human CreatePair(Human human)
        {
            if (human == null)
            {
                throw new ArgumentNullException();
            }
            return CreatePair(human, RandomDataProvider.RandomGender());
        }

        Human CreatePair(Human human, Sex gender)
        {
            if (human == null)
            {
                throw new ArgumentNullException();
            }

            Human newHuman;
            if (human is Botan)
            {
                newHuman = CreateCoolParentPair(human as Botan);
            }
            else if (human is CoolParent)
            {
                newHuman = CreateBotanPair(human as CoolParent, gender);
            }
            else if (human is Student)
            {
                newHuman = CreateParentPair(human as Student);
            }
            else if (human is Parent)
            {
                newHuman = CreateStudentPair(human as Parent, gender);
            }
            else
            {
                throw new ArgumentNullException();
            }
            Humans.Add(newHuman);
            return newHuman;
        }

        internal List<Human> GenerateHumans(int humanCount)
        {
            for (int i = 0; i < humanCount; ++i)
            {
                Humans.Add(CreateHuman());
            }
            return GetHumans();
        }

        internal List<Human> GetHumans()
        {
            return Humans;
        }

        internal double GetTotalMoney()
        {
            double sum = 0;
            for (int i = 0; i < Humans.Count; i++)
            {
                sum += this[i];
            }
            return sum;
        }

        double this[int index]
        {
            get
            {
                if (index < 0 || index >= Humans.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return Humans[index] is CoolParent
                    ? (Humans[index] as CoolParent).AmountMoney
                    : 0;
            }
        }

        Student CreateStudentPair(Parent parent, Sex gender)
            => new Student(
              name: RandomDataProvider.RandomName(gender),
              age: RandomDataProvider.RandomStudentAge(),
              gender: gender,
              middleName: NameHelper.GetChildMiddleName(parent.Name, gender));

        Botan CreateBotanPair(CoolParent coolParent, Sex gender)
            => new Botan(
              name: RandomDataProvider.RandomName(gender),
              age: RandomDataProvider.RandomStudentAge(),
              gender: gender,
              middleName: NameHelper.GetChildMiddleName(coolParent.Name, gender),
              averageMark: Math.Log10(coolParent.AmountMoney));

        Parent CreateParentPair(Student student)
            => new Parent(
                name: NameHelper.GetFatherName(student.MiddleName),
                age: RandomDataProvider.RandomParentAge(),
                gender: Sex.Male,
                amountChildren: RandomDataProvider.RandomAmountChildren());

        CoolParent CreateCoolParentPair(Botan botan)
            => new CoolParent(
              name: NameHelper.GetFatherName(botan.MiddleName),
              age: RandomDataProvider.RandomParentAge(),
              gender: Sex.Male,
              amountChildren: RandomDataProvider.RandomAmountChildren(),
              amountMoney: Math.Pow(10, botan.AverageMark));
    }
}
