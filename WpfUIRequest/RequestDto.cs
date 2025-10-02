using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUIRequest
{
    internal class RequestDto
    {
        public int ID { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string ClientFIO { get; set; }
        public int CourierID { get; set; }
        public string FIO { get; set; }
        public double Rating { get; set; }
        public string Address { get; set; }
        public string Text { get; set; }
        public string CanceledText { get; set; }
    }
}
