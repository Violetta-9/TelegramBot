using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.DataAccess;
using TelegramBot.Domain.Models;

namespace TelegramBot.ScheduledTask
{
    public class ScheduledTask
    {
        private readonly ApplicationDbContext _db;
        private readonly TelegramBotClient _client;

        public ScheduledTask(ApplicationDbContext db,TelegramBotClient client)
        {
            _db = db;
            _client = client;
        }

        public async Task SendTimeTable()
        {

            var subscriptions = _db.Subscriptions.Include(x=>x.User).ThenInclude(x=>x.Group).Where(x=>x.SubscriptionItem==SubscriptionItem.TimeTable).ToArray();
            var i = subscriptions.Where(x => x.User.Group.Id == 1).ToArray();
            await _client.SendTextMessageAsync(subscriptions[0].User.IdChat, i[0].SubscriptionItem.ToString());

            EvenWeek even;
            DateTime myDateTime = DateTime.Now;
            int firstDayOfYear = (int)new DateTime(myDateTime.Year, 1, 1).DayOfWeek;
            int weekNumber = (myDateTime.DayOfYear + firstDayOfYear) / 7 + 1;
            if (weekNumber % 2 == 0)
            {
                even = EvenWeek.Even;
            }
            else
            {
                even = EvenWeek.Even;
            }
            var dayOfWeekNow = myDateTime.DayOfWeek;

            
            //foreach (var item in subscriptions)
            //{
            //   var timeTableTry= _db.TimeTables.Where(x => x.Group.Title == item.Key).Where(x => x.Week == dayOfWeekNow).Where(x => x.EvenWeek == even).ToArray();

            //   var id = item.Select(x => x.UserId);

            //   foreach (var userId in id)
            //   {
            //       await _client.SendTextMessageAsync(userId, timeTableTry[0].LessonsOfTheDay);

            //   }

               

            //}
            
        }
        public void InitJobs()
        {
            RecurringJob.AddOrUpdate<ScheduledTask>("SendWeather", x => x.SendTimeTable(),
                "50 20 * * *", TimeZoneInfo.Local);

            //  RecurringJob.AddOrUpdate<ScheduledTask>("SendSchedule", x => x.SendSchedule(),
            //                                    "10 6 * * *", TimeZoneInfo.Local);
        }

    }
}
