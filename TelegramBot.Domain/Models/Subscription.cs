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
        public virtual User User { get; set; }

        public Subscription(int userId, SubscriptionItem subscriptionItem)
        {
            UserId = userId;
            SubscriptionItem = subscriptionItem;
        }

    }

     public enum SubscriptionItem:byte
    {
        Weather=1,
        TimeTable=2
    }
}
