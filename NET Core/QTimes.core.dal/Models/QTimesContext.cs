using Microsoft.EntityFrameworkCore;
using System;

namespace QTimes.core.dal.Models
{
    public partial class QTimesContext : DbContext
    {
        public virtual DbSet<TimeSlot> TimeSlots { get; set; }
        public static void SetConnectionString(string connectionString)
        {
            if (ConnectionString == null)

            {
                ConnectionString = connectionString;
            }
            else
            {
                throw new Exception();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        private static string ConnectionString;
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.HasNoKey();
            });
            base.OnModelCreating(modelBuilder);
        }
    }

}
