namespace Application.Common.Exceptions;

public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException() : base("User with this username already exist")
    {
        
    }

    public UserAlreadyExistException(string message) : base(message)
    {
        
    }
}