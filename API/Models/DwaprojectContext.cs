using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class DwaprojectContext : DbContext
{
    public DwaprojectContext()
    {
    }

    public DwaprojectContext(DbContextOptions<DwaprojectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=.;Database=DWAProject;User Id=sas;Password=SQL;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC072C97E101");

            entity.Property(e => e.Description).HasDefaultValueSql("('No description')");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC072A3DCF01");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_Users");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC07DE6BA2CF");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0749E9AB60");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Videos__3214EC07A12753CA");

            entity.HasMany(d => d.Genres).WithMany(p => p.Videos)
                .UsingEntity<Dictionary<string, object>>(
                    "VideoGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("FK_VideoGenres_Genres"),
                    l => l.HasOne<Video>().WithMany()
                        .HasForeignKey("VideoId")
                        .HasConstraintName("FK_VideoGenres_Videos"),
                    j =>
                    {
                        j.HasKey("VideoId", "GenreId");
                        j.ToTable("VideoGenres");
                    });

            entity.HasMany(d => d.Tags).WithMany(p => p.Videos)
                .UsingEntity<Dictionary<string, object>>(
                    "VideoTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_VideoTags_Tags"),
                    l => l.HasOne<Video>().WithMany()
                        .HasForeignKey("VideoId")
                        .HasConstraintName("FK_VideoTags_Videos"),
                    j =>
                    {
                        j.HasKey("VideoId", "TagId");
                        j.ToTable("VideoTags");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
