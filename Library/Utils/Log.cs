using System;
using System.IO;

namespace sharpbot.Utils
{
    public class Logger
    {
        public static string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static string logFile = Path.Combine(logDirectory, string.Format("sharpbot {0}.log", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));

        private static void WriteLog(string message)
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string output = string.Format("[{0}]: {1}", DateTime.Now.ToString("HH:mm:ss"), message);
            Console.WriteLine(output);

            try
            {
                File.AppendAllText(logFile, string.Format("{0}{1}", output, Environment.NewLine));
            }
            catch (Exception e)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error writing to log file!");
                Console.WriteLine(e.ToString());
                Console.ForegroundColor = defaultColor;
            }
        }

        public static void WriteChannelLog(string user, string message, string channel)
        {
            try
            {
                string output = string.Format("[{0}] <{1}> {2}{3}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), user, message, Environment.NewLine);
                string channelLogFile = Path.Combine(Logger.logDirectory, string.Format("{0}.log", channel));

                File.AppendAllText(channelLogFile, output);
            }
            catch (Exception e)
            {
                Logger.Error("Error while writing to channel log file!");
                Logger.Error(e.ToString());
            }
        }

        public static void Log(string msg)
        {
            WriteLog(msg);
        }

        public static void Message(string msg)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteLog(msg);
            Console.ForegroundColor = defaultColor;
        }

        public static void Error(string msg)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            WriteLog(msg);
            Console.ForegroundColor = defaultColor;
        }
    }
}
