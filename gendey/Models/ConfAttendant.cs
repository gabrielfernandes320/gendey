using System;
using System.Collections.Generic;

namespace gendey.Models
{
    public partial class ConfAttendant
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public DateTime RegisterDate { get; set; }

        public virtual User User { get; set; }
    }
}
