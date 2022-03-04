using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Application;
using TelegramBot.Domain.Models;

namespace TelegramBot.DataAccess
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> optionBuilder):base(optionBuilder)
        {

        }
    }
}
