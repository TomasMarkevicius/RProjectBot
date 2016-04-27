using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace RProjectBot
{
    class InfoCommand : ICommand
    {
        public async Task execute(MessageEventArgs e, string args, DiscordClient client)
        {
            await e.Channel.SendMessage("This is RProjectBot made by tiro and part");
        }

        public string getToken()
        {
            return "-info";
        }
    }
}
