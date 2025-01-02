using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Identity
{
    public class ApplicationUserApprovalDetailConfiguration : IEntityTypeConfiguration<ApplicationUserApprovalDetail>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserApprovalDetail> builder)
        {
            builder.ToTable("ApplicationUserApprovalDetail", schema: "Identity");

            builder.Property(x => x.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)");
            builder.Property(x => x.ModifiedDate).HasColumnType("datetime");
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.Version).IsRowVersion();
        }
    }
}