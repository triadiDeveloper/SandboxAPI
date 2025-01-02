using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
    }

    public abstract class BaseGuidDto
    {
        public Guid Id { get; set; }
    }

    public abstract class BaseCodeNameDto : BaseDto
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
    }

    public abstract class BaseCodeNameGuidDto : BaseGuidDto
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
    }

    public abstract class BaseDomainDetailDto : BaseDto
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
    }

    public abstract class BaseDomainDetailGuidDto : BaseGuidDto
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
