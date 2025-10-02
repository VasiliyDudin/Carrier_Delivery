using System.ComponentModel.DataAnnotations;

namespace MajorRequestServer.Models
{
    public class BaseModel
    {
        [Key]
        public int ID { get; set; }
    }
}
