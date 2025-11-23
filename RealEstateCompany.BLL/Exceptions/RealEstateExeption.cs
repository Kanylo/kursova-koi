// RealEstateCompany.BLL/Exceptions/
namespace RealEstateCompany.BLL.Exceptions
{
    public class RealEstateException : Exception
    {
        public RealEstateException(string message) : base(message) { }
    }

    public class MaximumProposalsExceededException : RealEstateException
    {
        public MaximumProposalsExceededException()
            : base("Cannot add more than 5 properties to client proposal") { }
    }

    public class ClientNotFoundException : RealEstateException
    {
        public ClientNotFoundException(int id)
            : base($"Client with ID {id} not found") { }
    }

    public class RealEstateNotFoundException : RealEstateException
    {
        public RealEstateNotFoundException(int id)
            : base($"Real estate with ID {id} not found") { }
    }
}