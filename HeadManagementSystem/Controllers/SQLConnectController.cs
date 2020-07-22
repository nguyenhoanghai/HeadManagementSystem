using GPRO.Core.Hai;
using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Xml;

namespace HeadManagementSystem.Controllers
{
    public class SQLConnectController : Controller
    {
        // GET: SQLConnect
        public ActionResult Index()
        {
            string info = BaseCore.Instance.GetStringConnectInfo(Server.MapPath("~/Config_XML") + "\\DATA.XML");
            if (!string.IsNullOrEmpty(info))
            {
                var infos = info.Split(',');
                var conn = new SqlConnection(getConnectString(infos[0], infos[2], infos[3], bool.Parse(infos[4])));
                conn.Open();
                var ds = new DataSet();
                string query = "select name from sysdatabases";
                var da = new SqlDataAdapter(query, conn);
                da.Fill(ds, "databasenames");
                var models = new List<ModelSelectItem>();
                if (ds.Tables["databasenames"] != null && ds.Tables["databasenames"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["databasenames"].Rows.Count; i++)
                    {
                        models.Add(new ModelSelectItem() { Code = ds.Tables["databasenames"].Rows[i]["name"].ToString(), Name = ds.Tables["databasenames"].Rows[i]["name"].ToString() });
                    }
                }
                ViewBag.databases = models;
                ViewBag.dataName = infos[1];
            }
            return View();
        }

        public JsonResult GetDatabases(string sname, string lg, string ps, bool authen)
        {
            var models = new List<ModelSelectItem>();
            if (!string.IsNullOrEmpty(sname))
            {
                SqlConnection conn;
                conn = new SqlConnection(getConnectString(sname, lg, ps, authen));
                conn.Open();
                var ds = new DataSet();
                string query = "select name from sysdatabases";
                var da = new SqlDataAdapter(query, conn);
                da.Fill(ds, "databasenames");
                if (ds.Tables["databasenames"] != null && ds.Tables["databasenames"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["databasenames"].Rows.Count; i++)
                    {
                        models.Add(new ModelSelectItem() { Code = ds.Tables["databasenames"].Rows[i]["name"].ToString(), Name = ds.Tables["databasenames"].Rows[i]["name"].ToString() });
                    }
                }
            }
            return Json(models);
        }

        [HttpPost]
        public JsonResult Save(  string sname, string lg, string ps, bool authen, string dataName)
        {
            try
            {
                if (!string.IsNullOrEmpty(sname))
                {
                    string filePath = Server.MapPath("~\\Config\\DATA.XML");
                    if (!System.IO.File.Exists(filePath))
                        System.IO.File.Create(filePath);

                    //Start writer
                    XmlTextWriter writer = new XmlTextWriter(filePath, System.Text.Encoding.UTF8);

                    //Start XM DOcument
                    writer.WriteStartDocument(true);
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;

                    //ROOT Element
                    writer.WriteStartElement("String_Connect");

                    //parent node start
                    writer.WriteStartElement("SQLServer");

                    writer.WriteStartElement("Server_Name");
                    writer.WriteString(EncryptionHelper.Instance.Encrypt(sname));
                    writer.WriteEndElement();

                    writer.WriteStartElement("Database");
                    writer.WriteString(EncryptionHelper.Instance.Encrypt(dataName));
                    writer.WriteEndElement();

                    writer.WriteStartElement("User");
                    writer.WriteString(EncryptionHelper.Instance.Encrypt(lg));
                    writer.WriteEndElement();

                    writer.WriteStartElement("Password");
                    writer.WriteString(EncryptionHelper.Instance.Encrypt(ps));
                    writer.WriteEndElement();

                    writer.WriteStartElement("Window_Authenticate");
                    writer.WriteString(authen.ToString());
                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    //End XML Document
                    writer.WriteEndDocument();

                    //Close writer
                    writer.Close();
                }
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json("Lưu thất bại.!");
            }
        }

        private string getConnectString(string serverName, string login, string pass, bool isAuthen)
        {
            if (!string.IsNullOrEmpty(serverName))
            {
                if (isAuthen)
                    return string.Concat(new string[] { "Server = ", serverName, ";Trusted_Connection=true;", });
                else
                    return string.Concat(new string[] { "Server = ", serverName, " ; Uid = ", login, " ;Pwd= ", pass });
            }
            return "";
        }

    }
}