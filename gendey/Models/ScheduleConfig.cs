using System;
using System.Collections.Generic;

namespace gendey.Models
{
    public partial class ScheduleConfig
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Duration { get; set; }
        public int? DayOfWeek { get; set; }

        public virtual User User { get; set; }
    }
}
