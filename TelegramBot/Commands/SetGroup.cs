using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Commands.Abstractions;
using Telegram.DataAccess;

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


            var chec = _db.Groups.FirstOrDefault(x => x.Title.Equals(@group));
            if (chec != null)
            {
                var user = new Domain.Models.User(msg.Chat.Id, chec);
                await _db.Users.AddAsync(user, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, "Группа успешно установленна", cancellationToken: cancellationToken);

            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, $"группа {group} не найдена ", cancellationToken: cancellationToken);
            }



        }


    }
}
