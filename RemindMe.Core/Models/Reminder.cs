using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemindMe.Core.Models
{
    public class Reminder
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        public string Title { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
