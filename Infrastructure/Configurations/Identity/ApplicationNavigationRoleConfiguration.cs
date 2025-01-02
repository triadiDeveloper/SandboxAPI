using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Identity
{
    public class ApplicationNavigationRoleConfiguration : IEntityTypeConfiguration<ApplicationNavigationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationNavigationRole> builder)
        {
            builder.ToTable("ApplicationNavigationRole", schema: "Identity");

            builder.Property(x => x.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)");
            builder.Property(x => x.ModifiedDate).HasColumnType("datetime");
            builder.Property(x => x.Version).IsRowVersion();
        }
    }
}