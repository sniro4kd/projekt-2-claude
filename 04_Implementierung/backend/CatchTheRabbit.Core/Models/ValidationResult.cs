namespace CatchTheRabbit.Core.Models;

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; private set; }
    public string? ErrorMessage { get; private set; }

    private ValidationResult(bool isValid, string? errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationResult Success() => new(true);

    /// <summary>
    /// Creates a failed validation result with an error message.
    /// </summary>
    public static ValidationResult Fail(string message) => new(false, message);
}
