﻿using System;
using Saftbot.NET.Modules;
using Saftbot.NET.DBSystem;
using Discore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saftbot.NET.Commands
{
    public class Ping : Command
    {
        public override void InitializeVariables()
        {
            Name = "Ping";
            Description = "Measures the time it takes the bot to respond";
            PermsRequired = 0;
            Usage = new string[0];
        }
       
        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            TimeSpan timeSincePost = DateTime.Now - cmdinfo.Message.Timestamp;
            return $"{cmdinfo.MentionAuthor} Pong! Took {timeSincePost.TotalMilliseconds} ms";
        }
    }

    public class Say : Command
    {
        public override void InitializeVariables()
        {
            Name = "Say";
            Description = "Sends the message as if written by the Bot";
            PermsRequired = 0;
            Usage = new string[] { "<message>" };
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            cmdinfo.Message.Delete();
            return $"{String.Join(" ", cmdinfo.Arguments)}";
        }
    }

    public class EightBall : Command
    {
        private static string[] answers = {"It is certain", "It is decidedly so", "Without a doubt", "Yes, definitely",
                                            "You may rely on it", "As I see it, yes", "Most likely", "Outlook good", "Yes",
                                            "Signs point to yes", "Reply hazy try again", "Ask again later", "Better not tell you now",
                                            "Cannot predict now", "Concentrate and ask again", "Don't count on it", "My reply is no",
                                            "My sources say no", "Outlook not so good", "Very doubtful", "Nah m8", "That's retarded",
                                            "Are you stupid?", "Literally kill yourself", "Negative"};

        public override void InitializeVariables()
        {
            Name = "8ball";
            Description = "Rolls a fair 8ball";
            PermsRequired = 0;
            Usage = new string[0];
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            Random random = new Random();

            return answers[(random.Next(00, answers.Length))];
        }
    }

    public class Status : Command
    {
        public override void InitializeVariables()
        {
            Name = "Status";
            Description = "Gives Information about the server the bot is hosted on";
            PermsRequired = 0;
            Usage = new string[0];
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            return Utility.SystemSummary();
        }
    }

    public class MyID : Command
    {
        public override void InitializeVariables()
        {
            Name = "MyID";
            Description = "Tells you what your Discord-UID is";
            PermsRequired = 0;
            Usage = new string[0];
        }
        
        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            return $"{cmdinfo.MentionAuthor} , your ID is {cmdinfo.Author.UserID}";
        }
    }

    public class Laughter : Command
    {
        public override void InitializeVariables()
        {
            Name = "Laughter";
            Description = "Make the bot laugh";
            PermsRequired = 0;
            Usage = new string[0];
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            string vocals = "aeiou";
            string messsage = "";
            Random rng = new Random();
            int length = rng.Next(5, 10);

            for (int i = 0; i < length; i++)
            {
                messsage += "h";
                messsage += vocals[rng.Next(0, vocals.Length)];
            }

            return messsage;
        }
    }

    public class MakeOwnerAdmin : Command
    {
        public override void InitializeVariables()
        {
            Name = "MakeOwnerAdmin";
            Description = "Gives Current guild owner admin perms";
            PermsRequired = 0;
            Usage = new string[0];
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            //grab guilds owner from cache
            DiscordGuild guild = (cmdinfo.Shard.Cache.GetGuild(new Snowflake(cmdinfo.Guild.GuildID)));
            ulong ownerID = guild.OwnerId.Id;
            ulong guildID = guild.Id.Id;

            UserProfile owner = new UserProfile(ownerID, guildID);

            if(owner.IsAdmin)
            {
                return $"{owner.Mention} is already an Admin!";
            }
            else
            {
                owner.IsAdmin = true;
                return $"{owner.Mention} is now an Admin!";
            }
        }
    }

    public class WhoIs : Command
    {
        public override void InitializeVariables()
        {
            Name = "WhoIs";
            Description = "Report on the given user(s) permissions";
            PermsRequired = 0;
            Usage = new string[] { "<user mention(s)>" };
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            string message = "";

            foreach (DiscordUser mentionedUser in cmdinfo.Message.Mentions)
            {
                List<string> descriptions = new List<string>();
                UserProfile mentionedUserProfile = new UserProfile(mentionedUser.Id.Id, cmdinfo.Guild.GuildID);
                
                //TODO: [maybe] make !whois more dynamic
                if (mentionedUserProfile.IsAdmin)
                    descriptions.Add("an admin");
                if (mentionedUserProfile.IsIgnored)
                    descriptions.Add("ignored");
                if (mentionedUserProfile.IsDJ)
                    descriptions.Add("a DJ");
                if (mentionedUserProfile.IsDev)
                    descriptions.Add("a dev");

                if (descriptions.Count > 0)
                    message += $"{Utility.Mention(mentionedUser)} is {Utility.SpecialAndJoin(descriptions)}\n";
                else
                    message += $"{Utility.Mention(mentionedUser)} is a nobody\n";
            }

            return message;
        }
    }

    public class Repo : Command
    {
        public override void InitializeVariables()
        {
            Name = "Repo";
            Description = "Links to the Bot's source code";
            PermsRequired = 0;
            Usage = new string[0];
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            return "My source can be found at https://github.com/LordGruem/SaftbotNET";
        }
    }

    public class Help : Command
    {
        public override void InitializeVariables()
        {
            Name = "Help";
            Description = "describes all commands ";
            PermsRequired = 0;
            Usage = new string[] { "[<command>]" };
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            if(cmdinfo.Arguments.Length > 0)
            {
                foreach (Command cmd in Program.AllCommands)
                {
                    if (cmd.Name.ToLower() == cmdinfo.Arguments[0].ToLower())
                    {
                        return $"Usage: !{cmd.Name} {String.Join(' ',cmd.Usage)}";
                    }
                }

                return "Unknown command";
            }
            else
            {
                string message = "";

                foreach (Command cmd in Program.AllCommands)
                {
                    message += $"__**!{cmd.Name}**__: {cmd.Description}\n";
                }

                return message;
            }
        }
    }

    public class Square : Command
    {
        public override void InitializeVariables()
        {
            Name = "Square";
            Description = "Prints a message *squared*";
            PermsRequired = 0;
            Usage = new string[] { "<Message>" };
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            string input = String.Join(' ', cmdinfo.Arguments);
            
            string response = input;

            foreach (char chr in input.Substring(1))
            {
                response += "\n" + chr;
            }

            cmdinfo.Message.Delete();
            return response;
        }
    }

    public class Die : Command
    {
        public override void InitializeVariables()
        {
            Name = "D";
            Description = "Rolls a fair dice with results between 1 und the given number (inclusive)";
            PermsRequired = 0;
            Usage = new string[] { "<Die size>" };
        }

        internal override string InternalRunCommand(CommandInformation cmdinfo)
        {
            if (Int32.TryParse(cmdinfo.Arguments[0], out int diesize))
            {
                if (diesize > 1)
                {
                    Random rng = new Random();
                    return rng.Next(1, (diesize + 1)).ToString();
                }
            }

            return InvalidValue("Die size");
        }
    }
}
