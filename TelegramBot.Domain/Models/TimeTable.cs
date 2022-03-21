using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace TelegramBot.Domain.Models
{
   public  class TimeTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LessonsOfTheDay { get; set; }
        public DayOfWeek Week { get; set; }
        public EvenWeek EvenWeek { get; set; }
        public  Group Group { get; set; }


        public TimeTable(string lessonsOfTheDay, DayOfWeek week, EvenWeek evenWeek,Group group)
        {
            LessonsOfTheDay = lessonsOfTheDay;
            Week = week;
            EvenWeek = evenWeek;
            Group = group;
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
