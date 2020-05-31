using System;
using System.Collections.Generic;

namespace gendey.Models
{
    public partial class ConfClient
    {
        public int Id { get; set; }
        public DateTime? RegisterDate { get; set; }

        public virtual User User { get; set; }
    }
}
