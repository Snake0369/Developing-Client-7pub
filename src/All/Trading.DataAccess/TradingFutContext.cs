using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;
using Index = Trading.Bot.Domain.Contracts.Index;

namespace Trading.Bot.DataAccess
{
    public class TradingFutContext : DbContext
    {
        public TradingFutContext(DbContextOptions<TradingFutContext> options) : base(options)
        {
        }

        public DbSet<Index> Indexes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<TradeHistory> TradesHistories { get; set; }
        public DbSet<FileHistory> FileHistories { get; set; }
        public DbSet<BarHistory> BarHistories { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<Equity> Equities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Subposition> Subpositions { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<TradeDto> TradeDtos { get; set; }
        public DbSet<OrderDto> OrderDtos { get; set; }
        public DbSet<TradeAllDto> TradeAllDtos { get; set; }
        public DbSet<TransactionDto> TransactionDtos { get; set; }
        public DbSet<FileArchive> FileArchives { get; set; }
        public DbSet<Renko> Renkos { get; set; }
        public DbSet<CommentMap> CommentMaps { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderChannel> OrderChannels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("trading_bot_5");

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contracts");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Expired).HasColumnType("timestamp without time zone");
                entity.Property(e => e.LastTradingDate).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<TradeHistory>(entity =>
            {
                entity.ToTable("TradeHistories");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<FileHistory>(entity =>
            {
                entity.ToTable("FileHistories");
                entity.HasKey("Id");
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(120);
                entity.Property(e => e.LastModified).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<BarHistory>(entity =>
            {
                entity.ToTable("Bars");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Strategy>(entity =>
            {
                entity.ToTable("Strategies");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Equity>(entity =>
            {
                entity.ToTable("Equities");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).IsRequired().HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Positions");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Created).IsRequired().HasColumnType("timestamp without time zone");
                entity.Property(e => e.LastUpdated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Subposition>(entity =>
            {
                entity.ToTable("Subpositions");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Created).IsRequired().HasColumnType("timestamp without time zone");
                entity.Property(e => e.LastUpdated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Trade>(entity =>
            {
                entity.ToTable("Trades");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
                entity.Property(e => e.Created).IsRequired().HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Created).IsRequired().HasColumnType("timestamp without time zone");
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
                entity.HasOne<Contract>().WithMany().HasForeignKey(p => p.ContractId);
            });
            modelBuilder.Entity<TradeDto>(entity =>
            {
                entity.ToTable("TradeDto");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<OrderDto>(entity =>
            {
                entity.ToTable("OrderDto");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<BarDto>(entity =>
            {
                entity.ToTable("BarDto");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");
                entity.Property(e => e.Timestamp2).HasColumnType("timestamp without time zone");
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<TradeAllDto>(entity =>
            {
                entity.ToTable("TradeAllDto");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<TransactionDto>(entity =>
            {
                entity.ToTable("TransactionDto");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<FileArchive>(entity =>
            {
                entity.ToTable("FileArchive");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Updated).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Renko>(entity =>
            {
                entity.ToTable("Renkos");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<CommentMap>(entity =>
            {
                entity.ToTable("CommentMaps");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<OrderChannel>(entity =>
            {
                entity.ToTable("OrderChannel");
                entity.HasKey("Id");
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
