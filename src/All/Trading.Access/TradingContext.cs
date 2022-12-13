using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.Contracts;
using Trading.Domain.Contracts.Enums;

namespace Trading.DataAccess
{
    public class TradingContext : DbContext
    {
        public TradingContext(DbContextOptions<TradingContext> options) : base(options)
        {
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<Equity> Equities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Subposition> Subpositions { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<FuturePosition> FuturePositions { get; set; }
        public DbSet<ReturnIssue> Returns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("trading_cl10_bot");

            modelBuilder.Entity<Strategy>(entity =>
            {
                entity.ToTable("Strategies");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
            });
            modelBuilder.Entity<Equity>(entity =>
            {
                entity.ToTable("Equities");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
            });
            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Positions");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.TimestampTakeOn).HasColumnType("timestamp with time zone");
                entity.Property(e => e.TimestampStop).HasColumnType("timestamp with time zone");
            });
            modelBuilder.Entity<Subposition>(entity =>
            {
                entity.ToTable("Subpositions");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
            });
            modelBuilder.Entity<Trade>(entity =>
            {
                entity.ToTable("Trades");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
            });
            modelBuilder.Entity<FuturePosition>(entity =>
            {
                entity.ToTable("FuturePositions");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
            });
            modelBuilder.Entity<ReturnIssue>(entity =>
            {
                entity.ToTable("Returns");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.FirstPositionsTimestamp).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.LastPositionsTimestamp).IsRequired().HasColumnType("timestamp with time zone");
            });
        }
    }
}
