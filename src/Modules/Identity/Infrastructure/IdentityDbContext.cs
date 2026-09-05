using CIOT.Common.Data;
using CIOT.Modules.Identity.Domain;
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
    public DbSet<AuthProviderConfig> AuthProviderConfigs => Set<AuthProviderConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

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
            b.HasIndex(x => x.Name).IsUnique();
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Permission>(b =>
        {
            b.ToTable("permissions");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Code).IsUnique();
            b.Property(x => x.Code).HasMaxLength(100).IsRequired();
            b.Property(x => x.Category).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<RolePermission>(b =>
        {
            b.ToTable("role_permissions");
            b.HasKey(x => new { x.RoleId, x.PermissionId });
            b.HasOne(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserRoleAssignment>(b =>
        {
            b.ToTable("user_role_assignments");
            b.HasKey(x => new { x.UserId, x.RoleId });
            b.HasOne(x => x.UserAccount)
                .WithMany(x => x.RoleAssignments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Role)
                .WithMany(x => x.UserAssignments)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserScopeAssignment>(b =>
        {
            b.ToTable("user_scope_assignments");
            b.HasKey(x => x.Id);
            b.HasOne(x => x.UserAccount)
                .WithMany(x => x.ScopeAssignments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            b.Property(x => x.ScopeType).HasMaxLength(50).IsRequired();
            b.Property(x => x.ScopeValue).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<ApiClient>(b =>
        {
            b.ToTable("api_clients");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.ClientId).IsUnique();
            b.Property(x => x.ClientId).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<AuthProviderConfig>(b =>
        {
            b.ToTable("auth_provider_configs");
            b.HasKey(x => x.ProviderKey);
            b.Property(x => x.ProviderKey).HasMaxLength(100);
            b.Property(x => x.IssuerUrl).HasMaxLength(500).IsRequired();
        });
    }
}
