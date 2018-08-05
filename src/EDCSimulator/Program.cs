using System;

namespace EDCSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            new SynchronousSocketListener().StartListening();

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey(true);
        }
    }
}
