using System;
using System.Text;

internal sealed class Program
{
	static void Main()
	{
		// Russian text for learning encodings :)
		Console.OutputEncoding = Encoding.UTF8;
		Console.WriteLine("Я - интеллектуальный калькулятор!");
		Console.WriteLine("Как тебя зовут?");
		var name = Console.ReadLine();

		var rnd = new Random();
		var fst = rnd.Next(1, 11);
		var snd = rnd.Next(1, 11);

		Console.WriteLine("Сколько будет {0} + {1}?", fst, snd);

		var result = 0;
		var isInt = Int32.TryParse(Console.ReadLine().Trim(), out result);
		if (isInt && result == fst + snd)
		{
			Console.WriteLine("Верно, {0}!", name);
		}
		else
		{
			Console.WriteLine("{0}, ты не прав", name);
		}
	}
}