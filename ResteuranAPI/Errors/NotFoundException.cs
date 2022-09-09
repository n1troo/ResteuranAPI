namespace ResteuranAPI.Errors;

public class NotFoundException : Exception
{
    public NotFoundException(string message) :base(message)
    {
        
    }
}