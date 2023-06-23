using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
namespace Recommender_sys_test
{
    public class MovieRating
    {
        [LoadColumn(0)]
        public float ID_USER;
        [LoadColumn(1)]
        public float ID_SONG;
        [LoadColumn(2)]
        public float Label;
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
    public class MovieRatingPrediction
    {
        public float Label;
        public float Score;
    }
}
