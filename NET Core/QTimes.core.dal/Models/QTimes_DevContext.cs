using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QTimes.core.dal.Models
{
    public partial class QTimesContext : DbContext
    {
        public QTimesContext()
        {
        }

        public QTimesContext(DbContextOptions<QTimesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<BusinessDetail> BusinessDetail { get; set; }
        public virtual DbSet<BusinessType> BusinessType { get; set; }
        public virtual DbSet<Concert> Concert { get; set; }
        public virtual DbSet<ConcertEvent> ConcertEvent { get; set; }
        public virtual DbSet<ConcertEventGuest> ConcertEventGuest { get; set; }
        public virtual DbSet<ConcertEventMessageRule> ConcertEventMessageRule { get; set; }
        public virtual DbSet<ConcertEventReservation> ConcertEventReservation { get; set; }
        public virtual DbSet<ConcertEventReservationMessage> ConcertEventReservationMessage { get; set; }
        public virtual DbSet<ConcertEventSeatLock> ConcertEventSeatLock { get; set; }
        public virtual DbSet<ConcertEventSeating> ConcertEventSeating { get; set; }
        public virtual DbSet<DeliveryStatus> DeliveryStatus { get; set; }
        public virtual DbSet<GuestType> GuestType { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<ReservationBusiness> ReservationBusiness { get; set; }
        public virtual DbSet<ReservationGuest> ReservationGuest { get; set; }
        public virtual DbSet<ReservationMessage> ReservationMessage { get; set; }
        public virtual DbSet<ReservationMessageRule> ReservationMessageRule { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<BusinessDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BusinessName).HasMaxLength(255);

                entity.Property(e => e.CummunityGuidelinesFilePath).HasMaxLength(1024);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.IsDlscanEnabled).HasColumnName("IsDLScanEnabled");

                entity.Property(e => e.LogoPath).HasMaxLength(512);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OpenDayFrom)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OpenDayTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrivacyPolicyFilePath).HasMaxLength(1024);

                entity.Property(e => e.ServiceTermsFilePath).HasMaxLength(1024);

                entity.Property(e => e.ShortUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.VirtualNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BusinessType)
                    .WithMany(p => p.BusinessDetail)
                    .HasForeignKey(d => d.BusinessTypeId)
                    .HasConstraintName("FK_BusinessDetail_BusinessType");

                entity.HasOne(d => d.ReservationBusiness)
                    .WithMany(p => p.BusinessDetail)
                    .HasForeignKey(d => d.ReservationBusinessId)
                    .HasConstraintName("FK_BusinessDetail_ReservationBusiness");
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Concert>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CummunityGuidelinesFilePath).HasMaxLength(1024);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.ImagePath).HasMaxLength(1024);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrivacyPolicyFilePath).HasMaxLength(1024);

                entity.Property(e => e.ServiceTermsFilePath).HasMaxLength(1024);

                entity.Property(e => e.VirtualNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ReservationBusiness)
                    .WithMany(p => p.Concert)
                    .HasForeignKey(d => d.ReservationBusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Concert_ReservationBusiness");
            });

            modelBuilder.Entity<ConcertEvent>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CummunityGuidelinesFilePath).HasMaxLength(1024);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.ImagePath).HasMaxLength(1024);

                entity.Property(e => e.IsDlscanEnabled).HasColumnName("IsDLScanEnabled");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrivacyPolicyFilePath).HasMaxLength(1024);

                entity.Property(e => e.SeatMapPath).HasMaxLength(1024);

                entity.Property(e => e.SeatType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceTermsFilePath).HasMaxLength(1024);

                entity.Property(e => e.ShortUrl).HasMaxLength(512);

                entity.Property(e => e.VirtualNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Concert)
                    .WithMany(p => p.ConcertEvent)
                    .HasForeignKey(d => d.ConcertId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConcertEvent_Concert");
            });

            modelBuilder.Entity<ConcertEventGuest>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.CheckInTime).HasColumnType("datetime");

                entity.Property(e => e.City).HasMaxLength(150);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FamilyName).HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(150);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Photo).HasMaxLength(1024);

                entity.Property(e => e.SeatNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State).HasMaxLength(150);

                entity.Property(e => e.Temperature)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Zip).HasMaxLength(150);

                entity.HasOne(d => d.ConcertEventReservation)
                    .WithMany(p => p.ConcertEventGuest)
                    .HasForeignKey(d => d.ConcertEventReservationId)
                    .HasConstraintName("FK_ConcertGuest_ConcertReservation");
            });

            modelBuilder.Entity<ConcertEventMessageRule>(entity =>
            {
                entity.Property(e => e.BeforeAfter)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InOut)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Message).HasMaxLength(512);

                entity.Property(e => e.MessageType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValueType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConcertEvent)
                    .WithMany(p => p.ConcertEventMessageRule)
                    .HasForeignKey(d => d.ConcertEventId)
                    .HasConstraintName("FK_ConcertEventMessageRule_ConcertEvent");
            });

            modelBuilder.Entity<ConcertEventReservation>(entity =>
            {
                entity.Property(e => e.ConfirmationNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PassUrl).HasMaxLength(512);

                entity.Property(e => e.Seatings)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConcertEvent)
                    .WithMany(p => p.ConcertEventReservation)
                    .HasForeignKey(d => d.ConcertEventId)
                    .HasConstraintName("FK_ConcertEventReservation_ConcertEvent");

                entity.HasOne(d => d.GuestType)
                    .WithMany(p => p.ConcertEventReservation)
                    .HasForeignKey(d => d.GuestTypeId)
                    .HasConstraintName("FK_ConcertReservation_GuestType");
            });

            modelBuilder.Entity<ConcertEventReservationMessage>(entity =>
            {
                entity.Property(e => e.MessageReplied).HasMaxLength(50);

                entity.Property(e => e.MessageRepliedDateTime).HasColumnType("datetime");

                entity.Property(e => e.MessageSent).HasMaxLength(512);

                entity.Property(e => e.MessageSentDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.ConcertEventReservation)
                    .WithMany(p => p.ConcertEventReservationMessage)
                    .HasForeignKey(d => d.ConcertEventReservationId)
                    .HasConstraintName("FK_ConcertReservationMessage_ConcertReservation");
            });

            modelBuilder.Entity<ConcertEventSeatLock>(entity =>
            {
                entity.Property(e => e.LockTime).HasColumnType("datetime");

                entity.Property(e => e.ReleaseTime).HasColumnType("datetime");

                entity.Property(e => e.SeatNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConcertEvent)
                    .WithMany(p => p.ConcertEventSeatLock)
                    .HasForeignKey(d => d.ConcertEventId)
                    .HasConstraintName("FK_ConcertEventSeatLock_ConcertEvent");
            });

            modelBuilder.Entity<ConcertEventSeating>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConcertEvent)
                    .WithMany(p => p.ConcertEventSeating)
                    .HasForeignKey(d => d.ConcertEventId)
                    .HasConstraintName("FK_ConcertEventSeating_ConcertEvent");
            });

            modelBuilder.Entity<DeliveryStatus>(entity =>
            {
                entity.Property(e => e.ErrCode)
                    .HasColumnName("err_code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MessageId)
                    .HasColumnName("messageId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MessageTimestamp).HasColumnName("message_timestamp");

                entity.Property(e => e.Msisdn)
                    .HasColumnName("msisdn")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetworkCode)
                    .HasColumnName("network_code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Project)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Scts)
                    .HasColumnName("scts")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.To)
                    .HasColumnName("to")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GuestType>(entity =>
            {
                entity.Property(e => e.GuestType1)
                    .HasColumnName("GuestType")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(e => e.CancelledDateTime).HasColumnType("datetime");

                entity.Property(e => e.Comments).HasMaxLength(512);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.PassUrl).HasMaxLength(512);

                entity.Property(e => e.TimeIn).HasColumnType("datetime");

                entity.Property(e => e.TimeOut).HasColumnType("datetime");

                entity.Property(e => e.UtcDateTimeFrom).HasColumnType("datetime");

                entity.Property(e => e.UtcDateTimeTo).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessDetail)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.BusinessDetailId)
                    .HasConstraintName("FK_Reservation_BusinessDetail");

                entity.HasOne(d => d.GuestType)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.GuestTypeId)
                    .HasConstraintName("FK_Reservation_GuestType");

                entity.HasOne(d => d.ReservationGuest)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.ReservationGuestId)
                    .HasConstraintName("FK_Reservation_ReservationGuest");
            });

            modelBuilder.Entity<ReservationBusiness>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(512);

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.City).HasMaxLength(512);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.State).HasMaxLength(512);

                entity.Property(e => e.TimezoneOffset)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimezoneOffsetValue)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReservationGuest>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.City).HasMaxLength(105);

                entity.Property(e => e.Dob).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FamilyName).HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(150);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.State).HasMaxLength(150);

                entity.Property(e => e.Zip).HasMaxLength(150);

                entity.HasOne(d => d.BusinessDetail)
                    .WithMany(p => p.ReservationGuest)
                    .HasForeignKey(d => d.BusinessDetailId)
                    .HasConstraintName("FK_ReservationGuest_BusinessDetail");
            });

            modelBuilder.Entity<ReservationMessage>(entity =>
            {
                entity.Property(e => e.MessageReplied).HasMaxLength(50);

                entity.Property(e => e.MessageRepliedDateTime).HasColumnType("datetime");

                entity.Property(e => e.MessageSent).HasMaxLength(512);

                entity.Property(e => e.MessageSentDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.ReservationMessage)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK_ReservationMessage_Reservation");
            });

            modelBuilder.Entity<ReservationMessageRule>(entity =>
            {
                entity.Property(e => e.BeforeAfter)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InOut)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Message).HasMaxLength(512);

                entity.Property(e => e.MessageType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValueType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BusinessDetail)
                    .WithMany(p => p.ReservationMessageRule)
                    .HasForeignKey(d => d.BusinessDetailId)
                    .HasConstraintName("FK_ReservationMessageRule_BusinessDetail");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_UserInfo_1");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.HasOne(d => d.ReservationBusiness)
                    .WithMany(p => p.UserInfo)
                    .HasForeignKey(d => d.ReservationBusinessId)
                    .HasConstraintName("FK_UserInfo_ReservationBusiness");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
