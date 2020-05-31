using System;
using System.Collections.Generic;

namespace gendey.Models
{
    public partial class City
    {
        public City()
        {
            Adress = new HashSet<Adress>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? IbgeCode { get; set; }
        public string Uf { get; set; }

        public virtual ICollection<Adress> Adress { get; set; }
    }
}
