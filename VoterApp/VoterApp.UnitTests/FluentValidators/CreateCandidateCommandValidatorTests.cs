using Shouldly;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;

namespace VoterApp.UnitTests.FluentValidators;

public class CreateCandidateCommandValidatorTests : IClassFixture<ValidatorFixture>
{
    private readonly ValidatorFixture _fixture;

    public CreateCandidateCommandValidatorTests(ValidatorFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Validate_NameTooLong_ShouldReturnValidationError()
    {
        // Arrange
        var createCandidateCommand = _fixture.ValidCreateCandidateCommand with
        {
            Name = "Super Long Name That Doesnt Even Exist And Never Will"
        };
        var validator = new CreateCandidateCommandValidator(_fixture.MockCandidateRepo.Object);

        // Act
        var result = await validator.ValidateAsync(createCandidateCommand);

        // Assert
        result.Errors[0].PropertyName.ShouldBe(nameof(createCandidateCommand.Name));
    }

    [Fact]
    public async Task Validate_NameTooShort_ShouldReturnValidationError()
    {
        // Arrange
        var createCandidateCommand = _fixture.ValidCreateCandidateCommand with { Name = "A" };
        var validator = new CreateCandidateCommandValidator(_fixture.MockCandidateRepo.Object);
        // Act
        var result = await validator.ValidateAsync(createCandidateCommand);

        // Assert
        result.Errors[0].PropertyName.ShouldBe(nameof(createCandidateCommand.Name));
    }

    [Fact]
    public async Task Validate_NameIsNotUnique_ShouldReturnValidationError()
    {
        // Arrange
        var createCandidateCommand = _fixture.ValidCreateCandidateCommand with
        {
            Name = "Same Name"
        };
        var validator = new CreateCandidateCommandValidator(_fixture.MockCandidateRepo.Object);
        // Act
        var result = await validator.ValidateAsync(createCandidateCommand);

        // Assert
        result.Errors[0].PropertyName.ShouldBe(nameof(createCandidateCommand.Name));
    }
}