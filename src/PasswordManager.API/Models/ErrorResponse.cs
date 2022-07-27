namespace PasswordManager.API.Models;

internal class ErrorResponse
{
    public List<ErrorModel> Errors { get; set; } = new();
}

internal class ErrorModel
{
    public string FieldName { get; set; } = default!;

    public string Message { get; set; } = default!;
}