using SQLite;

namespace RemindMe.Core.Models
{
    public class Reminder
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }

        public long Date { get; set; }
        public int AlreadyNotified { get; set; }
    }
}
