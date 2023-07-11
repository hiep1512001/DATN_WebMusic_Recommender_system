using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class GetValueRating
    {
        public float max { get; set; }
        public float value { get; set; }
        public int idSong { get; set; }
        public int idUser { get; set; }
    }
}