using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUIRequest
{
    internal class Request
    {
        public int id { get; set; }
        public int statusID { get; set; }
        public string statusName { get; set; }
        public string clientFIO { get; set; }
        public int courierID { get; set; }
        public string fio { get; set; }
        public double rating { get; set; }
        public string address { get; set; }
        public string text { get; set; }
        public string canceledText { get; set; }
    }
}
