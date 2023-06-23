using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class CustomSong
    {
        public int id { get; set; }
        public string name { get; set; }
        public int idcasi { get; set; }
        public int idtheloai { get; set; }
        public int idalbum { get; set; }
        public string image { get; set; }
        public string song { get; set; }
        public string tencasi { get; set; }
        public string tenalbum { get; set; }
        public string tentheloai { get; set; }
        public int idtacgia { get; set; }
        public string tentacgia { get; set; }
        public int views { get; set; }
        public int luottruycap { get; set; }
        public List<CustomPlaylist> listplaylist { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
        public HttpPostedFileWrapper SongFile { get; set; }
    }
}