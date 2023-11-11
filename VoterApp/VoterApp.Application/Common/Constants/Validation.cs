namespace VoterApp.Application.Common.Constants;

public static class Validation
{
    public const int MaxNameLength = 25;

    public static class Messages
    {
        public const string IsRequired = "{PropertyName} is required.";
        public const string MustBeUnique = "'{PropertyName}' must be unique.";

        public static readonly string MaxLength =
            "{PropertyName}" + $" must be less or equal {MaxNameLength} characters.";
    }
}