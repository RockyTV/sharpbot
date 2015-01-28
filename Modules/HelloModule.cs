using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sharpbot;
using sharpbot.Utils;

namespace botmodules
{
    public class HelloModule : Module
    {
        public override void OnMessageReceived(string sender, string message, string channel)
        {
            if (message == "!hello")
            {
                ModuleHandler.SendMessage("Hello!", channel);
            }

            if (message.StartsWith("!hello "))
            {
                string target = message.Substring(7);
                if (ModuleHandler.client.Channels[channel].Users.Contains(target))
                {
                    ModuleHandler.SendMessage("Hello, " + target + "!", channel);
                }
                else
                {
                    ModuleHandler.SendPrivateMessage("Looks like that the user '" + target + "' is not on the channel '" + channel + "'", sender);
                }
            }
        }
    }

    public class AdminCommands : Module
    {
        public override void OnMessageReceived(string sender, string receivedMessage, string channel)
        {
            if (sender == "RockyTV")
            {
                if (receivedMessage.StartsWith("!join "))
                {
                    string targetChannel = receivedMessage.Substring(6);
                    ModuleHandler.client.JoinChannel(targetChannel);
                    ModuleHandler.SendPrivateMessage("Joined " + targetChannel + ".", sender);
                }
            }
        }
    }

    public class Botsnack : Module
    {
        public override void OnMessageReceived(string sender, string receivedMessage, string channel)
        {
            string[] responses = new string[] 
                {
                    "Om nom nom!",
                    "That's very nice of you!",
                    "Oh thx, have a cookie yourself!",
                    "Thank you very much.",
                    "Thanks for the treat!" 
                };

            if (receivedMessage.ToLower().Contains("botsnack"))
            {
                Random rand = new Random();
                ModuleHandler.SendMessage(responses[rand.Next(responses.Length)], channel);
            }
        }
    }

    public class Ackbar : Module
    {
        static string[] ackbars = new string[]
        {
            "http://i.imgur.com/OTByx1b.jpg",
            "http://farm4.static.flickr.com/3572/3637082894_e23313f6fb_o.jpg",
            "http://6.asset.soup.io/asset/0610/8774_242b_500.jpeg",
            "http://files.g4tv.com/ImageDb3/279875_S/steampunk-ackbar.jpg",
            "http://farm6.staticflickr.com/5126/5725607070_b80e61b4b3_z.jpg",
            "http://farm6.static.flickr.com/5291/5542027315_ba79daabfb.jpg",
            "http://farm6.staticflickr.com/5250/5216539895_09f963f448_z.jpg",
            "http://static.fjcdn.com/pictures/Its_2031a3_426435.jpg",
            "http://www.millionaireplayboy.com/mpb/wp-content/uploads/2011/01/1293668358_bottom_trappy.jpeg",
            "http://31.media.tumblr.com/tumblr_lqrrkpAqjf1qiorsyo1_500.jpg",
            "https://i.chzbgr.com/maxW500/4930876416/hB0F640C6/",
            "http://i.qkme.me/356mr9.jpg",
            "http://24.media.tumblr.com/e4255aa10151ebddf57555dfa3fc8779/tumblr_mho9v9y5hE1r8gxxfo1_500.jpg",
            "http://farm2.staticflickr.com/1440/5170210261_fddb4c480c_z.jpg",
            "http://fashionablygeek.com/wp-content/uploads/2010/02/its-a-mouse-trap.jpg?cb5e28",
            "http://31.media.tumblr.com/tumblr_lmn8d1xFXN1qjs7yio1_500.jpg"
                                  
        };
        public override void OnMessageReceived(string sender, string receivedMessage, string channel)
        {
            if (receivedMessage.ToLower().Contains("it's a trap") || receivedMessage.ToLower().Contains("its a trap"))
            {
                Random rand = new Random();

                ModuleHandler.SendMessage(ackbars[rand.Next(ackbars.Length)], channel);
            }
        }
    }
}
