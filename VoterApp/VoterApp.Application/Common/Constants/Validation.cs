namespace VoterApp.Application.Common.Constants;

public static class Validation
{
    public const int MaxNameLength = 25;
    public const int MinNameLength = 3;

    public const int MaxTopicLength = 150;
    public const int MinTopicLength = 3;

    public static class Messages
    {
        public const string IsRequired = "{PropertyName} is required.";
        public const string MustBeUniqueInElection = "'{PropertyName}' must be unique in election.";
        public const string MustExistElection = "'{PropertyName}' must have corresponding existing Election.";

        public static readonly string MaxNameLength =
            "{PropertyName}" + $" must be less or equal {Validation.MaxNameLength} characters.";

        public static readonly string MinNameLength =
            "{PropertyName}" + $" must be at least {Validation.MinNameLength} characters.";

        public static readonly string MaxTopicLength =
            "{PropertyName}" + $" must be less or equal {Validation.MaxTopicLength} characters.";

        public static readonly string MinTopicLength =
            "{PropertyName}" + $" must be at least {Validation.MinTopicLength} characters.";
    }
}