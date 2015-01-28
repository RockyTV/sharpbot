using System;
using System.IO;
using ChatSharp;
using sharpbot.Utils;
using System.Reflection;
using System.Collections.Generic;

namespace sharpbot
{
    // Original code by Joshua Blake <http://github.com/JoshBlake/>
    public static class ModuleHandler
    {
        private static readonly List<IModule> loadedModules = new List<IModule>();

        public static IrcClient client;

        static ModuleHandler()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName == args.Name)
                {
                    return assembly;
                }
            }
            return null;
        }

        public static void LoadModules(IrcClient cli)
        {
            client = cli;
            string moduleDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");
            if (!Directory.Exists(moduleDirectory))
            {
                Directory.CreateDirectory(moduleDirectory);
            }
            Logger.Log("Loading modules...");

            List<Assembly> loadedAssemblies = new List<Assembly>();
            string[] moduleFiles = Directory.GetFiles(moduleDirectory, "*", SearchOption.AllDirectories);
            foreach (string moduleFile in moduleFiles)
            {
                string prettyFile = string.Format("{0}/{1}", Path.GetFileName(moduleDirectory), Path.GetFileName(moduleFile));
                if (Path.GetExtension(moduleFile).ToLower() == ".dll")
                {
                    try
                    {
                        Assembly loadedAssembly = Assembly.UnsafeLoadFrom(moduleFile);
                        loadedAssemblies.Add(loadedAssembly);
                        Logger.Log(string.Format("Loaded: {0}", prettyFile));
                    }
                    catch (NotSupportedException)
                    {
                        Logger.Log("Can't load DLL, perhaps it is blocked: " + prettyFile);
                    }
                    catch (Exception e)
                    {
                        Logger.Log("Error while loading module " + moduleFile);
                    }
                }
            }

            Type moduleType = typeof(IModule);

            foreach (Assembly loadedAssembly in loadedAssemblies)
            {
                Type[] loadedTypes = loadedAssembly.GetExportedTypes();
                foreach (Type loadedType in loadedTypes)
                {
                    Type[] typeInterfaces = loadedType.GetInterfaces();
                    bool containsModuleInterface = false;
                    foreach (Type typeInterface in typeInterfaces)
                    {
                        if (typeInterface == moduleType)
                        {
                            containsModuleInterface = true;
                        }
                    }
                    if (containsModuleInterface)
                    {
                        Logger.Log("Loading module: " + loadedType.FullName);

                        try
                        {
                            IModule moduleInstance = ActivateModuleType(loadedType)
;

                            if (moduleInstance != null)
                            {
                                Logger.Log("Loaded module: " + loadedType.FullName);

                                loadedModules.Add(moduleInstance);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Error loading module " + loadedType.FullName + "(" + loadedType.Assembly.FullName + ")");
                            Logger.Log("Exception: " + ex.ToString());
                        }
                    }
                }
            }
            Logger.Log("Loaded modules!");
        }

        private static IModule ActivateModuleType(Type loadedType)
        {
            try
            {
                IModule moduleInstance = Activator.CreateInstance(loadedType) as IModule;
                return moduleInstance;
            }
            catch (Exception e)
            {
                Logger.Log("Cannot activate module " + loadedType.Name);
                Logger.Log("Exception: " + e.ToString());
                return null;
            }
        }

        public static void OnMessageReceived(string sender, string msg)
        {
            foreach (IModule module in loadedModules)
            {
                try
                {
                    module.OnMessageReceived(sender, msg);
                }
                catch (Exception e)
                {
                    Logger.Log(string.Format("Error thrown in 'OnMessageReceived' event for {0} ({1}): {2}", module.GetType().FullName, module.GetType().Assembly.FullName, e.Message));
                }
            }
        }

        public static void OnMessageReceived(string sender, string msg, string channel)
        {
            foreach (IModule module in loadedModules)
            {
                try
                {
                    module.OnMessageReceived(sender, msg, channel);
                }
                catch (Exception e)
                {
                    Logger.Log(string.Format("Error thrown in 'OnMessageReceived' event for {0} ({1}): {2}", module.GetType().FullName, module.GetType().Assembly.FullName, e.Message));
                }
            }
        }

        public static void OnUserJoin(string user, string channel)
        {
            foreach (IModule module in loadedModules)
            {
                try
                {
                    module.OnUserJoin(user, channel);
                }
                catch (Exception e)
                {
                    Logger.Log(string.Format("Error thrown in 'OnUserJoin' event for {0} ({1}): {2}", module.GetType().FullName, module.GetType().Assembly.FullName, e.Message));
                }
            }
        }

        public static void OnUserLeave(string user, string channel)
        {
            foreach (IModule module in loadedModules)
            {
                try
                {
                    module.OnUserLeave(user, channel);
                }
                catch (Exception e)
                {
                    Logger.Log(string.Format("Error thrown in 'OnUserLeave' event for {0} ({1}): {2}", module.GetType().FullName, module.GetType().Assembly.FullName, e.Message));
                }
            }
        }

        public static void SendMessage(string message, string channel)
        {
            if (channel.StartsWith("#"))
            {
                client.Channels[channel].SendMessage(message);
            }
            else
            {
                SendPrivateMessage(message, channel);
            }
        }

        public static void SendPrivateMessage(string message, string user)
        {
            client.SendMessage(message, user);
        }
    }
}
