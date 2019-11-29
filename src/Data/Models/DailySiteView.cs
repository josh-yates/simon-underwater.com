using System;

namespace Data.Models
{
    public class DailySiteView
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public int Count { get; set; }
    }
}