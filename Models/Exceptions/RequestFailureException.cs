namespace BoatRecords.Models.Exceptions;

class RequestFailureException : Exception
{
    public RequestFailureException(string message) : base(message) { }
}
