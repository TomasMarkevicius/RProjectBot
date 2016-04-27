using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Logging;

namespace RProjectBot
{
    interface IPermanent
    {
        void Save(DiscordClient client);
        void Load(DiscordClient client);
    }
}
