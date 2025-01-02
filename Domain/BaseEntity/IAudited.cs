using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.BaseEntity;

public interface IAudited
{
    [StringLength(255)]
    public string? CreatedUser { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedDate { get; set; }
    [StringLength(255)]
    public string? ModifiedUser { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

