using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class CustomSinger
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}