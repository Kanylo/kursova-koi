namespace RealEstateApp.BLL.Exceptions
{
    public class RealEstateException : Exception
    {
        public RealEstateException(string message) : base(message) { }
        public RealEstateException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class ValidationException : RealEstateException
    {
        public ValidationException(string message) : base(message) { }
    }

    public class NotFoundException : RealEstateException
    {
        public NotFoundException(string entityName, int id)
            : base($"{entityName} with ID {id} not found") { }
    }
}