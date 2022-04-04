namespace Application.Common.Exceptions;

public class WrongPasswordException : Exception
{
    public WrongPasswordException()
        : base("Wrong password")
    { }

    public WrongPasswordException(string message)
        : base(message)
    { }
}