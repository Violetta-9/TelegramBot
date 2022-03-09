using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Domain.Models
{
   public  class Group
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual IEnumerable<User> Users { get; set; }
        public virtual IEnumerable<TimeTable> TimeTables { get; set; }
    }
}
