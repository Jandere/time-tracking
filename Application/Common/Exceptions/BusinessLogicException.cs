namespace Application.Common.Exceptions;

public class BusinessLogicException : Exception
{
    public BusinessLogicException() : base()
    {
        
    }
    
    public BusinessLogicException(string message) : base(message)
    {
        
    }
}