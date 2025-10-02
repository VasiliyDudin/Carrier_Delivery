using System.ComponentModel.DataAnnotations;

namespace MajorRequestServer.Models
{
    public class Courier : BaseModel
    {
        [StringLength(50)]
        public string FIO { get; set; }
        public bool Access { get; set; }
        public double Rating { get; set; }
    }
}
