using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Recommender_sys_test;

namespace Recommender_sys_test
{
    class Program
    {
        public const string connectionString = @"Data Source=LAPTOP-K9OTI2P6\SQLEXPRESS;Initial Catalog=Web_music;User ID=sa;Integrated Security=True;Connect Timeout=30";
        public const string sqlCommand1 = @"SELECT CAST([ID_USER] as REAL), CAST([ID_SONG] as REAL), CAST([rating] as REAL) FROM [dbo].[RATING]";
        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);
            EvaluateModel(mlContext, testDataView, model);
            UseModelForSinglePrediction(mlContext, model);
            SaveModel(mlContext, trainingDataView.Schema, model);
        }
        static (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<ModelInput>();  
            DatabaseSource dbSource1 = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand1);         
            DatabaseSource dbSource2 = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand1);
            var data = loader.Load(dbSource1);
            DataOperationsCatalog.TrainTestData dataSlipt = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
            IDataView trainingDataView = dataSlipt.TrainSet;
            IDataView testDataView = dataSlipt.TestSet;

            return (trainingDataView, testDataView);

        }
        static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "ID_USER")
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "ID_SONG"));
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "RATING",
                NumberOfIterations = 10,
                ApproximationRank = 10
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
            Console.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }
        static void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            Console.WriteLine("=============== Evaluating the model ===============");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "RATING", scoreColumnName: "Score");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
        static void UseModelForSinglePrediction(MLContext mlContext, ITransformer model)
        {
            Console.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
            var testInput = new MovieRating { ID_USER = 6, ID_SONG = 146 };

            var movieRatingPrediction = predictionEngine.Predict(testInput);
            if (Math.Round(movieRatingPrediction.Score, 1) > 3.5)
            {
                Console.WriteLine("Movie " + testInput.ID_SONG + " is recommended for user " + testInput.ID_USER);
            }
            else
            {
                Console.WriteLine("Movie " + testInput.ID_SONG + " is not recommended for user " + testInput.ID_USER);
            }
        }
        static void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "MovieRecommenderModel.zip");
            Console.WriteLine("=============== Saving the model to a file ===============");
            mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
        }
    }
}
