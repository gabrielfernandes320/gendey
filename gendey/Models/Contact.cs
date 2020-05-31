using System;
using System.Collections.Generic;

namespace gendey.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Fone { get; set; }
        public string Fone2 { get; set; }
        public string NameAlt { get; set; }
        public string FoneAlt { get; set; }
        public DateTime RegisterDate { get; set; }

        public virtual User User { get; set; }
    }
}
