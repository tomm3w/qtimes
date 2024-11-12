using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeatQ.core.dal.Models
{
    [Table("web_application_user")]
    public class WebApplicationUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WebApplicationUserId { get; set; }
        UserProfile User { get; set; }
        WebApplication WebApp { get; set; }
    }
}