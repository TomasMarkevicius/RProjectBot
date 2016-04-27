using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace RProjectBot
{
    interface ICommand
    {
        string getToken();
        Task execute(MessageEventArgs e, string args, DiscordClient client);
    }
}
