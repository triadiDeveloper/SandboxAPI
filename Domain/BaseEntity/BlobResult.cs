namespace Application.BaseEntity
{
    public class BlobResult
    {
        public byte[] FileByte { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public string FileName { get; set; } = default!;
    }
}
