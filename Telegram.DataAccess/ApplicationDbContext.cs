using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Domain.Models;

namespace Telegram.DataAccess
{
     public class ApplicationDbContext:DbContext
    {
           public DbSet<TimeTable> TimeTables { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Subscription> Subscriptions { get; set; }
            public DbSet<TelegramBot.Domain.Models.Group> Groups { get; set; }
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> optionBuilder) : base(optionBuilder)
            {

            }
        
    }
}
