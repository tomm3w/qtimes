using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeatQ.core.dal.Models
{
    [Table("web_application")]
    public class WebApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppId { get; set; }
        public string AppName { get; set; }
        public ICollection<WebApplicationUser> Users { get; set; }
        
    }
}