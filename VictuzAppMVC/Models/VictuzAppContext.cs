using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VictuzAppMVC.Models;

public partial class VictuzAppContext : DbContext
{
    public VictuzAppContext()
    {
    }

    public VictuzAppContext(DbContextOptions<VictuzAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aanmeldingen> Aanmeldingens { get; set; }

    public virtual DbSet<Activiteiten> Activiteitens { get; set; }

    public virtual DbSet<EvenementCategorieën> EvenementCategorieëns { get; set; }

    public virtual DbSet<Gebruiker> Gebruikers { get; set; }

    public virtual DbSet<Lidmaatschappen> Lidmaatschappens { get; set; }

    public virtual DbSet<ViewActiviteitenAanmeldingen> ViewActiviteitenAanmeldingens { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aanmeldingen>(entity =>
        {
            entity.HasKey(e => e.AanmeldingId).HasName("PK__Aanmeldi__D705C3B943894FF4");

            entity.ToTable("Aanmeldingen");

            entity.HasIndex(e => new { e.GebruikerId, e.ActiviteitId }, "UQ_Gebruiker_Activiteit").IsUnique();

            entity.Property(e => e.AanmeldingId).HasColumnName("AanmeldingID");
            entity.Property(e => e.AanmeldDatum)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ActiviteitId).HasColumnName("ActiviteitID");
            entity.Property(e => e.GebruikerId).HasColumnName("GebruikerID");

            entity.HasOne(d => d.Activiteit).WithMany(p => p.Aanmeldingens)
                .HasForeignKey(d => d.ActiviteitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Aanmeldin__Activ__2C3393D0");

            entity.HasOne(d => d.Gebruiker).WithMany(p => p.Aanmeldingens)
                .HasForeignKey(d => d.GebruikerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Aanmeldin__Gebru__2B3F6F97");
        });

        modelBuilder.Entity<Activiteiten>(entity =>
        {
            entity.HasKey(e => e.ActiviteitId).HasName("PK__Activite__B14595E035BB1C25");

            entity.ToTable("Activiteiten");

            entity.Property(e => e.ActiviteitId).HasColumnName("ActiviteitID");
            entity.Property(e => e.Beschrijving).HasMaxLength(500);
            entity.Property(e => e.Datum).HasColumnType("datetime");
            entity.Property(e => e.Titel).HasMaxLength(100);

            entity.HasMany(d => d.Categories).WithMany(p => p.Activiteits)
                .UsingEntity<Dictionary<string, object>>(
                    "ActiviteitCategorieKoppeling",
                    r => r.HasOne<EvenementCategorieën>().WithMany()
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Activitei__Categ__4222D4EF"),
                    l => l.HasOne<Activiteiten>().WithMany()
                        .HasForeignKey("ActiviteitId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Activitei__Activ__412EB0B6"),
                    j =>
                    {
                        j.HasKey("ActiviteitId", "CategorieId").HasName("PK__Activite__CE21AF388DC1EBB2");
                        j.ToTable("ActiviteitCategorieKoppeling");
                        j.IndexerProperty<int>("ActiviteitId").HasColumnName("ActiviteitID");
                        j.IndexerProperty<int>("CategorieId").HasColumnName("CategorieID");
                    });
        });

        modelBuilder.Entity<EvenementCategorieën>(entity =>
        {
            entity.HasKey(e => e.CategorieId).HasName("PK__Evenemen__F643AD86180B9053");

            entity.ToTable("EvenementCategorieën");

            entity.Property(e => e.CategorieId).HasColumnName("CategorieID");
            entity.Property(e => e.CategorieNaam).HasMaxLength(100);
        });

        modelBuilder.Entity<Gebruiker>(entity =>
        {
            entity.HasKey(e => e.GebruikerId).HasName("PK__Gebruike__4282ABF5F068F3BC");

            entity.HasIndex(e => e.Email, "UQ__Gebruike__A9D1053470D9A463").IsUnique();

            entity.Property(e => e.GebruikerId).HasColumnName("GebruikerID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.LidmaatschapId).HasColumnName("LidmaatschapID");
            entity.Property(e => e.Naam).HasMaxLength(100);
            entity.Property(e => e.Wachtwoord).HasMaxLength(50);

            entity.HasOne(d => d.Lidmaatschap).WithMany(p => p.Gebruikers)
                .HasForeignKey(d => d.LidmaatschapId)
                .HasConstraintName("FK__Gebruiker__Lidma__44FF419A");
        });

        modelBuilder.Entity<Lidmaatschappen>(entity =>
        {
            entity.HasKey(e => e.LidmaatschapId).HasName("PK__Lidmaats__27325D7CD2D147F7");

            entity.ToTable("Lidmaatschappen");

            entity.Property(e => e.LidmaatschapId).HasColumnName("LidmaatschapID");
            entity.Property(e => e.Beschrijving).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewActiviteitenAanmeldingen>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ActiviteitenAanmeldingen");

            entity.Property(e => e.ActiviteitId).HasColumnName("ActiviteitID");
            entity.Property(e => e.Datum).HasColumnType("datetime");
            entity.Property(e => e.Titel).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
