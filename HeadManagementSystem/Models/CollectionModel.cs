using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeadManagementSystem.Models
{
    public class CollectionModel
    {
        public List<ModelSelectItem> item1 { get; set; }
        public List<ModelSelectItem> item2 { get; set; }
        public List<ModelSelectItem> item3 { get; set; }
        public List<ModelSelectItem> item4 { get; set; }
        public List<ModelSelectItem> item5 { get; set; }
        public string SellNumber { get; set; }
        public string SerNumber { get; set; }
        public int index { get; set; }
        public int int1 { get; set; }
        public int int2 { get; set; } 
    }
}