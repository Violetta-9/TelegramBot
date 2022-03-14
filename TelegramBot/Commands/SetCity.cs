using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Telegram.Application.Contracts;
using Telegram.Application.Queries;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.DataAccess;
using TelegramBot.Commands.Abstractions;


namespace TelegramBot.Commands
{
    public class SetCity : IBotCommand
    {
        public string[] Alias { get; set; } = new[] { "SETCITY" };
        private readonly ApplicationDbContext _db;
        private readonly TelegramBotClient _client;
        private readonly IMediator _mediator;

        public SetCity(ApplicationDbContext db, TelegramBotClient client, IMediator mediator)
        {
            _db = db;
            _client = client;
            _mediator = mediator;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            var city = msg.Text.Split(" ")[1].ToUpper();
            var checkCity = await _mediator.Send(new GetCity(city));// todo: проверка города на существования 
            
            if (checkCity != null)
            {
                var user = _db.Users.FirstOrDefault(x => x.IdChat == msg.Chat.Id);

                if (user != null)
                {
                    user.City = city;

                    await _db.SaveChangesAsync(cancellationToken);
                    await _client.SendTextMessageAsync(msg.Chat.Id, "Город установлен", cancellationToken: cancellationToken);
                }
            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, $"{city.Substring(0, 1).ToUpper() + city.Substring(1, city.Length - 1).ToLower()} не существует", cancellationToken: cancellationToken);
            }

        }
    }
}