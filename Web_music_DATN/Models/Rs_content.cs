using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web_music_DATN.Models
{
    public class Rs_content
    {
        private  DataTable matric = new DataTable();
        private  string[] NameColumn;
        private  List<CustomSong> lstCusSong;
        private  static List<SONG> lstSongNoRate= new List<SONG>();
        private  double[] rating;
        private  double[] userProfile;
        static int countColumn = 0;
        static int countRow = 0;
        //Hàm lấy dữ liệu từ db và thêm column
        public List<SONG> getSongRating(int id)
        {
            lstSongNoRate.Clear();
            //Gia su user id=2
            Web_musicEntities conn = new Web_musicEntities();
            List<SONG> lstSong = new List<SONG>();
            List<RATING> lstRating1 = conn.RATINGs.ToList();
            List<RATING> lstRating2 = new List<RATING>();
            foreach (RATING a in lstRating1)
            {
                if (a.ID_USER == id && a.RATING1 >= 3)
                {
                    lstRating2.Add(a);
                    SONG song = conn.SONGs.Find(a.ID_SONG);
                    lstSong.Add(song);
                }
            }
            lstSongNoRate = conn.SONGs.ToList();
            foreach(SONG a in lstSong)
            {
                lstSongNoRate.Remove(a);
            }
            rating = new double[lstRating2.Count];
            for (int i = 0; i < lstRating2.Count; i++)
            {
                rating[i] = Convert.ToDouble(lstRating2[i].RATING1);
            }
            return lstSong;
        }

        public void getData(List<SONG> lstSong)
        {
            matric.Clear();
            //lấy danh sách
            Web_musicEntities conn = new Web_musicEntities();
            List<GENRE> lstGenre = conn.GENRES.ToList();
            List<COMPOSER> lstComposer = conn.COMPOSERs.ToList();
            List<SINGER> lstSinger = conn.SINGERs.ToList();
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
                cus.tentheloai = "TL" + conn.GENRES.Find(a.ID_GENRE).NAME_GENRE;
                cus.idtacgia = Convert.ToInt32(a.ID_COMPOSER);
                cus.tentacgia = "TG" + conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER;
                lstCusSong.Add(cus);
            }
            List<string> nameComposer = new List<string>();
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
            }
            //Thực hiện tính số cột, số hàng, tên cột
            nameComposer = KtraTrung(nameComposer);
            nameGenre = KtraTrung(nameGenre);
            nameSinger = KtraTrung(nameSinger);
            countColumn = nameSinger.Count() + nameGenre.Count + nameComposer.Count() + 1;          
              //
            countRow = lstCusSong.Count();
            NameColumn = new string[countColumn];
            NameColumn[0] = "ID";
            int i = 1;
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
        }
        public void getData()
        {
            matric.Clear();
            //lấy danh sách
            Web_musicEntities conn = new Web_musicEntities();
            List<GENRE> lstGenre = conn.GENRES.ToList();
            List<COMPOSER> lstComposer = conn.COMPOSERs.ToList();
            List<SINGER> lstSinger = conn.SINGERs.ToList();
            //xử lý dữ liệu bai hát
            lstCusSong = new List<CustomSong>();
            foreach (SONG a in lstSongNoRate)
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
                cus.tentheloai = "TL" + conn.GENRES.Find(a.ID_GENRE).NAME_GENRE;
                cus.idtacgia = Convert.ToInt32(a.ID_COMPOSER);
                cus.tentacgia = "TG" + conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER;
                lstCusSong.Add(cus);
            }
            List<string> nameComposer = new List<string>();
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
            }
            //Thực hiện tính số cột, số hàng, tên cột
            nameComposer = KtraTrung(nameComposer);
            nameGenre = KtraTrung(nameGenre);
            nameSinger = KtraTrung(nameSinger);
            countColumn = nameSinger.Count() + nameGenre.Count + nameComposer.Count() + 1;
            countRow = lstCusSong.Count();
            NameColumn = new string[countColumn];
            NameColumn[0] = "ID";
            int i = 1;
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
        }
        public void getData(int id)
        {
            matric.Clear();
            //lấy danh sách
            Web_musicEntities conn = new Web_musicEntities();
            SONG xy = conn.SONGs.Find(id);
            List<SONG> lstSong = conn.SONGs.ToList();
            lstSong.Remove(xy);
            List<GENRE> lstGenre = conn.GENRES.ToList();
            List<COMPOSER> lstComposer = conn.COMPOSERs.ToList();
            List<SINGER> lstSinger = conn.SINGERs.ToList();
            //xử lý dữ liệu bai hát
            lstCusSong = new List<CustomSong>();
            CustomSong custom = new CustomSong();
            custom.id = xy.ID_SONG;
            custom.name = xy.NAME_SONG;
            custom.image = xy.PICTURE_SONG;
            custom.song = xy.PATH_SONG;
            if (xy.ID_ALBUM != null)
            {
                custom.idalbum = Convert.ToInt32(xy.ID_ALBUM);
                custom.tenalbum = conn.ALBUMs.Find(xy.ID_ALBUM).NAME_ALBUM;
            }
            else
            {
                custom.tenalbum = "none";
            }
            custom.idcasi = Convert.ToInt32(xy.ID_SINGER);
            custom.idtheloai = Convert.ToInt32(xy.ID_GENRE);
            custom.tencasi = "CS" + conn.SINGERs.Find(xy.ID_SINGER).NAME_SINGER;
            custom.tentheloai = "TL" + conn.GENRES.Find(xy.ID_GENRE).NAME_GENRE;
            custom.idtacgia = Convert.ToInt32(xy.ID_COMPOSER);
            custom.tentacgia = "TG" + conn.COMPOSERs.Find(xy.ID_COMPOSER).NAME_COMPOSER;
            lstCusSong.Add(custom);
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
                cus.tentheloai = "TL" + conn.GENRES.Find(a.ID_GENRE).NAME_GENRE;
                cus.idtacgia = Convert.ToInt32(a.ID_COMPOSER);
                cus.tentacgia = "TG" + conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER;
                lstCusSong.Add(cus);
            }
            List<string> nameComposer = new List<string>();
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
            }
            //Thực hiện tính số cột, số hàng, tên cột
            nameComposer = KtraTrung(nameComposer);
            nameGenre = KtraTrung(nameGenre);
            nameSinger = KtraTrung(nameSinger);
            countColumn = nameSinger.Count() + nameGenre.Count + nameComposer.Count() + 1;
            countRow = lstCusSong.Count();
            NameColumn = new string[countColumn];
            NameColumn[0] = "ID";
            int i = 1;
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
        }
        public List<string> KtraTrung(List<string> lst)
        {
            List<string> newlist = new List<string>();
            newlist = lst.Distinct().ToList();
            return newlist;
        }
        public void CreateColumnMatric(string nameColumn)
        {
            DataColumn dtColumn = new DataColumn();
            dtColumn.DataType = typeof(double);
            dtColumn.ColumnName = nameColumn;
            matric.Columns.Add(dtColumn);
        }
        public void addColumn()
        {
            userProfile = new double[countColumn];
            for (int i = 0; i < countColumn; i++)
            {
                CreateColumnMatric(NameColumn[i]);
            }
        }
        public void addRow()
        {
            foreach (CustomSong a in lstCusSong)
            {
                DataRow row = matric.NewRow();
                row["ID"] = a.id;
                for (int i = 1; i < countColumn; i++)
                {
                    if (matric.Columns[i].ColumnName.Equals(a.tencasi) || matric.Columns[i].ColumnName.Equals(a.tentacgia) || matric.Columns[i].ColumnName.Equals(a.tentheloai))
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
        public void Load()
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
        //Hàm chuẩn hóa thuộc tính
        public void Chuanhoa()
        {
            for (int j = 0; j < countRow; j++)
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
                        double Gia_tri_da_chuan_hoa = Convert.ToDouble(1 / Math.Sqrt(Total));

                        matric.Rows[j][NameColumn[i]] = Gia_tri_da_chuan_hoa;
                        matric.AcceptChanges();
                    }
                }
            }
        }
        public List<ValueCosin> Cosin()
        {
            List<ValueCosin> lst_gia_tri_cosin = new List<ValueCosin>();
            for (int i = 1; i < countRow; i++)
            {
                ValueCosin gia_tri_cosin = new ValueCosin();
                gia_tri_cosin.cosin = 0;
                for (int j = 1; j < countColumn; j++)
                {
                    double giatri1 = Convert.ToDouble(matric.Rows[0].ItemArray[j]);
                    double giatri2 = Convert.ToDouble(matric.Rows[i].ItemArray[j]);
                    gia_tri_cosin.cosin = gia_tri_cosin.cosin + Convert.ToDouble(giatri1 * giatri2);
                    gia_tri_cosin.id =Convert.ToInt32( matric.Rows[i].ItemArray[0]);                
                }
                lst_gia_tri_cosin.Add(gia_tri_cosin);              
            }
            lst_gia_tri_cosin.Sort();
            List<ValueCosin> lst_10_gia_tri_lon_nhat = new List<ValueCosin>();
            for (int i = lst_gia_tri_cosin.Count - 1; i >= 0; i--)
            {
                if (lst_10_gia_tri_lon_nhat.Count == 10)
                {
                    break;
                }
                lst_10_gia_tri_lon_nhat.Add(lst_gia_tri_cosin[i]);
            }         
            return lst_10_gia_tri_lon_nhat;
        }
        public void AddValueUserProfile()
        {
            {
                for (int i = 1; i < userProfile.Length; i++)
                {
                    double giatri = 0;
                    int df = 0;
                    for (int j = 0; j < rating.Length; j++)
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
        public List<ValueCosin> TinhDoUaThich()
        {
            List<ValueCosin> list_gia_tri = new List<ValueCosin>();
            for (int i = 0; i < countRow; i++)
            {
                ValueCosin value = new ValueCosin();
                double giatri = 0;
                for (int j = 1; j < userProfile.Length; j++)
                {
                    giatri = giatri + Convert.ToDouble(matric.Rows[i].ItemArray.GetValue(j)) * userProfile[j] ;
                }
                value.cosin = giatri;
                value.id =Convert.ToInt32( matric.Rows[i]["ID"]);
                list_gia_tri.Add(value);
            }
            list_gia_tri.Sort();
            List<ValueCosin> lst_6_gia_tri_lon_nhat = new List<ValueCosin>();
            for (int i = list_gia_tri.Count - 1; i >= 0; i--)
            {
                if (lst_6_gia_tri_lon_nhat.Count == 6)
                {
                    break;
                }
                lst_6_gia_tri_lon_nhat.Add(list_gia_tri[i]);
            }
            return lst_6_gia_tri_lon_nhat;
        }

    }
}