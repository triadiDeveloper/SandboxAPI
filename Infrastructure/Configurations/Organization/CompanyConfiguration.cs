using Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presentation.Configurations.Organization
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company", schema: "Organization");

            builder.HasIndex(x => x.Code).IsUnique(true);
            builder.HasIndex(x => x.Name);

            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)");
            builder.Property(x => x.ModifiedDate).HasColumnType("datetime");
            builder.Property(x => x.Version).IsRowVersion();
        }
    }
}