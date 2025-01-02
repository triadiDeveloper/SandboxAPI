namespace Domain.BaseEntity
{
    public class ResponseModel
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Message { get; set; } = default!;
    }

    public class ResponseCodes
    {
        //public const string LE01 = "User Not Registered";
        public static readonly Dictionary<dynamic, dynamic> Dict = new Dictionary<dynamic, dynamic>()
        {
            { "0000", "Sukses" },
            { "U001", "Token tidak valid" },
            { "U002", "Token sudah kedaluwarsa" },
            { "U003", "Role tidak memiliki akses endpoint" },
            { "U004", "UserId tidak ditemukan" },
            { "U005", "User dan Password tidak sesuai" },
            { "U006", "User tidak aktif" },
            { "U007", "Karyawan sudah resign" },
        };
    }
}
