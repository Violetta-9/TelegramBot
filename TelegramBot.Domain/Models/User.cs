using System;
using System.Collections.Generic;
using System.Text;


namespace TelegramBot.Domain.Models
{
   public  class User
    {
        public int Id { get; set; }
        public long IdChat { get; set; }
        public string NameGroup { get; set; }
        public IEnumerable<TimeTable> TimeTables { get; set; }

        public User(long idChat,string nameGroup)
        {
            IdChat = idChat;
            NameGroup = nameGroup;
        }

        public User(long id)
        {

        }
        
    }
}
