using System;
using System.Collections.Generic;

namespace gendey.Models
{
    public partial class Adress
    {
        public int Id { get; set; }
        public int FkCity { get; set; }
        public string Nbhood { get; set; }
        public string Street { get; set; }
        public int? Num { get; set; }
        public string Complement { get; set; }
        public int? Postalcode { get; set; }
        public DateTime RegisterDate { get; set; }

        public virtual City FkCityNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
