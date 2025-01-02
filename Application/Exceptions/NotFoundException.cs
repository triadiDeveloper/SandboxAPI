namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(int id, string message) : base($"{message} dengan Id {id} tidak ditemukan.")
        {
            
        }

        public NotFoundException(Guid id, string message) : base($"{message} dengan Id {id} tidak ditemukan.")
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }
    }
}
