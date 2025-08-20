using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using FluentAssertions;

namespace BloodDonationSystem.Tests.Domain
{
    public class DonorTests
    {
        private static Donor CreateDonor(
            string name = "Antigo Nome",
            string email = "old@mail.com")
        {
            var address = new Address(
                street: "Rua A",
                city: "Cidade",
                state: "SP",
                zipCode: "01001000",
                number: "123",
                complement: null,
                district: "Centro");

            return new Donor(
                name: name,
                email: email,
                birthDate: new DateTime(1990, 1, 1),
                gender: GenderEnum.Male,
                weight: 72.5,
                bloodType: BloodTypeEnum.O,
                rhFactor: RhFactorEnum.Positive,
                address: address
            );
        }

        [Fact(DisplayName = "Update deve alterar nome e e-mail")]
        public void Update_Should_Change_Name_And_Email()
        {
            var donor = CreateDonor();

            donor.Update("Novo Nome", "new@mail.com");

            donor.Name.Should().Be("Novo Nome");
            donor.Email.Should().Be("new@mail.com");
        }

        [Fact(DisplayName = "Update não deve alterar outros campos")]
        public void Update_Should_Not_Change_Other_Fields()
        {
            var donor = CreateDonor();

            var before = new
            {
                donor.BirthDate,
                donor.Gender,
                donor.Weight,
                donor.BloodType,
                donor.RhFactor,
                donor.Address
            };

            donor.Update("Nome X", "x@mail.com");

            donor.BirthDate.Should().Be(before.BirthDate);
            donor.Gender.Should().Be(before.Gender);
            donor.Weight.Should().Be(before.Weight);
            donor.BloodType.Should().Be(before.BloodType);
            donor.RhFactor.Should().Be(before.RhFactor);
            donor.Address.Should().BeSameAs(before.Address);
        }
    }
}