// By Brevin Bell, and Charles Li
using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // starting the server
            Settings settings = new Settings("..\\..\\..\\..\\Resources\\settings.xml"); // go to settings
            ServerController serverController = new ServerController(settings);
            serverController.Start();
            Console.Read();
        }
    }
}
