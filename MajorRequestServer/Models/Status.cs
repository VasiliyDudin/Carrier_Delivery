using MajorRequestServer.Repository;
using System.ComponentModel.DataAnnotations;

namespace MajorRequestServer.Models
{
    public class Status : BaseModel
    {
        [StringLength(50)]
        public string StatusName { get; set; }
    }
}
