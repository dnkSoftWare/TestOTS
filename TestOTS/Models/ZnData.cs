using System;
using System.Security.Cryptography;

namespace TestOTS.Models
{
    public class ZnData
    {
        public DateTime ZnDate { get; set; }
        public string Name { get; set; }
        public int ZnFrom { get; set; }
        public int ZnTo { get; set; }
        public string ZnIdss { get; set; }
    }
}