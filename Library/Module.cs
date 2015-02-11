using System;
using ChatSharp;

namespace sharpbot
{
    public interface IModule
    {
        void OnUserJoin(string user, string channel);
        void OnUserLeave(string user, string channel);
        void OnMessageReceived(string sender, string receivedMessage);
        void OnMessageReceived(string sender, string receivedMessage, string channel);
    }

    public abstract class PluginModule : IModule
    {
        public virtual void OnUserJoin(string user, string channel) { }
        public virtual void OnUserLeave(string user, string channel) { }
        public virtual void OnMessageReceived(string sender, string receivedMessage) { }
        public virtual void OnMessageReceived(string sender, string receivedMessage, string channel) { }
    }
}
