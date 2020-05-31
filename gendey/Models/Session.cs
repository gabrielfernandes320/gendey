using System;
using System.Collections.Generic;

namespace gendey.Models
{
    public partial class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime AuthDate { get; set; }
        public string LastToken { get; set; }
        public DateTime TokenRefreshDate { get; set; }

        public virtual User User { get; set; }
    }
}
