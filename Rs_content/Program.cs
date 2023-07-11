using Microsoft.ML;
using Microsoft.ML.Trainers;
using Rs_content.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rs_content
{
    class Program
    {
        //ma trận
        private static DataTable matric = new DataTable();
        //tên cột
        private static string[] NameColumn;
        //danh sách dữ liệu
        private static List<CustomSong> lstCusSong;
        private static List<CustomSong> lst;
        //số cột
        static int countColumn = 0;
        //số hàng
        static int countRow = 0;
        //mảng chứa giá trị đánh giá của người dùng
        private static double[] rating ;
        // mảnh chứa giá trị là vector người dùng
        private static double[] userProfile;
        static List<SONG> getSong()
        {

            Web_musicEntities conn = new Web_musicEntities();
            List<SONG> lstSong = new List<SONG>();
/*            SONG song1 = conn.SONGs.Find(143);
            lstSong.Add(song1);
            SONG song2 = conn.SONGs.Find(138);
            lstSong.Add(song2);
            SONG song3 = conn.SONGs.Find(144);
            lstSong.Add(song3);*/
            SONG song4 = conn.SONGs.Find(108);
            lstSong.Add(song4);
            SONG song5 = conn.SONGs.Find(146);
            lstSong.Add(song5);
            return lstSong;
        }
        //Lấy danh sách bài hát được người dùng đánh giá
        static List<SONG> getSongRating(int id )
        {
            //Gia su user id=2
            Web_musicEntities conn = new Web_musicEntities();
            List<SONG> lstSong = new List<SONG>();
            List<RATING> lstRating1 = conn.RATINGs.ToList();
            List<RATING> lstRating2 = new List<RATING>();
            foreach (RATING a in lstRating1)
            {
                if (a.ID_USER == id)
                {
                    lstRating2.Add(a);
                    SONG song = conn.SONGs.Find(a.ID_SONG);
                    lstSong.Add(song);
                }
            }
            rating = new double[lstRating2.Count];
            for(int i=0; i<lstRating2.Count; i++)
            {
                rating[i] =Convert.ToDouble( lstRating2[i].RATING1);
            }
            return lstSong;
        }

        //Hàm lấy dữ liệu từ database và thêm column
        static void getData(List<SONG> lstSong)
        {
            matric.Clear();
            //lấy danh sách
            Web_musicEntities conn = new Web_musicEntities();
            List<GENRE> lstGenre = conn.GENRES.ToList();
            List<COMPOSER> lstComposer = conn.COMPOSERs.ToList();
            List<SINGER> lstSinger = conn.SINGERs.ToList();
            List<string> nameComposer = new List<string>();

            List<string> nameGenre = new List<string>();

            List<string> nameSinger = new List<string>();

            //xử lý dữ liệu bai hát
            lstCusSong = new List<CustomSong>();
            foreach(SONG a in lstSong)
            {
                CustomSong cus = new CustomSong();
                cus.id = a.ID_SONG;
                cus.name = a.NAME_SONG;
                cus.image = a.PICTURE_SONG;
                cus.song = a.PATH_SONG;
                if (a.ID_ALBUM != null)
                {
                    cus.idalbum = Convert.ToInt32(a.ID_ALBUM);
                    cus.tenalbum = conn.ALBUMs.Find(a.ID_ALBUM).NAME_ALBUM;
                }
                else
                {
                    cus.tenalbum = "none";
                }
                cus.idcasi = Convert.ToInt32(a.ID_SINGER);
                cus.idtheloai = Convert.ToInt32(a.ID_GENRE);
                cus.tencasi ="CS"+conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER;
                nameSinger.Add(conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER);
                cus.tentheloai ="TL"+ conn.GENRES.Find(a.ID_GENRE).NAME_GENRE;
                nameGenre.Add(conn.GENRES.Find(a.ID_GENRE).NAME_GENRE);
                cus.idtacgia = Convert.ToInt32(a.ID_COMPOSER);
                cus.tentacgia ="TG"+ conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER;
                nameComposer.Add(conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER);
                lstCusSong.Add(cus);
            }
/*            List<string> nameComposer = new List<string>();
            foreach (COMPOSER a in lstComposer)
            {
                string name = a.NAME_COMPOSER;
                nameComposer.Add(name);
            }
            List<string> nameGenre = new List<string>();
            foreach (GENRE a in lstGenre)
            {
                string name = a.NAME_GENRE;
                nameGenre.Add(name);
            }
            List<string> nameSinger = new List<string>();
            foreach (SINGER a in lstSinger)
            {
                string name = a.NAME_SINGER;
                nameSinger.Add(name);
            }*/
            //Thực hiện tính số cột, số hàng, tên cột
            nameComposer = KtraTrung(nameComposer);
            nameGenre = KtraTrung(nameGenre);
            nameSinger = KtraTrung(nameSinger);
            countColumn = nameSinger.Count() + nameGenre.Count + nameComposer.Count() + 3;
            userProfile = new double[countColumn];
            countRow = lstCusSong.Count();
            NameColumn = new string[countColumn];
            NameColumn[0] = "ID";
            int i = 1;
            NameColumn[i] = "TLNhạc trữ tình";
            i=i + 1;
            foreach (string a in nameGenre)
            {
                NameColumn[i] = "TL" + a;
                i = i + 1;
            }
            foreach (string a in nameSinger)
            {
                NameColumn[i] = "CS" + a;
                i = i + 1;
            }
            foreach (string a in nameComposer)
            {
                NameColumn[i] = "TG" + a;
                i = i + 1;
            }
            NameColumn[i] = "TGMạnh Phát";
        }
        static void getData()
        {
            matric.Clear();
            //lấy danh sách
            Web_musicEntities conn = new Web_musicEntities();
            List<GENRE> lstGenre = conn.GENRES.ToList();
            List<COMPOSER> lstComposer = conn.COMPOSERs.ToList();
            List<SINGER> lstSinger = conn.SINGERs.ToList();
            List<string> nameComposer = new List<string>();

            List<string> nameGenre = new List<string>();

            List<string> nameSinger = new List<string>();
            List<SONG> lstSong = new List<SONG>();
            /*            SONG song1 = conn.SONGs.Find(143);
                        lstSong.Add(song1);
                        SONG song2 = conn.SONGs.Find(138);
                        lstSong.Add(song2);
                        SONG song3 = conn.SONGs.Find(144);
                        lstSong.Add(song3);*/
            SONG song4 = conn.SONGs.Find(108);
            lstSong.Add(song4);
            SONG song5 = conn.SONGs.Find(146);
            lstSong.Add(song5);
            //xử lý dữ liệu bai hát
            lstCusSong = new List<CustomSong>();
            foreach (SONG a in lstSong)
            {
                CustomSong cus = new CustomSong();
                cus.id = a.ID_SONG;
                cus.name = a.NAME_SONG;
                cus.image = a.PICTURE_SONG;
                cus.song = a.PATH_SONG;
                if (a.ID_ALBUM != null)
                {
                    cus.idalbum = Convert.ToInt32(a.ID_ALBUM);
                    cus.tenalbum = conn.ALBUMs.Find(a.ID_ALBUM).NAME_ALBUM;
                }
                else
                {
                    cus.tenalbum = "none";
                }
                cus.idcasi = Convert.ToInt32(a.ID_SINGER);
                cus.idtheloai = Convert.ToInt32(a.ID_GENRE);
                cus.tencasi = "CS" + conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER;
                nameSinger.Add(conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER);
                cus.tentheloai = "TL" + conn.GENRES.Find(a.ID_GENRE).NAME_GENRE;
                nameGenre.Add(conn.GENRES.Find(a.ID_GENRE).NAME_GENRE);
                cus.idtacgia = Convert.ToInt32(a.ID_COMPOSER);
                cus.tentacgia = "TG" + conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER;
                nameComposer.Add(conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER);
                lstCusSong.Add(cus);
            }
            /*            List<string> nameComposer = new List<string>();
                        foreach (COMPOSER a in lstComposer)
                        {
                            string name = a.NAME_COMPOSER;
                            nameComposer.Add(name);
                        }
                        List<string> nameGenre = new List<string>();
                        foreach (GENRE a in lstGenre)
                        {
                            string name = a.NAME_GENRE;
                            nameGenre.Add(name);
                        }
                        List<string> nameSinger = new List<string>();
                        foreach (SINGER a in lstSinger)
                        {
                            string name = a.NAME_SINGER;
                            nameSinger.Add(name);
                        }*/
            //Thực hiện tính số cột, số hàng, tên cột
/*            nameComposer = KtraTrung(nameComposer);
            nameGenre = KtraTrung(nameGenre);
            nameSinger = KtraTrung(nameSinger);
            countColumn = nameSinger.Count() + nameGenre.Count + nameComposer.Count() + 3;
            userProfile = new double[countColumn];*/
            countRow = lstCusSong.Count();
/*            NameColumn = new string[countColumn];
            NameColumn[0] = "ID";
            int i = 1;
            NameColumn[i] = "TLNhạc trữ tình";
            i = i + 1;
            foreach (string a in nameGenre)
            {
                NameColumn[i] = "TL" + a;
                i = i + 1;
            }
            foreach (string a in nameSinger)
            {
                NameColumn[i] = "CS" + a;
                i = i + 1;
            }
            foreach (string a in nameComposer)
            {
                NameColumn[i] = "TG" + a;
                i = i + 1;
            }
            NameColumn[i] = "TGMinh Phát";*/
        }
        // hàm kiểm tra trùng của cột matric
        static List<String> KtraTrung(List<String> lst)
        {
            List<String> newlist = new List<string>();
            newlist = lst.Distinct().ToList();
            return newlist;
        }
        // Hàm tạo cột cho ma trận
        static void CreateColumnMatric(string nameColumn)
        {
            DataColumn dtColumn = new DataColumn();
            dtColumn.DataType = typeof(double);
            dtColumn.ColumnName = nameColumn;
            matric.Columns.Add(dtColumn);
        }
        // hàm gán cột cho ma trận
        static void addColumn()
        {
            for(int i=0; i<countColumn; i++)
            {
                CreateColumnMatric(NameColumn[i]);
            }
            
        }
        // Hàm thêm dòng cho ma trận
        static void addRow()
        {
           
            foreach(CustomSong a in lstCusSong)
            {
                DataRow row = matric.NewRow();
                row["ID"] = a.id;
                for(int i=1; i<countColumn; i++)
                {
                    if(matric.Columns[i].ColumnName.Equals(a.tencasi) || matric.Columns[i].ColumnName.Equals(a.tentacgia) || matric.Columns[i].ColumnName.Equals(a.tentheloai))
                    {
                        row[NameColumn[i]] = 1;
                    }
                    else
                    {
                        row[NameColumn[i]] = 0;
                    }
                }
                matric.Rows.Add(row);
            }
        }
        static void Load()
        {
            Console.WriteLine();
            for (int i = 0; i < countRow; i++)
            {
                for (int j = 0; j < countColumn; j++)
                {
                    Console.Write(" | " + matric.Rows[i].ItemArray[j]);
                }
                Console.WriteLine();
            }
        }
        //Hàm chuẩn hóa các giá trị thuộc tính
        static void Chuanhoa()
        {
            for(int j=0; j<countRow; j++)
            {
                int Total = 0;
                for (int i = 1; i < countColumn; i++)
                {
                    if (Convert.ToInt32(matric.Rows[j].ItemArray[i]) == 1)
                    {
                        Total = Total + 1;
                    }
                }
                for (int i = 1; i < countColumn; i++)
                {
                    if (Convert.ToInt32(matric.Rows[j].ItemArray[i]) == 1)
                    {
                        double Gia_tri_da_chuan_hoa = Convert.ToDouble(1 / Math.Sqrt(3));

                        matric.Rows[j][NameColumn[i]] = Gia_tri_da_chuan_hoa;
                        matric.AcceptChanges();
                    }
                }
            }
        }
        // Hàm tính giá trị cosin
        static void Cosin()
        {
            double[] gia_tri_cosin = new double[countRow];
            for (int i = 1; i < countRow; i++)
            {
                for (int j = 1; j < countColumn; j++)
                {
                    double giatri1 = Convert.ToDouble(matric.Rows[0].ItemArray[j]);
                    double giatri2 = Convert.ToDouble(matric.Rows[i].ItemArray[j]);
                    gia_tri_cosin[i] = gia_tri_cosin[i] + Convert.ToDouble(giatri1 * giatri2);
                    
                }
            }
            for (int i = 1; i < countRow; i++)
            {
                Console.WriteLine("do tuong dong bai 1 voi bai "+ (i+1)+ " la: "+gia_tri_cosin[i]);
            }
        }
        // Hàm tính vector người dùng
        static void AddValueUserProfile()
        {
            {
                for (int i = 1; i < userProfile.Length; i++)
                {
                    double giatri = 0;
                    int df = 0;
                    for (int j = 0; j < rating.Length ; j++)
                    {
                        {
                            double giatri1 = Convert.ToDouble(rating[j]);
                            double giatri2 = Convert.ToDouble(matric.Rows[j].ItemArray.GetValue(i));
                            giatri = giatri + Convert.ToDouble(giatri1 * giatri2);
                            if (giatri2 != 0)
                            {
                                df = df + 1;
                            }
                        }
                    }
                    userProfile[i] = giatri;
                }
            }
        }
        // Tính độ ưa thích của người dùng với bài hát
        static void TinhDoUaThich()
        {
            for (int i = 0; i < countRow; i++)
            {
                double giatri = 0;
                for (int j = 1; j < userProfile.Length; j++)
                {
                    giatri = giatri + (Convert.ToDouble(matric.Rows[i].ItemArray.GetValue(j)) * userProfile[j]);
                }
                rating[i] = giatri;
            }
        }






        // rs collap

/*        static (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {

            var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-test.csv");

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
            IDataView testDataView = mlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');

            return (trainingDataView, testDataView);

        }
        static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "movieId"));
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
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
            var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
        static void UseModelForSinglePrediction(MLContext mlContext, ITransformer model)
        {
            Console.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
            var testInput = new MovieRating { userId = 6, movieId = 10 };

            var movieRatingPrediction = predictionEngine.Predict(testInput);
            if (Math.Round(movieRatingPrediction.Score, 1) > 3.5)
            {
                Console.WriteLine("Movie " + testInput.movieId + " is recommended for user " + testInput.userId);
            }
            else
            {
                Console.WriteLine("Movie " + testInput.movieId + " is not recommended for user " + testInput.userId);
            }
        }
        static void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "MovieRecommenderModel.zip");
            Console.WriteLine("=============== Saving the model to a file ===============");
            mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
        }
*/



        static void Main(string[] args)
        {
            //rs content
            getData(getSongRating(66));
            addColumn();
            addRow();
            Console.WriteLine("truoc chuan hoa");
            for (int i = 0; i < countColumn; i++)
            {
                Console.Write(" | " + matric.Columns[i].ColumnName);
            }
            Load();
            Console.WriteLine("sau chuan hoa");
            Chuanhoa();
            Load();
            Console.WriteLine("User profile");
            AddValueUserProfile();
            for (int i = 0; i < userProfile.Length; i++)
            {
                Console.Write(" | " + userProfile[i]);
            }
            /*getData();
            addRow();
            Console.WriteLine("\ntruoc chuan hoa");
            for (int i = 0; i < countColumn; i++)
            {
                Console.Write(" | " + matric.Columns[i].ColumnName);

            }
            Load();
            Console.WriteLine("sau chuan hoa");
            Chuanhoa();
            Load();
            Console.WriteLine("Tinh do ua thich");
            TinhDoUaThich();
            for (int i = 0; i < countRow; i++)
            {
                Console.WriteLine(" Gia tri yeu thich bai i " + (i+4) + " la: " + rating[i]);
            }*/
            /*            Console.WriteLine("Tính cosin");*/
            /*            Cosin();*/
            Console.ReadLine();
            //rs collap

/*            MLContext mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);
            EvaluateModel(mlContext, testDataView, model);
            UseModelForSinglePrediction(mlContext, model);
            Console.ReadLine();*/
            /*            SaveModel(mlContext, trainingDataView.Schema, model);*/

        }
    }
}
