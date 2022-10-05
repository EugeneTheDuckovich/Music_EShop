using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace eshop.Models;

public class EShopFullDbContext : IdentityDbContext<ApplicationUser>
{
    EShopFullDbContext(DbContextOptions<EShopFullDbContext> options) : base(options)
    {
    }


    public EShopFullDbContext()
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();

        //Instruments.Add(new Instrument
        //{
        //    Producer = "Yamaha",
        //    Model = "G100",
        //    Price = 500,
        //    BriefDescription = "Acoustic guitar",
        //    ImageSource = ""
        //});
        //SaveChanges();
        //Guitars.Add(new Guitar
        //{
        //    Instrument = Instruments.First(),
        //    NumberOfStrings = 6,
        //    BodyMaterial = "Mahagony",
        //    DeckMaterial = "Mahagony"
        //});

        //Instruments.Add(new Instrument
        //{
        //    Producer = "Yamaha",
        //    Model = "S200",
        //    Price = 500,
        //    BriefDescription = "Piano",
        //    ImageSource = ""
        //});
        //SaveChanges();
        //Keyboards.Add(new Keyboard
        //{
        //    Instrument = Instruments.First(i => i.Id == 2),
        //    NumberOfOctaves = 6,
        //    Material = "Mahagony"
        //});
        //SaveChanges();
    }


    public virtual DbSet<Instrument> Instruments { get; set; } = null!;
    public virtual DbSet<Guitar> Guitars { get; set; } = null!;
    public virtual DbSet<Keyboard> Keyboards { get; set; } = null!;
    public virtual DbSet<Purchase> Purchases { get; set; } = null!;
    public virtual DbSet<PurchaseProduct> PurchaseProducts { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=EShop;Integrated Security=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.Entity<Role>(entity =>
        //{
        //    entity.ToTable("Role", "Identity");

        //    entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
        //        .IsUnique()
        //        .HasFilter("([NormalizedName] IS NOT NULL)");

        //    entity.Property(e => e.Name).HasMaxLength(256);

        //    entity.Property(e => e.NormalizedName).HasMaxLength(256);
        //});

        //modelBuilder.Entity<RoleClaim>(entity =>
        //{
        //    entity.ToTable("RoleClaims", "Identity");

        //    entity.HasIndex(e => e.RoleId, "IX_RoleClaims_RoleId");

        //    entity.HasOne(d => d.Role)
        //        .WithMany(p => p.RoleClaims)
        //        .HasForeignKey(d => d.RoleId);
        //});

        //modelBuilder.Entity<User>(entity =>
        //{
        //    entity.ToTable("User", "Identity");

        //    entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

        //    entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
        //        .IsUnique()
        //        .HasFilter("([NormalizedUserName] IS NOT NULL)");

        //    entity.Property(e => e.Discriminator).HasDefaultValueSql("(N'')");

        //    entity.Property(e => e.Email).HasMaxLength(256);

        //    entity.Property(e => e.FirstName).HasMaxLength(25);

        //    entity.Property(e => e.LastName).HasMaxLength(25);

        //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

        //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

        //    entity.Property(e => e.UserName).HasMaxLength(256);

        //    entity.HasMany(d => d.Roles)
        //        .WithMany(p => p.Users)
        //        .UsingEntity<Dictionary<string, object>>(
        //            "UserRole",
        //            l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
        //            r => r.HasOne<User>().WithMany().HasForeignKey("UserId"),
        //            j =>
        //            {
        //                j.HasKey("UserId", "RoleId");

        //                j.ToTable("UserRoles", "Identity");

        //                j.HasIndex(new[] { "RoleId" }, "IX_UserRoles_RoleId");
        //            });
        //});

        //modelBuilder.Entity<UserClaim>(entity =>
        //{
        //    entity.ToTable("UserClaims", "Identity");

        //    entity.HasIndex(e => e.UserId, "IX_UserClaims_UserId");

        //    entity.HasOne(d => d.User)
        //        .WithMany(p => p.UserClaims)
        //        .HasForeignKey(d => d.UserId);
        //});

        //modelBuilder.Entity<UserLogin>(entity =>
        //{
        //    entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

        //    entity.ToTable("UserLogins", "Identity");

        //    entity.HasIndex(e => e.UserId, "IX_UserLogins_UserId");

        //    entity.Property(e => e.LoginProvider).HasMaxLength(128);

        //    entity.Property(e => e.ProviderKey).HasMaxLength(128);

        //    entity.HasOne(d => d.User)
        //        .WithMany(p => p.UserLogins)
        //        .HasForeignKey(d => d.UserId);
        //});

        //modelBuilder.Entity<UserToken>(entity =>
        //{
        //    entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

        //    entity.ToTable("UserTokens", "Identity");

        //    entity.Property(e => e.LoginProvider).HasMaxLength(128);

        //    entity.Property(e => e.Name).HasMaxLength(128);

        //    entity.HasOne(d => d.User)
        //        .WithMany(p => p.UserTokens)
        //        .HasForeignKey(d => d.UserId);
        //});

        modelBuilder.Entity<Instrument>(entity =>
        {
            entity.ToTable("Instruments");
            entity.HasKey("Id");
        });

        modelBuilder.Entity<Guitar>(entity =>
        {
            entity.ToTable("Guitars");
        });

        modelBuilder.Entity<Keyboard>(entity =>
        {
            entity.ToTable("Keyboards");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchases");
            entity.HasOne(e => e.User).WithMany(i => i.Purchases);
        });

        modelBuilder.Entity<PurchaseProduct>(entity =>
        {
            entity.ToTable("PurchaseProducts");

            entity.HasKey(pp => pp.Id);

            entity.HasOne(pp => pp.Purchase).WithMany(p => p.PurchaseProducts).HasForeignKey(pp => pp.PurchaseId);

            entity.HasOne(pp => pp.Instrument).WithMany(i => i.PurchaseProducts).HasForeignKey(pp => pp.InstrumentId);
        });

        //OnModelCreatingPartial(modelBuilder);
    }
}