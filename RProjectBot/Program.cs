using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using Discord;

namespace RProjectBot
{
    class Program
    {
        private static ConsoleCancelEventHandler saveDelegate;
        static void Main(string[] args)
        {
           
            var client = new DiscordClient();
            List<ICommand> Commands = new List<ICommand>();
            List<IPermanent> Permanents = new List<IPermanent>();

            client.Log.Message += Log_Message;
            saveDelegate = (object o, ConsoleCancelEventArgs e) =>
            {
                Permanents.ForEach(p => { p.Save(client); });
                client.Log.Log(LogSeverity.Info, "botchan", "Everything has been saved.");
                Thread.Sleep(500);
            };
            Console.CancelKeyPress += saveDelegate;


            
            var searchspace = @"RPgrojectBot";
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == searchspace && (t.GetInterfaces().Contains(typeof(ICommand)) || t.GetInterfaces().Contains(typeof(IPermanent)))
                    select t;
            foreach (Type t in q)
            {
                var obj = t.InvokeMember("", BindingFlags.CreateInstance, null, null, null);
                if (t.GetInterfaces().Contains(typeof(ICommand)))
                {
                    Commands.Add(obj as ICommand);
                }
                if(t.GetInterfaces().Contains(typeof(IPermanent)))
                {
                    Permanents.Add(obj as IPermanent);
                }
            }

            foreach(IPermanent p in Permanents)
            {
                p.Load(client);
            }

            client.MessageReceived += async (s, e) =>
            {
                if (e.User.Id != client.CurrentUser.Id)
                {
                    var validCommands = Commands.Where(c => { return e.Message.RawText.StartsWith(c.getToken()); });
                    if (validCommands.Count() > 0)
                    {
                        var longestCommandLen = validCommands.Max(c => { return c.getToken().Length; });
                        var command = validCommands.Where(c => { return longestCommandLen == c.getToken().Length; });
                        await command.First().execute(e, e.Message.RawText.Substring(longestCommandLen).Trim(' '), client);
                    }
                    else if (e.Message.RawText == "-save" && e.User.Id == 95543627391959040)
                    {
                        await e.Channel.SendMessage("Saved");
                        Permanents.ForEach(p => { p.Save(client); });
                    }
                    else if (e.Message.RawText == "-load" && e.User.Id == 95543627391959040)
                    {
                        await e.Channel.SendMessage("Loaded");
                        Permanents.ForEach(p => { p.Load(client); });
                    }
                }

            };

            client.ExecuteAndWait(async () =>
            {
                await client.Connect(Sensitive.Token);
                Thread.Sleep(100);
                client.Log.Log(LogSeverity.Info, "botchan", "RProjectBot.", null);
                client.Log.Log(LogSeverity.Info, "botchan", "Currently on servers: ", null);
                foreach (Server s in client.Servers)
                {
                    client.Log.Log(LogSeverity.Info, "botchan", "    " + s.Name + " with " + s.Users.Count() + " Users.", null);
                }
            });
        }

        private static void Log_Message(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine("[" + e.Severity + "] " + e.Source + ": " + e.Message);
        }
    }
    
}
