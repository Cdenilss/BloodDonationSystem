using BloodDonationSystem.Application.Commands.DonationsCommand.Insert;
using Bogus;

namespace BloodDonationSystem.Tests.Shared.Fakers
{
    public static class DonationFakers
    {
        static DonationFakers()
        {
            Randomizer.Seed = new Random(2025);
        }

        /// <summary>
        /// Gera um CreateDonationCommand válido (e-mail aleatório, data não-futura, 420..473 ml)
        /// </summary>
        public static readonly Faker<CreateDonationCommand> CreateDonationCommandValid =
            new Faker<CreateDonationCommand>("pt_BR")
                .RuleFor(x => x.DonorEmail, f => f.Internet.Email())
                .RuleFor(x => x.DateDonation, f => f.Date.Recent(15).Date) // hoje ou passado recente
                .RuleFor(x => x.QuantityMl, f => f.Random.Int(420, 473));
    }
}