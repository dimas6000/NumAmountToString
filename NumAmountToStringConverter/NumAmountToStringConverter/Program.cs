using System;

namespace NumAmountToStringConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a number to convert to English equivalent or 'exit' to close the program:");
            while (true)
            {
                try
                {
                    var inputStr = Console.ReadLine();
                    if (inputStr.ToLower().Contains("exit"))
                        Environment.Exit(0);

                    var result = NumAmountToString.ConvertNumAmountToString(inputStr);
                    Console.WriteLine(result);

                }
                catch (NumAmountToStringException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            Console.ReadLine();
        }

    }
}