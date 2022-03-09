using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Domain.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public SubscriptionItem SubscriptionItem { get; set; }

    }

     public enum SubscriptionItem:byte
    {
        Weather=1,
        TimeTable=2
    }
}
