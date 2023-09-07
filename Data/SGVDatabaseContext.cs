﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SGV_Booking.Models;

namespace SGV_Booking.Data
{
    public partial class SGVDatabaseContext : DbContext
    {
        public SGVDatabaseContext()
        {
        }

        public SGVDatabaseContext(DbContextOptions<SGVDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Banquet> Banquets { get; set; } = null!;
        public virtual DbSet<BanquetItem> BanquetItems { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=BIMIMBAPY\\SQLEXPRESS;Initial Catalog=SGVDatabase;Integrated Security=True");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).HasColumnName("addressID");

                entity.Property(e => e.AddressLine)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("addressLine");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country");

                entity.Property(e => e.Postcode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("postcode");

                entity.Property(e => e.Region)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("region");

                entity.Property(e => e.Suburb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("suburb");
            });

            modelBuilder.Entity<Banquet>(entity =>
            {
                entity.Property(e => e.BanquetId).HasColumnName("banquetID");

                entity.Property(e => e.BanquetMinPeople).HasColumnName("banquetMinPeople");

                entity.Property(e => e.BanquetName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("banquetName");

                entity.Property(e => e.BanquetPrice).HasColumnName("banquetPrice");

                entity.Property(e => e.RestaurantId).HasColumnName("restaurantID");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Banquets)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_restaurantBanquetID");
            });

            modelBuilder.Entity<BanquetItem>(entity =>
            {
                entity.Property(e => e.BanquetItemId).HasColumnName("banquetItemID");

                entity.Property(e => e.BanquetDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("banquetDesc");

                entity.Property(e => e.BanquetId).HasColumnName("banquetID");

                entity.Property(e => e.BanquetItem1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("banquetItem");

                entity.HasOne(d => d.Banquet)
                    .WithMany(p => p.BanquetItems)
                    .HasForeignKey(d => d.BanquetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_banquetID");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.BookingId).HasColumnName("bookingID");

                entity.Property(e => e.BanquetOption).HasColumnName("banquetOption");

                entity.Property(e => e.BookingNotes)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("bookingNotes");

                entity.Property(e => e.BookingTime)
                    .HasColumnType("datetime")
                    .HasColumnName("bookingTime");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.RestaurantId).HasColumnName("restaurantID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_customerID");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_restaurantID");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.RestaurantId).HasColumnName("restaurantID");

                entity.Property(e => e.RestaurantAddressId).HasColumnName("restaurantAddressID");

                entity.Property(e => e.RestaurantName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("restaurantName");

                entity.Property(e => e.RestaurantPhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("restaurantPhoneNumber");

                entity.HasOne(d => d.RestaurantAddress)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.RestaurantAddressId)
                    .HasConstraintName("fk_addressID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .HasColumnName("password")
                    .IsFixedLength();

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.UserType).HasColumnName("userType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
