using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RelatedKeywordLibrary.Models
{
    public partial class UserContext : DbContext
    {
        public UserContext()
        {
        }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Searchhistory> Searchhistories { get; set; }
        public virtual DbSet<Searchresult> Searchresults { get; set; }
        public virtual DbSet<Userinfo> Userinfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Server=localhost;User=norah;Database=userdb;Port=3306;Password=userNorah20@@;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Searchhistory>(entity =>
            {
                entity.HasKey(e => e.HistoryIndex)
                    .HasName("PRIMARY");

                entity.ToTable("searchhistory");

                entity.Property(e => e.HistoryIndex).HasColumnName("history_index");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Keyword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("keyword");

                entity.Property(e => e.Result).HasColumnName("result");

                entity.Property(e => e.UserKey).HasColumnName("user_key");
            });

            modelBuilder.Entity<Searchresult>(entity =>
            {
                entity.HasKey(e => new { e.Seq, e.ResultKey })
                    .HasName("PRIMARY");

                entity.ToTable("searchresult");

                entity.HasIndex(e => e.HistoryIndex, "history_index");

                entity.Property(e => e.Seq).HasColumnName("seq");

                entity.Property(e => e.ResultKey)
                    .HasMaxLength(50)
                    .HasColumnName("result_key");

                entity.Property(e => e.HistoryIndex).HasColumnName("history_index");

                entity.Property(e => e.MobileCnt).HasColumnName("mobile_cnt");

                entity.Property(e => e.PcCnt).HasColumnName("pc_cnt");

                entity.Property(e => e.RelatedKeyword)
                    .HasMaxLength(50)
                    .HasColumnName("related_keyword");

                entity.Property(e => e.SumCnt).HasColumnName("sum_cnt");

                entity.HasOne(d => d.HistoryIndexNavigation)
                    .WithMany(p => p.Searchresults)
                    .HasForeignKey(d => d.HistoryIndex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("searchresult_ibfk_1");
            });

            modelBuilder.Entity<Userinfo>(entity =>
            {
                entity.HasKey(e => e.UserKey)
                    .HasName("PRIMARY");

                entity.ToTable("userinfo");

                entity.Property(e => e.UserKey).HasColumnName("user_key");

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.EditDate).HasColumnName("edit_date");

                entity.Property(e => e.Ip)
                    .HasMaxLength(50)
                    .HasColumnName("ip");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .HasColumnName("user_id");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(50)
                    .HasColumnName("user_password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
