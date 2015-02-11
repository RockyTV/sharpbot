using sharpbot;
using sharpbot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace botmodules
{
    public class TriviaModule : PluginModule
    {
        private static bool gameRunning = false;
        private static string category;
        private static string gameChannel = "";
        private static Dictionary<string, string[]> questions;
        private static Dictionary<string, string[]> answers;
        private static KeyValuePair<string, string> selectedQuestion;

        public TriviaModule()
        {
            Logger.Log("TriviaModule started.");

            questions = new Dictionary<string, string[]>();
            questions.Add("GENERAL", new string[] {
             "Who created Microsoft?"   
            });

            answers = new Dictionary<string, string[]>();
            answers.Add("GENERAL", new string[] {
             "Bill Gates"   
            });

            selectedQuestion = new KeyValuePair<string, string>();
        }

        public override void OnMessageReceived(string sender, string receivedMessage, string channel)
        {
            if (receivedMessage.StartsWith("!trivia "))
            {
                string trimmedMessage = receivedMessage.Substring(8);
                string inputCategory = trimmedMessage.ToUpper();
                if (!questions.ContainsKey(inputCategory))
                {
                    ModuleHandler.SendMessage(string.Format("Category '{0}' does not exist.", inputCategory), channel);
                }
                if (!gameRunning)
                {
                    StartGame(category);
                    gameChannel = channel;
                }
                else
                {
                    ModuleHandler.SendMessage("A game is already in progress!", channel);
                }
            }
        }

        private static void StartGame(string cat)
        {
            category = cat;
            gameRunning = true;
            ModuleHandler.SendMessage(string.Format("Trivia game started. Category: {0}", category), gameChannel);
            //selectedQuestion = new KeyValuePair<string, string>(questions.);
        }
    }
}
