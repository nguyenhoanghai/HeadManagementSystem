using HMS.Data.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLNhapPT
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLNhapPT _Instance;  //volatile =>  tranh dung thread
        public static BLLNhapPT Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLNhapPT();

                return _Instance;
            }
        }
        private BLLNhapPT() { }
        #endregion

        public ResponseModel InsertOrUpdate(string connectString, H_NgheNghiep NhapPT)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (NhapPT.Id == 0)
                    db.H_NgheNghiep.Add(NhapPT);
                else
                {
                    var found = db.H_NgheNghiep.FirstOrDefault(x => !x.IsDeleted && x.Id == NhapPT.Id);
                    if (found != null)
                    {
                        found.Code = NhapPT.Code;
                        found.Note = NhapPT.Note;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.sms = "Nghề nghiệp này đã bị xóa hoặc không tồn tại trong hệ thống.!";
                    }
                }
                if (result.IsSuccess)
                    db.SaveChanges();
                return result;
            }
        }

        public int InsertFromExcel(Stream fileStream, string path, string connectionString)
        {
            try
            {
                using (var excel = new ExcelPackage(fileStream))
                {
                    var ws = excel.Workbook.Worksheets.First();
                    using (var db = new HMSEntities(connectionString))
                    {
                        var dsPTs = (from x in db.H_PhuTung where !x.IsDeleted select new { Id = x.Id, Code = x.Code, Name = x.Name, sl = x.Quantities }).ToList();

                        H_PhuTung phuTung;
                        H_NhapPT nhapPT;
                        string malo = "", code = "", name = "", soluong = "", giamua = "", giaban = "", query = "";
                        double _giaban = 0, _giamua = 0;
                        int _soluong = 0;
                        var now = DateTime.Now;
                        for (int ii = 2; ii <= ws.Dimension.End.Row; ii++)
                        {
                            _soluong = 0;
                            _giaban = 0;
                            _giamua = 0;
                            malo = ws.Cells[ii, 1].Text.ToString();
                            code = ws.Cells[ii, 2].Text.ToString();
                            name = ws.Cells[ii, 3].Text.ToString();
                            soluong = ws.Cells[ii, 4].Text.ToString();
                            giamua = ws.Cells[ii, 5].Text.ToString();
                            giaban = ws.Cells[ii, 6].Text.ToString();
                            int.TryParse(soluong, out _soluong);
                            double.TryParse(giamua, out _giamua);
                            double.TryParse(giaban, out _giaban);
                            var found = dsPTs.FirstOrDefault(x => x.Code.Trim().ToUpper() == code.Trim().ToUpper());
                            if (found == null)
                            {
                                found = dsPTs.FirstOrDefault(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper());
                                if (found == null)
                                {
                                    phuTung = new H_PhuTung() { Code = code, Name = name, Quantities = _soluong, Price_In = _giamua, Price_Out = _giaban };
                                    nhapPT = new H_NhapPT() { Code = code, Name = name, Price_In = _giamua, Price_Out = _giaban, MaLo = malo, H_PhuTung = phuTung, CreatedDate = now, Quantities = _soluong };
                                    phuTung.H_NhapPT = new List<H_NhapPT>();
                                    phuTung.H_NhapPT.Add(nhapPT);
                                    db.H_PhuTung.Add(phuTung);
                                }
                                else
                                {
                                    nhapPT = new H_NhapPT() { Code = code, Name = name, Price_In = _giamua, Price_Out = _giaban, MaLo = malo, PTId = found.Id, CreatedDate = now, Quantities = _soluong };
                                    db.H_NhapPT.Add(nhapPT);
                                    query += " UPDATE [dbo].[H_PhuTung] set [Quantities] =" + (_soluong + found.sl) + " where id=" + found.Id;
                                }
                            }
                            else
                            {
                                nhapPT = new H_NhapPT() { Code = code, Name = name, Price_In = _giamua, Price_Out = _giaban, MaLo = malo, PTId = found.Id, CreatedDate = now, Quantities = _soluong };
                                db.H_NhapPT.Add(nhapPT);
                                query += " UPDATE [dbo].[H_PhuTung] set [Quantities] =" + (_soluong + found.sl) + " where id=" + found.Id;
                            }
                        }
                        if (!string.IsNullOrEmpty(query))
                            db.Database.ExecuteSqlCommand(query);
                        db.SaveChanges();
                        return 1;
                    } 
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
