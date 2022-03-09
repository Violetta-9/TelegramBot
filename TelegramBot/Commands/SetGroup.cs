using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using TelegramBot.Commands.Abstractions;
using TelegramBot.DataAccess;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Domain.Models;

namespace TelegramBot.Commands
{
    public class SetGroup : IBotCommand
    {
        public string[] Alias { get; set; }=new[] { "SETGROUP" };
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        public SetGroup(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            var group = msg.Text.Split(" ")[1].ToUpper();


            var chec = _db.Groups.Where(x => x.Title.Equals(group)).FirstOrDefault();
            if (chec is Group)
            {
                var user = new Domain.Models.User(msg.Chat.Id, chec);
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, "Группа успешно установленна");

            }



        }


    }
}
