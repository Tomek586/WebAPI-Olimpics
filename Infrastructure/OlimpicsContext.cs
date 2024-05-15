using System;
using System.Collections.Generic;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class OlimpicsContext : DbContext
{
    public OlimpicsContext()
    {
    }

    public OlimpicsContext(DbContextOptions<OlimpicsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CompetitorEvent> CompetitorEvents { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GamesCity> GamesCities { get; set; }

    public virtual DbSet<GamesCompetitor> GamesCompetitors { get; set; }

    public virtual DbSet<Medal> Medals { get; set; }

    public virtual DbSet<NocRegion> NocRegions { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonRegion> PersonRegions { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Olimpics;Username=postgres;Password=admin;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("city_pkey");

            entity.ToTable("city", "olympics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CityName)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("city_name");
        });

        modelBuilder.Entity<CompetitorEvent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("competitor_event", "olympics");

            entity.HasIndex(e => e.CompetitorId, "IX_competitor_event_competitor_id");

            entity.HasIndex(e => e.EventId, "IX_competitor_event_event_id");

            entity.HasIndex(e => e.MedalId, "IX_competitor_event_medal_id");

            entity.Property(e => e.CompetitorId).HasColumnName("competitor_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.MedalId).HasColumnName("medal_id");

            entity.HasOne(d => d.Competitor).WithMany()
                .HasForeignKey(d => d.CompetitorId)
                .HasConstraintName("fk_ce_com");

            entity.HasOne(d => d.Event).WithMany()
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("fk_ce_ev");

            entity.HasOne(d => d.Medal).WithMany()
                .HasForeignKey(d => d.MedalId)
                .HasConstraintName("fk_ce_med");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_event");

            entity.ToTable("event", "olympics");

            entity.HasIndex(e => e.SportId, "IX_event_sport_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventName)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("event_name");
            entity.Property(e => e.SportId).HasColumnName("sport_id");

            entity.HasOne(d => d.Sport).WithMany(p => p.Events)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("fk_ev_sp");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_games");

            entity.ToTable("games", "olympics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GamesName)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("games_name");
            entity.Property(e => e.GamesYear).HasColumnName("games_year");
            entity.Property(e => e.Season)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("season");
        });

        modelBuilder.Entity<GamesCity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("games_city", "olympics");

            entity.HasIndex(e => e.CityId, "IX_games_city_city_id");

            entity.HasIndex(e => e.GamesId, "IX_games_city_games_id");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.GamesId).HasColumnName("games_id");

            entity.HasOne(d => d.City).WithMany()
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("fk_gci_city");

            entity.HasOne(d => d.Games).WithMany()
                .HasForeignKey(d => d.GamesId)
                .HasConstraintName("fk_gci_gam");
        });

        modelBuilder.Entity<GamesCompetitor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_gamescomp");

            entity.ToTable("games_competitor", "olympics");

            entity.HasIndex(e => e.GamesId, "IX_games_competitor_games_id");

            entity.HasIndex(e => e.PersonId, "IX_games_competitor_person_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.GamesId).HasColumnName("games_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");

            entity.HasOne(d => d.Games).WithMany(p => p.GamesCompetitors)
                .HasForeignKey(d => d.GamesId)
                .HasConstraintName("fk_gc_gam");

            entity.HasOne(d => d.Person).WithMany(p => p.GamesCompetitors).HasForeignKey(d => d.PersonId);
        });

        modelBuilder.Entity<Medal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_medal");

            entity.ToTable("medal", "olympics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MedalName)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("medal_name");
        });

        modelBuilder.Entity<NocRegion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_nocregion");

            entity.ToTable("noc_region", "olympics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Noc)
                .HasMaxLength(5)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("noc");
            entity.Property(e => e.RegionName)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("region_name");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_person");

            entity.ToTable("person", "olympics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("gender");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        modelBuilder.Entity<PersonRegion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("person_region", "olympics");

            entity.HasIndex(e => e.PersonId, "IX_person_region_person_id");

            entity.HasIndex(e => e.RegionId, "IX_person_region_region_id");

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.RegionId).HasColumnName("region_id");

            entity.HasOne(d => d.Person).WithMany()
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("fk_per_per");

            entity.HasOne(d => d.Region).WithMany()
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("fk_per_reg");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sport_pkey");

            entity.ToTable("sport", "olympics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SportName)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("sport_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
