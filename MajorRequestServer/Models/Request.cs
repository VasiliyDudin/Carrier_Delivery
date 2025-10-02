using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MajorRequestServer.Models
{
    public class Request : BaseModel
    {
        public int StatusID { get; set; }
        [StringLength(50)]
        public string ClientFIO { get; set; }
        public int CourierID { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(200)]
        public string Text { get; set; }
        [StringLength(200)]
        public string CanceledText { get; set; }
    }
}
