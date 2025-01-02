using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Identity
{
    public class ApplicationUserApprovalConfiguration : IEntityTypeConfiguration<ApplicationUserApproval>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserApproval> builder)
        {
            builder.ToTable("ApplicationUserApproval", schema: "Identity");

            builder.HasMany(x => x.ApplicationUserApprovalDetails).WithOne(y => y.ApplicationUserApproval).OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.DocumentDate).HasColumnType("date");
            builder.Property(x => x.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)");
            builder.Property(x => x.ModifiedDate).HasColumnType("datetime");
            builder.Property(x => x.Version).IsRowVersion();
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        }
    }
}