using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ETemizlik.Models
{
    public partial class AuthUseContext : DbContext
    {
        public AuthUseContext()
        {
        }

        public AuthUseContext(DbContextOptions<AuthUseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<BeyazEsyaBilgisi> BeyazEsyaBilgisis { get; set; } = null!;
        public virtual DbSet<BosEvTemizligiSipari> BosEvTemizligiSiparis { get; set; } = null!;
        public virtual DbSet<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; } = null!;
        public virtual DbSet<EvTemizligiSipari> EvTemizligiSiparis { get; set; } = null!;
        public virtual DbSet<HaliBilgisi> HaliBilgisis { get; set; } = null!;
        public virtual DbSet<Ilce> Ilces { get; set; } = null!;
        public virtual DbSet<InsaatTemizligiSipari> InsaatTemizligiSiparis { get; set; } = null!;
        public virtual DbSet<KatBilgisi> KatBilgisis { get; set; } = null!;
        public virtual DbSet<KoltukBilgisi> KoltukBilgisis { get; set; } = null!;
        public virtual DbSet<OdaBilgisi> OdaBilgisis { get; set; } = null!;
        public virtual DbSet<RandevuSaat> RandevuSaats { get; set; } = null!;
        public virtual DbSet<Sehir> Sehirs { get; set; } = null!;
        public virtual DbSet<TeknolojikAletBilgisi> TeknolojikAletBilgisis { get; set; } = null!;
        public virtual DbSet<YatakBilgisi> YatakBilgisis { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=HPYUSUF;Initial Catalog=ETemizlik;TrustServerCertificate=true;trusted_connection=yes ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<BeyazEsyaBilgisi>(entity =>
            {
                entity.Property(e => e.BeyazEsyaId).ValueGeneratedNever();
            });

            modelBuilder.Entity<BosEvTemizligiSipari>(entity =>
            {
                entity.HasOne(d => d.Ilce)
                    .WithMany(p => p.BosEvTemizligiSiparis)
                    .HasForeignKey(d => d.IlceId)
                    .HasConstraintName("FK_BosEvTemizligiSiparis_Ilce");

                entity.HasOne(d => d.KacKatliNavigation)
                    .WithMany(p => p.BosEvTemizligiSiparis)
                    .HasForeignKey(d => d.KacKatli)
                    .HasConstraintName("FK_BosEvTemizligiSiparis_KatBilgisi");

                entity.HasOne(d => d.KacOdaliNavigation)
                    .WithMany(p => p.BosEvTemizligiSiparis)
                    .HasForeignKey(d => d.KacOdali)
                    .HasConstraintName("FK_BosEvTemizligiSiparis_OdaBilgisi");

                entity.HasOne(d => d.Saat)
                    .WithMany(p => p.BosEvTemizligiSiparis)
                    .HasForeignKey(d => d.SaatId)
                    .HasConstraintName("FK_BosEvTemizligiSiparis_RandevuSaat");

                entity.HasOne(d => d.Sehir)
                    .WithMany(p => p.BosEvTemizligiSiparis)
                    .HasForeignKey(d => d.SehirId)
                    .HasConstraintName("FK_BosEvTemizligiSiparis_Sehir");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BosEvTemizligiSiparis)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BosEvTemizligiSiparis_AspNetUsers");
            });

            modelBuilder.Entity<EsyaTemizligiSipari>(entity =>
            {
                entity.HasOne(d => d.BeyazEsyaSayisiNavigation)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.BeyazEsyaSayisi)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_BeyazEsyaBilgisi");

                entity.HasOne(d => d.HaliSayisiNavigation)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.HaliSayisi)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_HaliBilgisi");

                entity.HasOne(d => d.Ilce)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.IlceId)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_Ilce");

                entity.HasOne(d => d.KoltukSayisiNavigation)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.KoltukSayisi)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_KoltukBilgisi");

                entity.HasOne(d => d.Saat)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.SaatId)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_RandevuSaat");

                entity.HasOne(d => d.Sehir)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.SehirId)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_Sehir");

                entity.HasOne(d => d.TeknolojikAletSayisiNavigation)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.TeknolojikAletSayisi)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_TeknolojikAletBilgisi");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_AspNetUsers");

                entity.HasOne(d => d.YatakSayisiNavigation)
                    .WithMany(p => p.EsyaTemizligiSiparis)
                    .HasForeignKey(d => d.YatakSayisi)
                    .HasConstraintName("FK_EsyaTemizligiSiparis_YatakBilgisi");
            });

            modelBuilder.Entity<EvTemizligiSipari>(entity =>
            {
                entity.HasOne(d => d.Ilce)
                    .WithMany(p => p.EvTemizligiSiparis)
                    .HasForeignKey(d => d.IlceId)
                    .HasConstraintName("FK_EvTemizligiSiparis_Ilce");

                entity.HasOne(d => d.KacKatliNavigation)
                    .WithMany(p => p.EvTemizligiSiparis)
                    .HasForeignKey(d => d.KacKatli)
                    .HasConstraintName("FK_EvTemizligiSiparis_KatBilgisi");

                entity.HasOne(d => d.KacOdaliNavigation)
                    .WithMany(p => p.EvTemizligiSiparis)
                    .HasForeignKey(d => d.KacOdali)
                    .HasConstraintName("FK_EvTemizligiSiparis_OdaBilgisi");

                entity.HasOne(d => d.Saat)
                    .WithMany(p => p.EvTemizligiSiparis)
                    .HasForeignKey(d => d.SaatId)
                    .HasConstraintName("FK_EvTemizligiSiparis_RandevuSaat");

                entity.HasOne(d => d.Sehir)
                    .WithMany(p => p.EvTemizligiSiparis)
                    .HasForeignKey(d => d.SehirId)
                    .HasConstraintName("FK_EvTemizligiSiparis_Sehir");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EvTemizligiSiparis)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_EvTemizligiSiparis_AspNetUsers");
            });

            modelBuilder.Entity<HaliBilgisi>(entity =>
            {
                entity.Property(e => e.HaliId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Ilce>(entity =>
            {
                entity.Property(e => e.IlceId).ValueGeneratedNever();

                entity.HasOne(d => d.Sehir)
                    .WithMany(p => p.Ilces)
                    .HasForeignKey(d => d.SehirId)
                    .HasConstraintName("FK_Ilce_Sehir");
            });

            modelBuilder.Entity<InsaatTemizligiSipari>(entity =>
            {
                entity.HasOne(d => d.Ilce)
                    .WithMany(p => p.InsaatTemizligiSiparis)
                    .HasForeignKey(d => d.IlceId)
                    .HasConstraintName("FK_InsaatTemizligiSiparis_Ilce");

                entity.HasOne(d => d.KacKatliNavigation)
                    .WithMany(p => p.InsaatTemizligiSiparis)
                    .HasForeignKey(d => d.KacKatli)
                    .HasConstraintName("FK_InsaatTemizligiSiparis_KatBilgisi");

                entity.HasOne(d => d.KacOdaliNavigation)
                    .WithMany(p => p.InsaatTemizligiSiparis)
                    .HasForeignKey(d => d.KacOdali)
                    .HasConstraintName("FK_InsaatTemizligiSiparis_OdaBilgisi");

                entity.HasOne(d => d.Saat)
                    .WithMany(p => p.InsaatTemizligiSiparis)
                    .HasForeignKey(d => d.SaatId)
                    .HasConstraintName("FK_InsaatTemizligiSiparis_RandevuSaat");

                entity.HasOne(d => d.Sehir)
                    .WithMany(p => p.InsaatTemizligiSiparis)
                    .HasForeignKey(d => d.SehirId)
                    .HasConstraintName("FK_InsaatTemizligiSiparis_Sehir");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InsaatTemizligiSiparis)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_InsaatTemizligiSiparis_AspNetUsers");
            });

            modelBuilder.Entity<KatBilgisi>(entity =>
            {
                entity.Property(e => e.KatId).ValueGeneratedNever();
            });

            modelBuilder.Entity<KoltukBilgisi>(entity =>
            {
                entity.Property(e => e.KoltukBilgisiId).ValueGeneratedNever();
            });

            modelBuilder.Entity<OdaBilgisi>(entity =>
            {
                entity.Property(e => e.OdaId).ValueGeneratedNever();
            });

            modelBuilder.Entity<RandevuSaat>(entity =>
            {
                entity.Property(e => e.RandevuSaatId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sehir>(entity =>
            {
                entity.Property(e => e.SehirId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TeknolojikAletBilgisi>(entity =>
            {
                entity.Property(e => e.TeknolojikAletId).ValueGeneratedNever();
            });

            modelBuilder.Entity<YatakBilgisi>(entity =>
            {
                entity.Property(e => e.YatakId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
