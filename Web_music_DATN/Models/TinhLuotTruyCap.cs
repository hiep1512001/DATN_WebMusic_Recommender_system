using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class TinhLuotTruyCap: IComparable<TinhLuotTruyCap>
    {
        public int id { get; set; }
        public int dem { get; set; }

        public int CompareTo(TinhLuotTruyCap other)
        {
            return this.dem.CompareTo(other.dem);
        }
    }
}