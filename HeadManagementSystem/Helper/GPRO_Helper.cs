﻿using GPRO.Core.Hai;
using System;
using System.Data.Entity.Core.EntityClient; 
using System.IO; 
using System.Web;
using System.Xml;

namespace HeadManagementSystem.Helper
{
    public class GPRO_Helper
    {
        #region constructor
        static object key = new object();
        private static volatile GPRO_Helper _Instance;  //volatile =>  tranh dung thread
        public static GPRO_Helper Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new GPRO_Helper();

                return _Instance;
            }
        }
        private GPRO_Helper() { }
        #endregion

        public string GetEntityConnectString()
        {
            try
            { 
                string filename =HttpContext.Current.Server.MapPath("~/Config\\DATA.XML");
                if (File.Exists(filename))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filename);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SQLServer");
                    string serverName = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[0].InnerText);
                    string databaseName = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[1].InnerText);
                    string login = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[2].InnerText);
                    string password = EncryptionHelper.Instance.Decrypt(elementsByTagName.Item(0).ChildNodes[3].InnerText);
                    string windowAuthen = elementsByTagName.Item(0).ChildNodes[4].InnerText;
                    //Build an Entity Framework connection string
                    var entityString = new EntityConnectionStringBuilder()
                    {
                        Provider = "System.Data.SqlClient",
                        //nhớ sua lai model name
                        Metadata = "res://*/HMSModel.csdl|res://*/HMSModel.ssdl|res://*/HMSModel.msl"
                    };
                    if (!Boolean.Parse(windowAuthen))
                    {
                        entityString.ProviderConnectionString = @"data source=" + serverName + ";initial catalog=" + databaseName + ";user id=" + login + ";password=" + password + ";MultipleActiveResultSets=true;";
                    }
                    else
                    {
                        entityString.ProviderConnectionString = @"data source=" + serverName + ";initial catalog=" + databaseName + ";integrated security=True;MultipleActiveResultSets=true;";
                    }
                    return entityString.ConnectionString;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string getNumber(int index)
        {
            string num = "00000";
            if (index < 10)
                num = "0000" + index;
            else if (index > 10 && index < 100)
                num = "000" + index;
            else if (index > 100 && index < 1000)
                num = "00" + index;
            else if (index > 1000 && index < 10000)
                num = "0" + index;
            else
                num = index.ToString();
            return num;
        }
    }
}