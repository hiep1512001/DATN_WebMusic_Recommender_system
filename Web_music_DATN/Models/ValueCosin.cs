using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class ValueCosin: IComparable<ValueCosin>
    {
        public int id { get; set; }
        public double cosin { get; set; }

        public int CompareTo(ValueCosin other)
        {
            return this.cosin.CompareTo(other.cosin);
        }
    }
}