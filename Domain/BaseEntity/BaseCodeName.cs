using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.BaseEntity;

public abstract class BaseIdGUID : IIdentityGuid
{
    public Guid Id { get; set; }
}

public abstract class BaseIdInt : IIdentityInt
{
    public int Id { get; set; }
}

public abstract class BaseCodeNameGuid : BaseIdGUID, IDefaultColumn, INote, IActivatable, IAudited, IVersion, IAuditable
{
    [DisplayName("Kode")]
    [MaxLength(15)]
    public string Code { get; set; } = default!;
    [DisplayName("Nama")]
    [MaxLength(255)]
    public string Name { get; set; } = default!;

    [MaxLength(2000)]
    public string? Note { get; set; }

    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }

    [MaxLength(255)]
    public string? CreatedUser { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedDate { get; set; }

    [MaxLength(255)]
    public string? ModifiedUser { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [Timestamp]
    public byte[]? Version { get; set; }

}

public abstract class BaseCodeName : BaseIdInt, IDefaultColumn, INote, IActivatable, IAudited, IVersion, IAuditable
{
    [DisplayName("Kode")]
    [MaxLength(15)]
    public string Code { get; set; } = default!;

    [DisplayName("Nama")]
    [MaxLength(255)]
    public string Name { get; set; } = default!;

    [MaxLength(2000)]
    public string? Note { get; set; }

    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }

    [MaxLength(255)]
    public string? CreatedUser { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedDate { get; set; }

    [MaxLength(255)]
    public string? ModifiedUser { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [Timestamp]
    public byte[]? Version { get; set; }

}

public abstract class BaseDomainDetail : IIdentityInt, IActivatable, ISoftDeletable, IAudited, IVersion, IAuditable
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }

    [MaxLength(255)]
    public string? CreatedUser { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedDate { get; set; }

    [MaxLength(255)]
    public string? ModifiedUser { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [Timestamp]
    public byte[]? Version { get; set; }

}

public abstract class BaseDomainDetailGuid : BaseIdGUID, IActivatable, ISoftDeletable, IAudited, IVersion, IAuditable
{
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }

    [MaxLength(255)]
    public string? CreatedUser { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedDate { get; set; }

    [MaxLength(255)]
    public string? ModifiedUser { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [Timestamp]
    public byte[]? Version { get; set; }

}

public abstract class BaseDomainDeep : IIdentityInt, IAudited, IVersion, IAuditable
{
    public int Id { get; set; }

    [MaxLength(255)]
    public string? CreatedUser { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedDate { get; set; }

    [MaxLength(255)]
    public string? ModifiedUser { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [Timestamp]
    public byte[]? Version { get; set; }

}

public abstract class BaseDomainDeepGuid : IIdentityGuid, IAudited, IVersion, IAuditable
{
    public Guid Id { get; set; }

    [MaxLength(255)]
    public string? CreatedUser { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedDate { get; set; }

    [MaxLength(255)]
    public string? ModifiedUser { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [Timestamp]
    public byte[]? Version { get; set; }

}