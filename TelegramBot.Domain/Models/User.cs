using System;
using System.Collections.Generic;
using System.Text;


namespace TelegramBot.Domain.Models
{
   public  class User
    {
        public int Id { get; set; }
        public long IdChat { get; set; }
        public string City { get; set; }
        public Group Group { get; set; }
       

        public User(long idChat, Group group, string city=default)
        {
            IdChat = idChat;
            Group = group;
            City = city;;
        }

        public User()
        {

        }
        
    }
}
