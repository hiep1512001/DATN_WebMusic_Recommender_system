using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class CustomPlaylist
    {
        public int id { get; set; }
        public string name { get; set; }
        public int id_user { get; set; }
        public string name_user { get; set; }
        public int sumsong { get; set; }
    }
}