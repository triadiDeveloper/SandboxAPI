using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.BaseEntity;

public interface IVersion
{
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Timestamp]
    public byte[]? Version { get; set; }
}
