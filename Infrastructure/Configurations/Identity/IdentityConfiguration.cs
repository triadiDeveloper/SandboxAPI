using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Identity
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Ubah nama tabel untuk IdentityUser
            builder.ToTable("ApplicationUser", "Identity");
            builder.HasMany(e => e.ApplicationUserClaims)
                    .WithOne(e => e.ApplicationUser)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

            builder.HasIndex(x => new { x.EmployeeId, x.UserName }).IsUnique(true);

            // Each User can have many UserLogins
            builder.HasMany(e => e.ApplicationUserLogins)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            builder.HasMany(e => e.ApplicationUserTokens)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            builder.HasMany(e => e.ApplicationUserRoles)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            // Konfigurasi tambahan untuk kolom
            builder.Property(u => u.UserName).HasMaxLength(256).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.PhoneNumber).HasMaxLength(20);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)");
            builder.Property(x => x.ModifiedDate).HasColumnType("datetime");
            builder.Property(x => x.Version).IsRowVersion();
            builder.Property(x => x.SVersion).HasComputedColumnSql("CONVERT(NVARCHAR(MAX), CONVERT(BINARY(8), Version), 1)");
        }
    }

    public class IdentityRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            // Ubah nama tabel untuk IdentityRole
            builder.ToTable("ApplicationRole", "Identity");

            // Each Role can have many entries in the UserRole join table
            builder.HasMany(e => e.ApplicationUserRoles)
                .WithOne(e => e.ApplicationRole)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            builder.HasMany(e => e.ApplicationRoleClaims)
                .WithOne(e => e.ApplicationRole)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();

            builder.HasMany(x => x.ApplicationRoleClaims).WithOne(y => y.ApplicationRole).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.Version).IsRowVersion();
            builder.Property(x => x.SVersion).HasComputedColumnSql("CONVERT(NVARCHAR(MAX), CONVERT(BINARY(8), Version), 1)");
        }
    }

    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            // Ubah nama tabel untuk IdentityUserRole
            builder.ToTable("ApplicationUserRole", "Identity");
            builder.Property(x => x.SVersion).HasComputedColumnSql("CONVERT(NVARCHAR(MAX), CONVERT(BINARY(8), Version), 1)");
        }
    }

    public class IdentityUserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            // Ubah nama tabel untuk IdentityUserClaim
            builder.ToTable("ApplicationUserClaim", "Identity");
            builder.HasIndex(x => new { x.UserId, x.ApplicationControllerMethodId }).IsUnique(true);
        }
    }

    public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            // Ubah nama tabel untuk IdentityRoleClaim
            builder.ToTable("ApplicationRoleClaim", "Identity");
            builder.HasIndex(x => new { x.RoleId, x.ApplicationControllerMethodId }).IsUnique(true);
        }
    }

    public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<ApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            // Ubah nama tabel untuk IdentityUserLogin
            builder.ToTable("ApplicationUserLogin", "Identity");
        }
    }

    public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            // Ubah nama tabel untuk IdentityUserToken
            builder.ToTable("ApplicationUserToken", "Identity");
        }
    }

    public class IdentityEndpointConfiguration : IEntityTypeConfiguration<ApplicationEndpoint>
    {
        public void Configure(EntityTypeBuilder<ApplicationEndpoint> builder)
        {
            builder.ToTable("ApplicationEndPoint", schema: "Identity");
            builder.HasIndex(x => new { x.Name }).IsUnique(true);
            builder.Property("Name").HasMaxLength(255);
        }
    }

    public class IdentityUserCompanyConfiguration : IEntityTypeConfiguration<ApplicationUserCompany>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserCompany> builder)
        {
            builder.ToTable("ApplicationUserCompany", schema: "Identity");
            builder.HasIndex(x => new { x.ApplicationUserId, x.CompanyId }).IsUnique(true);
        }
    }

    public class IdentityUserInfoConfiguration : IEntityTypeConfiguration<ApplicationUserInfo>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserInfo> builder)
        {
            builder.ToTable("ApplicationUserInfo", schema: "Identity");
            builder.HasIndex(x => new { x.ApplicationUserId, x.IpAddress, x.DeviceName });
            builder.Property(x => x.LoginDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
        }
    }

    public class IdentityControllerConfiguration : IEntityTypeConfiguration<ApplicationController>
    {
        public void Configure(EntityTypeBuilder<ApplicationController> builder)
        {
            builder.ToTable("ApplicationController", schema: "Identity");
            builder.HasMany(x => x.ApplicationControllerMethods).WithOne(y => y.ApplicationController).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.Name }).IsUnique(true);
        }
    }

    public class IdentityControllerMethodConfiguration : IEntityTypeConfiguration<ApplicationControllerMethod>
    {
        public void Configure(EntityTypeBuilder<ApplicationControllerMethod> builder)
        {
            builder.ToTable("ApplicationControllerMethod", schema: "Identity");
            builder.HasIndex(x => new { x.Name, x.ApplicationControllerId }).IsUnique(true);
        }
    }
}