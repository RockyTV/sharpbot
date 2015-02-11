using sharpbot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace botmodules
{
    public class TimeModule : PluginModule
    {
        public override void OnMessageReceived(string sender, string receivedMessage, string channel)
        {
            if (receivedMessage.StartsWith("!time "))
            {
                string trimmedMessage = receivedMessage.Substring(5).Trim();
                if (trimmedMessage.Contains("+"))
                {
                    string utcZone = trimmedMessage.Substring(0, trimmedMessage.IndexOf("+") + 2).ToUpper();
                    Console.WriteLine(trimmedMessage.IndexOf("+").ToString());

                    ModuleHandler.SendMessage(DateTime.UtcNow.ToString("s"), channel);

                    ModuleHandler.SendPrivateMessage("UTC zone: " + utcZone, sender);
                }
            }
        }
    }
}
