using System;

namespace CQRS.Example
{
    public class Program
    {
        public static void Main()
        {
            try
            {

            }
            catch (Exception ex)
            {
                WriteException(ex);
            }

            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter key to continue...");
            Console.ReadLine();
        }

        private static void WriteException(Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
