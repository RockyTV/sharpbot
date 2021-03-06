﻿using System;
using System.IO;
using ChatSharp;
using System.Text;
using sharpbot.Utils;
using ChatSharp.Events;
using System.Collections.Generic;

namespace sharpbot
{
    public class SharpBot
    {
        public string Server { get; internal set; }
        public string Nick { get; internal set; }
        public string Password { get; internal set; }
        public IrcUser User { get; internal set; }
        public IrcClient Client { get; internal set; }

        public List<string> Channels { get; set; }

        internal SharpBot() 
        {
        }

        public SharpBot(string server, string nick)
        {
            Server = server;
            Nick = nick;
            User = new IrcUser(Nick, "sharpbot", Password, "sharpbot");
            Client = new IrcClient(Server, User, false, false);

            Channels = new List<string>();

            Client.RawMessageRecieved += RawMessageReceived;
            Client.RawMessageSent += RawMessageSent;
            Client.ConnectionComplete += ConnectionComplete;
            Client.ChannelMessageRecieved += ChannelMessageReceived;
            Client.UserMessageRecieved += UserMessageReceived;
            Client.UserJoinedChannel += UserJoinedChannel;
            Client.UserPartedChannel += UserPartedChannel;
        }

        public SharpBot(string server, string nick, string password) : this(server, nick)
        {
            Password = password;
        }

        private void ConnectionComplete(object sender, EventArgs args)
        {
            ModuleHandler.LoadModules((IrcClient)sender);

            foreach (string channel in Channels)
            {
                Client.JoinChannel(channel);
            }
        }

        private static void ChannelMessageReceived(object sender, PrivateMessageEventArgs args)
        {
            Logger.Log(string.Format("{0}: <{1}> {2}", args.PrivateMessage.Source, args.PrivateMessage.User.Nick, args.PrivateMessage.Message));
            if (IsUser(args.PrivateMessage.Source))
            {
                Logger.WriteChannelLog(args.PrivateMessage.User.Nick, args.PrivateMessage.Message, args.PrivateMessage.User.Nick);
            }
            else
            {
                Logger.WriteChannelLog(args.PrivateMessage.User.Nick, args.PrivateMessage.Message, args.PrivateMessage.Source);
            }
            ModuleHandler.OnMessageReceived(args.PrivateMessage.User.Nick, args.PrivateMessage.Message, args.PrivateMessage.Source);
        }

        private static void RawMessageReceived(object sender, RawMessageEventArgs args)
        {
            if (!args.Message.Contains("PRIVMSG"))
            {
                if (args.Message.Contains("PONG"))
                {
                    Logger.Message(string.Format("PONG reply from {0} {1}", args.Message.Substring(1, args.Message.IndexOf(" ") - 1), args.Message.Substring(args.Message.LastIndexOf(":"))));
                }
                else
                {
                    Logger.Message(args.Message);
                }
            }
        }

        private static void RawMessageSent(object sender, RawMessageEventArgs args)
        {
            if (!args.Message.Contains("PING"))
            {
                Logger.Message(string.Format("SENT {0}", args.Message));
            }
            else
            {
                Logger.Message(string.Format("Sent PING request {0}", args.Message.Substring(args.Message.LastIndexOf(":"))));
            }
        }

        private static void UserMessageReceived(object sender, PrivateMessageEventArgs args)
        {
            Logger.Log(string.Format("<{0}> {1}", args.PrivateMessage.User.Nick, args.PrivateMessage.Message));
            ModuleHandler.OnMessageReceived(args.PrivateMessage.User.Nick, args.PrivateMessage.Message);
            ModuleHandler.OnMessageReceived(args.PrivateMessage.User.Nick, args.PrivateMessage.Message, args.PrivateMessage.User.Nick);
        }

        private void UserJoinedChannel(object sender, ChannelUserEventArgs args)
        {
            if (args.User.Nick != this.Nick)
            {
                Logger.Log(string.Format("{0} has joined {1}", args.User.Nick, args.Channel.Name));
                Logger.WriteChannelLog(args.User.Nick, "has joined the channel", args.Channel.Name);
                ModuleHandler.OnUserJoin(args.User.Nick, args.Channel.Name);
            }
        }

        private void UserPartedChannel(object sender, ChannelUserEventArgs args)
        {
            if (args.User.Nick != this.Nick)
            {
                Logger.Log(string.Format("{0} has left {1}", args.User.Nick, args.Channel.Name));
                Logger.WriteChannelLog(args.User.Nick, "has left the channel", args.Channel.Name);
                ModuleHandler.OnUserLeave(args.User.Nick, args.Channel.Name);
            }
        }

        private static bool IsUser(string channel)
        {
            return channel.StartsWith("#");
        }
    }
}
