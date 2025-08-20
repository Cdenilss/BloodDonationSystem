using BloodDonationSystem.Application.Commands.DonorsCommand.Insert;
using BloodDonationSystem.Application.Commands.DonorsCommand.Put;
using BloodDonationSystem.Application.Services.ViaCep;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using Bogus;

namespace BloodDonationSystem.Tests.Shared.Fakers
{
    public static class DonorFakers
    {
        static DonorFakers()
        {
            Randomizer.Seed = new Random(2025);
        }


        public static readonly Faker<CreateDonorCommand> CreateDonorCommandValid =
            new Faker<CreateDonorCommand>("pt_BR")
                .RuleFor(x => x.Name, f => f.Person.FullName)
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.BirthDate, f => f.Date.Past(35, DateTime.Today.AddYears(-18)).Date)
                .RuleFor(x => x.Gender, f => f.PickRandom<GenderEnum>())
                .RuleFor(x => x.Weight, f => f.Random.Double(55, 110))
                .RuleFor(x => x.BloodType, f => f.PickRandom<BloodTypeEnum>())
                .RuleFor(x => x.RhFactor, f => f.PickRandom<RhFactorEnum>())
                .RuleFor(x => x.Cep, _ => "01001-000")
                .RuleFor(x => x.AddressNumber, f => f.Random.Int(1, 9999).ToString())
                .RuleFor(x => x.AddressComplement, f => f.Random.Bool() ? "Apto 45" : null);

        public static readonly Faker<ViaCepAddressResponse> ViaCepResponseValid =
            new Faker<ViaCepAddressResponse>("pt_BR")
                .RuleFor(x => x.Cep, _ => "01001-000")
                .RuleFor(x => x.Logradouro, _ => "Praça da Sé")
                .RuleFor(x => x.Bairro, _ => "Sé")
                .RuleFor(x => x.Localidade, _ => "São Paulo")
                .RuleFor(x => x.Uf, _ => "SP")
                .RuleFor(x => x.Erro, _ => false);

        public static readonly Faker<DonorPutCommand> DonorPutCommandValid =
            new Faker<DonorPutCommand>("pt_BR")
                .RuleFor(x => x.Name, f => f.Person.FullName)
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Cep, _ => "01001-000")
                .RuleFor(x => x.AddressNumber, f => f.Random.Int(1, 9999).ToString())
                .RuleFor(x => x.AddressComplement, f => f.Random.Bool() ? "Casa" : null);

        public static ViaCepAddressResponse InvalidViaCep() => null!;
    }
}