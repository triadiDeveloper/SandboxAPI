namespace Application.BaseEntity;

public interface ISoftDeletable
{
    bool? IsDeleted { get; set; }
}

