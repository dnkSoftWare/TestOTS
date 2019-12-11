using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Linq;
using System.Security.Cryptography;

namespace TestOTS.Models
{
    public class ZnData
    {
        public DateTime ZnDate { get; set; }
        public string Name { get; set; }
        public int? ZnFrom { get; set; }
        public int? ZnTo { get; set; }
        public int? AllHours { get; set; }
        public string ZnIdss { get; set; }
    }

    public class ZnHead
    {
        public DateTime ZnDate { get; set; }
        public string Fio { get; set; }
        public int? AllHours { get; set; }
        public List<ZnDetail> Zn {get; set; }
//        public int?[] FromHours { get; set; }
    }

    public class ZnDetail
    {
        public int? ZnFrom { get; set; }
        public int? ZnTo { get; set; }
        public int? ZnAllHours { get; set; }
        public string ZnIdss { get; set; }
    }

    public class ViewDataModel
    {
        public string Title = "";

        public ViewDataModel(string title, List<ZnHead> data)
        {
            this.Title = title;
            this.data = data;
        }

        public List<ZnHead> data { get; set; }
    }
}