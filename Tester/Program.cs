using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sharpbot;

class Program
{
    static void Main()
    {
        Console.WindowWidth = 100;
        Console.Title = "sharpbot";
        SharpBot bot = new SharpBot("irc.esper.net", "testbot", "");
        bot.Channels.Add("#rockytv");
        bot.Channels.Add("#dmp");

        bot.Client.ConnectAsync();

        while (true) ;
    }
}
