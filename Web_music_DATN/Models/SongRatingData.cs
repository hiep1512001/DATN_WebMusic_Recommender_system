using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class SongRating:IComparable<SongRating>
    {
        [LoadColumn(0)]
        public float ID_USER;
        [LoadColumn(1)]
        public float ID_SONG;
        [LoadColumn(2)]
        public float Label;

        public int CompareTo(SongRating other)
        {
            return this.Label.CompareTo(other.Label);
        }
    }
    public class ModelInput
    {
        [LoadColumn(0)]
        [ColumnName(@"ID_USER")]
        public float ID_USER { get; set; }

        [LoadColumn(1)]
        [ColumnName(@"ID_SONG")]
        public float ID_SONG { get; set; }

        [LoadColumn(2)]
        [ColumnName(@"RATING")]
        public float RATING { get; set; }

    }
    public class SongRatingPrediction
    {
        public float Label;
        public float Score;
    }
}