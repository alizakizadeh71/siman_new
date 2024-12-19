namespace Enums
{
    public enum ServicesResponse : int
    {
        Success = 1,
        NotResponse = 0,
        Error = -1,

        InsertError = -2,
        UpdateError = -3,
        ConflictError = -4,
        DeleteError = -5,
        ModelStateError = -6,

        NotFoundError = -20,
    }
}
