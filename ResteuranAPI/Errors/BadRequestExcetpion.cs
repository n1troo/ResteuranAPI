namespace ResteuranAPI.Errors;

public class BadRequestExcetpion : Exception
{
    public BadRequestExcetpion(string message): base(message)
    {

    }
}