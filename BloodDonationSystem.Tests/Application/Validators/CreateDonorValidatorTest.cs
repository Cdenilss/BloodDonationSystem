using BloodDonationSystem.Application.Commands.DonorsCommand.Insert;
using BloodDonationSystem.Application.Validators.DonorValidator;
using FluentAssertions;

namespace BloodDonationSystem.Tests.Application.Validators;

public class CreateDonorValidatorTests
{
    private readonly CreateDonorValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        var cmd = new CreateDonorCommand { Name = "", Email = "valid@mail.com", Weight = 70, Cep = "12345678" };
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Nome é obrigatório"));
    }

    [Fact]
    public void Should_Fail_When_Name_Too_Long()
    {
        var cmd = new CreateDonorCommand
        {
            Name = new string('A', 101),
            Email = "valid@mail.com",
            Weight = 70,
            Cep = "12345678"
        };
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Nome deve ter no máximo 100"));
    }

    [Fact]
    public void Should_Fail_When_Weight_Less_Or_Equal_50()
    {
        var cmd = new CreateDonorCommand { Name = "Carlos", Email = "valid@mail.com", Weight = 50, Cep = "12345678" };
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("peso mínimo"));
    }

    [Fact]
    public void Should_Fail_When_Email_Invalid()
    {
        var cmd = new CreateDonorCommand { Name = "Carlos", Email = "not-an-email", Weight = 70, Cep = "12345678" };
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("E-mail inválido"));
    }

    [Fact]
    public void Should_Fail_When_Cep_Empty_Or_Wrong_Length()
    {
        var cmd1 = new CreateDonorCommand { Name = "Carlos", Email = "valid@mail.com", Weight = 70, Cep = "" };
        var cmd2 = new CreateDonorCommand { Name = "Carlos", Email = "valid@mail.com", Weight = 70, Cep = "1234" };

        _validator.Validate(cmd1).IsValid.Should().BeFalse();
        _validator.Validate(cmd2).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_All_Valid()
    {
        var cmd = new CreateDonorCommand
        {
            Name = "Carlos",
            Email = "valid@mail.com",
            Weight = 70,
            Cep = "12345678"
        };

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeTrue();
    }
}