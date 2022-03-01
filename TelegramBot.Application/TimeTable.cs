using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.DataAccess
{
   public  class TimeTable
    {
        public int Id { get; set; }
        public string LessonsOfTheDay { get; set; }
        public Week Week { get; set; }
        public EvenWeek EvenWeek { get; set; }
       
    }

   public enum Week:byte
   {
       Monday=1,
       Tuesday=2,
       WednesDay=3,
       Thursday=4,
       Friday=5
       

   }

   public enum EvenWeek:byte
   {
       Even=1,
       NotEven=2
   }
}
