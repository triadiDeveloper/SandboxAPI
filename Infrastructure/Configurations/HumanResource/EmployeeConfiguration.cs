using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.HumanResource;

namespace Presentation.Configurations.HumanResource
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee", schema: "HumanResource");

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