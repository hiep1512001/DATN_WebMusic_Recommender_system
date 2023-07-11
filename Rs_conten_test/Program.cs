using System;
using System.Data;

namespace Rs_conten_test
{
    class Program
    {
        private static DataTable matric = new DataTable();
        private static string[] column = new string[5] { "big data", "R", "Py thon", "Machine Learning", "Learning Paths" };
        private static double[] rating = new double[6] { 1, -1, 0, 0, 0, 1 };
        private static double[] IDF = new double[5];
        private static double[] DF = new double[5];
        private static double[] userProfile = new double[5];
        static void CreateColumnMatric(string nameColumn)
        {
            DataColumn dtColumn = new DataColumn();
            dtColumn.DataType = typeof(double);
            dtColumn.ColumnName = nameColumn;
            matric.Columns.Add(dtColumn);
        }
        static void addRow()
        {
            DataRow row = matric.NewRow();
            row["big data"] = 1;
            row["R"] = 0;
            row["Py thon"] = 1;
            row["Machine Learning"] = 0;
            row["Learning Paths"] = 1;
            matric.Rows.Add(row);
        }
        static void Load(int countRow, int countcolumn)
        {
            for (int i = 0; i < countRow; i++)
            {
                for (int j = 0; j < countcolumn; j++)
                {
                    Console.Write(" | " + matric.Rows[i].ItemArray[j]);
                }
                Console.WriteLine();
            }
        }
        static void Chuanhoa(int indexRow, int conutColumn)
        {
            int Total = 0;
            for (int i = 0; i < conutColumn; i++)
            {
                if (Convert.ToInt32(matric.Rows[indexRow].ItemArray[i]) == 1)
                {
                    Total = Total + 1;
                }
            }           
            for (int i = 0; i < conutColumn; i++)
            {
                if (Convert.ToInt32(matric.Rows[indexRow].ItemArray[i]) == 1)
                {
                    double Gia_tri_da_chuan_hoa = Convert.ToDouble(1 / Math.Sqrt(Total));

                    matric.Rows[indexRow][column[i]] = Gia_tri_da_chuan_hoa;
                    matric.AcceptChanges();
                }
            }
        }
        static void AddValueUserProfile()
        {
            {
                for (int i = 0; i < userProfile.Length; i++)
                {
                       double giatri = 0;
                        int df = 0;
                        for (int j = 0; j < rating.Length; j++)
                        {
                            {
                                double giatri1 =Convert.ToDouble( rating[j]);
                                double giatri2 = Convert.ToDouble(matric.Rows[j].ItemArray.GetValue(i)); 
                                giatri = giatri + Convert.ToDouble(giatri1 * giatri2);
                                if (giatri2 != 0)
                                {
                                df = df + 1;
                                }
                            }                         
                        }
                        userProfile[i] = giatri;
                    DF[i] = df;
                }
            }
        }
        static void TinhIdf()
        {
            for(int i=0; i < IDF.Length; i++)
            {

                IDF[i] = Math.Log10(10 / DF[i]);
            }
            
        }
        static void TinhDoUaThich()
        {
            for(int i=0; i<rating.Length; i++)
            {
                double giatri = 0;
                for (int j=0; j< userProfile.Length; j++)
                {
                    
                    giatri = giatri +Convert.ToDouble(matric.Rows[i].ItemArray.GetValue(j)) * userProfile[j] * IDF[j];
                }
                rating[i] = giatri;
            }
        }
        static void Main(string[] args)
        {
            CreateColumnMatric("big data");
            CreateColumnMatric("R");
            CreateColumnMatric("Py thon");
            CreateColumnMatric("Machine Learning");
            CreateColumnMatric("Learning Paths");
            DataRow row1 = matric.NewRow();
            row1["big data"] = 1;
            row1["R"] = 0;
            row1["Py thon"] = 1;
            row1["Machine Learning"] = 0;
            row1["Learning Paths"] = 1;
            matric.Rows.Add(row1);

            DataRow row2 = matric.NewRow();
            row2["big data"] = 0;
            row2["R"] = 1;
            row2["Py thon"] = 1;
            row2["Machine Learning"] = 1;
            row2["Learning Paths"] = 0;
            matric.Rows.Add(row2);

            DataRow row3 = matric.NewRow();
            row3["big data"] = 0;
            row3["R"] = 0;
            row3["Py thon"] = 0;
            row3["Machine Learning"] = 1;
            row3["Learning Paths"] = 1;
            matric.Rows.Add(row3);

            DataRow row4 = matric.NewRow();
            row4["big data"] = 0;
            row4["R"] = 0;
            row4["Py thon"] = 1;
            row4["Machine Learning"] = 1;
            row4["Learning Paths"] = 0;
            matric.Rows.Add(row4);

            DataRow row5 = matric.NewRow();
            row5["big data"] = 0;
            row5["R"] = 1;
            row5["Py thon"] = 0;
            row5["Machine Learning"] = 0;
            row5["Learning Paths"] = 0;
            matric.Rows.Add(row5);

            DataRow row6 = matric.NewRow();
            row6["big data"] = 1;
            row6["R"] = 0;
            row6["Py thon"] = 0;
            row6["Machine Learning"] = 1;
            row6["Learning Paths"] = 0;
            matric.Rows.Add(row6);
            int countcolumn = matric.Columns.Count;
            int countRow = matric.Rows.Count;
            for (int i = 0; i < countcolumn; i++)
            {
                Console.Write(" | " + matric.Columns[i].ColumnName);
            }
            Console.WriteLine("\ntruoc chuan hoa");
            Load(countRow, countcolumn);
            //thuc hien chuan hoa
            for (int i = 0; i < countRow; i++)
            {
                Chuanhoa(i, countcolumn);
            }
            Console.WriteLine("sau chuan hoa");
            Load(countRow, countcolumn);
            //tinh vector
            double[] gia_tri_cosin = new double[] { 0, 0, 0, 0, 0, 0 };
            for (int i = 1; i < countRow; i++)
            {
                for (int j = 0; j < countcolumn; j++)
                {
                    double giatri1 = Convert.ToDouble(matric.Rows[0].ItemArray[j]);
                    double giatri2 = Convert.ToDouble(matric.Rows[i].ItemArray[j]);
                    gia_tri_cosin[i] = gia_tri_cosin[i] + Convert.ToDouble(giatri1 * giatri2);
                }
            }
            for (int i = 0; i < countRow; i++)
            {
                Console.WriteLine(gia_tri_cosin[i]);
            }

            Load(countRow,countcolumn);
            Console.WriteLine("User profile");
            AddValueUserProfile();
            for (int i = 0; i < userProfile.Length; i++)
            {
                Console.Write(" | "+userProfile[i] );
            }
            Console.WriteLine("\nDF");
            for (int i = 0; i < DF.Length; i++)
            {
                Console.Write(" | " + DF[i]);
            }
            TinhIdf();
            Console.WriteLine("\nIDF");
            for (int i = 0; i < IDF.Length; i++)
            {
                Console.Write(" | " + IDF[i]);
            }
            Console.WriteLine("\nTinh do ua thich");
            TinhDoUaThich();
            for (int i = 0; i < rating.Length; i++)
            {
                Console.Write(" | " + rating[i]);
            }
        }
    }
}
