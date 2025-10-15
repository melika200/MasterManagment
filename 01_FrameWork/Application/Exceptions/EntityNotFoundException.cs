namespace _01_FrameWork.Application.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, object key)
        : base($"Entity \"{entityName}\" with key \"{key}\" was not found.")
    {
    }
}
