using CIOT.Common.Data;
using CIOT.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Identity.Infrastructure;

public sealed class IdentityDbContext : BaseDbContext
{
    public const string Schema = "identity";

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserRoleAssignment> UserRoleAssignments => Set<UserRoleAssignment>();
    public DbSet<UserScopeAssignment> UserScopeAssignments => Set<UserScopeAssignment>();
    public DbSet<ApiClient> ApiClients => Set<ApiClient>();
    public DbSet<UserActivation> UserActivations => Set<UserActivation>();
    public DbSet<RolePermissionBaseline> RolePermissionBaselines => Set<RolePermissionBaseline>();
    public DbSet<UserDeviceManufacturerAccess> UserDeviceManufacturerAccesses => Set<UserDeviceManufacturerAccess>();
    public DbSet<UserAssetManufacturerAccess> UserAssetManufacturerAccesses => Set<UserAssetManufacturerAccess>();
    public DbSet<MobileTermsDocument> MobileTermsDocuments => Set<MobileTermsDocument>();
    public DbSet<MobileTermsAcceptance> MobileTermsAcceptances => Set<MobileTermsAcceptance>();
    public DbSet<MobileProfilePreference> MobileProfilePreferences => Set<MobileProfilePreference>();
    public DbSet<AuthRevocationMarker> AuthRevocationMarkers => Set<AuthRevocationMarker>();
    public DbSet<ApiClientPermission> ApiClientPermissions => Set<ApiClientPermission>();
    public DbSet<ApiClientDeviceManufacturerAccess> ApiClientDeviceManufacturerAccesses => Set<ApiClientDeviceManufacturerAccess>();
    public DbSet<ActivationDeliveryOutbox> ActivationDeliveryOutboxes => Set<ActivationDeliveryOutbox>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

        modelBuilder.Entity<UserAccount>(b =>
        {
            b.ToTable("user_accounts");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Email).IsUnique();
            b.HasIndex(x => x.ExternalIdentityId);
            b.Property(x => x.Email).HasMaxLength(255).IsRequired();
            b.Property(x => x.UserType).HasMaxLength(50).IsRequired();
            b.Property(x => x.Status).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Role>(b =>
        {
            b.ToTable("roles");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.RoleCode).IsUnique();
            b.Property(x => x.RoleCode).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Permission>(b =>
        {
            b.ToTable("permissions");
            b.HasKey(x => x.PermissionCode);
            b.Property(x => x.PermissionCode).HasMaxLength(100).IsRequired();
            b.Property(x => x.Category).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<RolePermission>(b =>
        {
            b.ToTable("role_permissions");
            b.HasKey(x => new { x.RoleId, x.PermissionCode });
            b.HasOne(x => x.Role)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionCode)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserRoleAssignment>(b =>
        {
            b.ToTable("user_role_assignments");
            b.HasKey(x => new { x.UserId, x.RoleId });
            b.HasOne(x => x.User)
                .WithMany(x => x.RoleAssignments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserScopeAssignment>(b =>
        {
            b.ToTable("user_scope_assignments");
            b.HasKey(x => x.Id);
            b.HasOne(x => x.User)
                .WithMany(x => x.ScopeAssignments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            b.Property(x => x.ScopeType).HasMaxLength(50).IsRequired();
            b.Property(x => x.ScopeCode).HasMaxLength(100);
        });

        modelBuilder.Entity<ApiClient>(b =>
        {
            b.ToTable("api_clients");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.EntraClientId).IsUnique();
            b.Property(x => x.EntraClientId).HasMaxLength(100).IsRequired();
        });
    }
}
