using System;
using System.IO;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;


#nullable disable

namespace BusinessObject
{
    public partial class BirthdayPartyBookingContext : DbContext
    {
        public BirthdayPartyBookingContext()
        {
        }

        public BirthdayPartyBookingContext(DbContextOptions<BirthdayPartyBookingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            string test = configuration.GetConnectionString("MyDB");
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDB"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("ACCOUNT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Name)
                    .HasMaxLength(300)
                    .HasColumnName("NAME");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .HasColumnName("PHONE")
                    .IsFixedLength(true);

                entity.Property(e => e.Role).HasColumnName("ROLE");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("ORDER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("DATE");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.GuestId).HasColumnName("GUEST_ID");

                entity.Property(e => e.HostId).HasColumnName("HOST_ID");

                entity.Property(e => e.Note)
                    .HasMaxLength(300)
                    .HasColumnName("NOTE");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ORDER_DATE");

                entity.Property(e => e.PlaceId).HasColumnName("PLACE_ID");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasColumnName("STATUS");

                entity.Property(e => e.TotalPrice).HasColumnName("TOTAL_PRICE");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.GuestId)
                    .HasConstraintName("FK_ORDER_GUEST_ID");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PlaceId)
                    .HasConstraintName("FK_ORDER_PLACE");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("ORDER_DETAIL");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.OrderId).HasColumnName("ORDER_ID");

                entity.Property(e => e.Price).HasColumnName("PRICE");

                entity.Property(e => e.ServiceId).HasColumnName("SERVICE_ID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_ORDER_DETAIL_ORDER_ID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_ORDER_DETAIL_SERVICE_ID");
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.ToTable("PLACE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .HasMaxLength(1000)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.HostId).HasColumnName("HOST_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(300)
                    .HasColumnName("NAME");

                entity.Property(e => e.Price).HasColumnName("PRICE");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.Places)
                    .HasForeignKey(d => d.HostId)
                    .HasConstraintName("FK_PLACE_HOST_ID");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("SERVICE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.HostId).HasColumnName("HOST_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(300)
                    .HasColumnName("NAME");

                entity.Property(e => e.Price).HasColumnName("PRICE");

                entity.Property(e => e.ServiceTypeId).HasColumnName("SERVICE_TYPE_ID");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.HostId)
                    .HasConstraintName("FK_SERVICE_HOST_ID");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_SERVICE_SERVICE_TYPE_ID");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("SERVICE_TYPE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("NAME");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
