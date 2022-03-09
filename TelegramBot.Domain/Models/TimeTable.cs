using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TelegramBot.Domain.Models
{
   public  class TimeTable
    {
        public int Id { get; set; }
        public string LessonsOfTheDay { get; set; }
        public DayOfWeek Week { get; set; }
        public EvenWeek EvenWeek { get; set; }
        public virtual Group Group { get; set; }


        public TimeTable(string lessonsOfTheDay, DayOfWeek week, EvenWeek evenWeek)
        {
            LessonsOfTheDay = lessonsOfTheDay;
            Week = week;
            EvenWeek = evenWeek;
        }

        public TimeTable()
        {

        }
       
    }
   public enum EvenWeek:byte
   {
       Even=1,
       NotEven=2
   }
}
