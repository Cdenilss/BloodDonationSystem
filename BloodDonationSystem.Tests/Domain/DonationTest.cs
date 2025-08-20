using BloodDonationSystem.Application.Validators.DonationValidator;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using BloodDonationSystem.Core.Repositories;
using FluentAssertions;
using Moq;

namespace BloodDonationSystem.Tests.Domain
{
    public class DonationTests
    {
        private static Donor CreateDonor(
            DateTime? birth = null,
            GenderEnum gender = GenderEnum.Male)
        {
            var address = new Address(
                street: "Rua X",
                city: "Cidade",
                state: "SP",
                zipCode: "01001000",
                number: "10",
                complement: null,
                district: "Centro");

            return new Donor(
                name: "Carlos",
                email: "carlos@mail.com",
                birthDate: birth ?? new DateTime(1990, 1, 1),
                gender: gender,
                weight: 70,
                bloodType: BloodTypeEnum.O,
                rhFactor: RhFactorEnum.Positive,
                address: address);
        }

        private static Donation CreateDonation(Guid donorId, DateTime date, int ml = 450)
            => new Donation(donorId, date, ml);

        [Fact(DisplayName = "Eligibility falha se doador for menor de idade")]
        public async Task Eligibility_Should_Fail_When_Donor_Is_Underage()
        {
            var repo = new Mock<IDonationRepository>();
            var validator = new DonationEligibilityValidator(repo.Object);

            var donor = CreateDonor(birth: DateTime.Today.AddYears(-17));
            var result = await validator.ValidateAsync(donor, DateTime.Today);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("menor de idade");
        }

        [Fact(DisplayName = "Eligibility aprova se for primeira doação")]
        public async Task Eligibility_Should_Pass_When_First_Donation()
        {
            var repo = new Mock<IDonationRepository>();
            repo.Setup(r => r.GetLastByDonorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Donation)null!);

            var validator = new DonationEligibilityValidator(repo.Object);
            var donor = CreateDonor();

            var result = await validator.ValidateAsync(donor, DateTime.Today);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact(DisplayName = "Eligibility reprova homem com menos de 60 dias da última doação")]
        public async Task Eligibility_Should_Fail_For_Male_Before_60_Days()
        {
            var lastDate = DateTime.Today.AddDays(-30);
            var repo = new Mock<IDonationRepository>();
            repo.Setup(r => r.GetLastByDonorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(CreateDonation(Guid.NewGuid(), lastDate));

            var validator = new DonationEligibilityValidator(repo.Object);
            var donor = CreateDonor(gender: GenderEnum.Male);

            var result = await validator.ValidateAsync(donor, DateTime.Today);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("Próxima doação possível");
        }

        [Fact(DisplayName = "Eligibility aprova homem com 60 dias ou mais")]
        public async Task Eligibility_Should_Pass_For_Male_With_60_Days_Or_More()
        {
            var lastDate = DateTime.Today.AddDays(-60);
            var repo = new Mock<IDonationRepository>();
            repo.Setup(r => r.GetLastByDonorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(CreateDonation(Guid.NewGuid(), lastDate));

            var validator = new DonationEligibilityValidator(repo.Object);
            var donor = CreateDonor(gender: GenderEnum.Male);

            var result = await validator.ValidateAsync(donor, DateTime.Today);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact(DisplayName = "Eligibility reprova mulher com menos de 90 dias da última doação")]
        public async Task Eligibility_Should_Fail_For_Female_Before_90_Days()
        {
            var lastDate = DateTime.Today.AddDays(-60);
            var repo = new Mock<IDonationRepository>();
            repo.Setup(r => r.GetLastByDonorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(CreateDonation(Guid.NewGuid(), lastDate));

            var validator = new DonationEligibilityValidator(repo.Object);
            var donor = CreateDonor(gender: GenderEnum.Female);

            var result = await validator.ValidateAsync(donor, DateTime.Today);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("Próxima doação possível");
        }

        [Fact(DisplayName = "Eligibility aprova mulher com 90 dias ou mais")]
        public async Task Eligibility_Should_Pass_For_Female_With_90_Days_Or_More()
        {
            var lastDate = DateTime.Today.AddDays(-90);
            var repo = new Mock<IDonationRepository>();
            repo.Setup(r => r.GetLastByDonorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(CreateDonation(Guid.NewGuid(), lastDate));

            var validator = new DonationEligibilityValidator(repo.Object);
            var donor = CreateDonor(gender: GenderEnum.Female);

            var result = await validator.ValidateAsync(donor, DateTime.Today);

            result.IsSuccess.Should().BeTrue();
        }


        [Fact(DisplayName = "CreateDonationValidator reprova e-mail inválido")]
        public void CreateDonationValidator_Should_Fail_For_Invalid_Email()
        {
            var v = new CreateDonationValidator();
            var cmd = new BloodDonationSystem.Application.Commands.DonationsCommand.Insert.CreateDonationCommand
            {
                DonorEmail = "not-an-email",
                DateDonation = DateTime.Today,
                QuantityMl = 450
            };

            var r = v.Validate(cmd);
            r.IsValid.Should().BeFalse();
            r.Errors.Should().Contain(e => e.ErrorMessage.Contains("E-mail inválido"));
        }

        [Fact(DisplayName = "CreateDonationValidator reprova data futura")]
        public void CreateDonationValidator_Should_Fail_For_Future_Date()
        {
            var v = new CreateDonationValidator();
            var cmd = new BloodDonationSystem.Application.Commands.DonationsCommand.Insert.CreateDonationCommand
            {
                DonorEmail = "ok@mail.com",
                DateDonation = DateTime.Today.AddDays(1),
                QuantityMl = 450
            };

            var r = v.Validate(cmd);
            r.IsValid.Should().BeFalse();
            r.Errors.Should().Contain(e => e.ErrorMessage.Contains("no futuro"));
        }

        [Fact(DisplayName = "CreateDonationValidator reprova quantidade fora do intervalo 420..473")]
        public void CreateDonationValidator_Should_Fail_For_Invalid_Quantity()
        {
            var v = new CreateDonationValidator();
            var cmdLow = new BloodDonationSystem.Application.Commands.DonationsCommand.Insert.CreateDonationCommand
            {
                DonorEmail = "ok@mail.com",
                DateDonation = DateTime.Today,
                QuantityMl = 419
            };
            var cmdHigh = new BloodDonationSystem.Application.Commands.DonationsCommand.Insert.CreateDonationCommand
            {
                DonorEmail = "ok@mail.com",
                DateDonation = DateTime.Today,
                QuantityMl = 474
            };

            v.Validate(cmdLow).IsValid.Should().BeFalse();
            v.Validate(cmdHigh).IsValid.Should().BeFalse();
        }

        [Fact(DisplayName = "CreateDonationValidator aprova caso válido")]
        public void CreateDonationValidator_Should_Pass_For_Valid_Command()
        {
            var v = new CreateDonationValidator();
            var cmd = new BloodDonationSystem.Application.Commands.DonationsCommand.Insert.CreateDonationCommand
            {
                DonorEmail = "ok@mail.com",
                DateDonation = DateTime.Today,
                QuantityMl = 450
            };

            v.Validate(cmd).IsValid.Should().BeTrue();
        }
    }
}