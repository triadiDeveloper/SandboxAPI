using Microsoft.EntityFrameworkCore;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Identity
{
    public class ApplicationNavigationConfiguration : IEntityTypeConfiguration<ApplicationNavigation>
    {
        public void Configure(EntityTypeBuilder<ApplicationNavigation> builder)
        {
            builder.ToTable("ApplicationNavigation", schema: "Identity");

            builder.HasIndex(x => x.Code).IsUnique(true);
            builder.HasIndex(x => new { x.Name, x.Icon, x.Url, x.ParentCode });

            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)");
            builder.Property(x => x.ModifiedDate).HasColumnType("datetime");
            builder.Property(x => x.Version).IsRowVersion();
        }
    }
}