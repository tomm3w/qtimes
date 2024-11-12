using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatQ.core.dal.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("AuthenticationConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<WebApplication> WebApplications { get; set; }
        public DbSet<WebApplicationUser> WebAppUsers { get; set; }
    }
}
