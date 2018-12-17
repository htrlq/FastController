using FastController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new FastControllerFactory();
            factory.Register<UserController>();
            factory.Use("127.0.0.1", 8008);

            Console.ReadLine();
        }
    }
}
