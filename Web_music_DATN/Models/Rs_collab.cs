using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class Rs_collab
    {
        private static List<SongRating> lstkq = new List<SongRating>();
        public const string connectionString = @"Data Source=LAPTOP-K9OTI2P6\SQLEXPRESS;Initial Catalog=Web_music;User ID=sa;Integrated Security=True;Connect Timeout=30";
        /*        public const string connectionString = @"Data Source=LAPTOP-K9OTI2P6\SQLEXPRESS;Initial Catalog=test;User ID=sa";*/
        public const string sqlCommand1 = @"SELECT CAST([ID_USER] as REAL), CAST([ID_SONG] as REAL), CAST([rating] as REAL) FROM [dbo].[RATING]";
/*        public const string sqlCommand2 = @"SELECT CAST([userId] as REAL), CAST([movieId] as REAL), CAST([rating] as REAL) FROM [dbo].[rstest]";*/
        public (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<ModelInput>();

            DatabaseSource dbSource1 = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand1);
            DatabaseSource dbSource2 = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand1);
            IDataView trainingDataView = loader.Load(dbSource1);
            IDataView testDataView = loader.Load(dbSource2);

            return (trainingDataView, testDataView);

        }
        public ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "ID_USER")
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "songIdEncoded", inputColumnName: "ID_SONG"));
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "songIdEncoded",
                LabelColumnName = "RATING",
                NumberOfIterations = 10,
                ApproximationRank = 10
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
            Console.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }
        public void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            Console.WriteLine("=============== Evaluating the model ===============");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "RATING", scoreColumnName: "Score");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
        public void UseModelForSinglePrediction(MLContext mlContext, ITransformer model, List<SongRating> lst)
        {         
            Console.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<SongRating, SongRatingPrediction>(model);
            foreach(SongRating testInput in lst)
            {
                var songRatingPrediction = predictionEngine.Predict(testInput);
                if (songRatingPrediction !=null && Math.Round(songRatingPrediction.Score, 1) > 3)
                {
                    SongRating a = new SongRating();
                    a.ID_SONG = testInput.ID_SONG;
                    a.ID_USER = testInput.ID_USER;
                    a.Label = songRatingPrediction.Score;
                    lstkq.Add(a);
                }
            }
        }
        public void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "MovieRecommenderModel.zip");
            Console.WriteLine("=============== Saving the model to a file ===============");
            mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
        }
        public List<SongRating>  UseCollap( List<SongRating> lstInput)
        {
            lstkq.Clear();
            MLContext mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);
            /*EvaluateModel(mlContext, testDataView, model);*/
            UseModelForSinglePrediction(mlContext, model,lstInput);
            return lstkq;
        }
    }
}