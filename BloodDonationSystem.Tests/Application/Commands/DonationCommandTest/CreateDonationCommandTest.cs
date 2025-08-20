using BloodDonationSystem.Application.Commands.DonationsCommand.Insert;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Application.Validators.DonationValidator;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Tests.Shared.Fakers;
using FluentAssertions;
using Moq;

namespace BloodDonationSystem.Tests.Application.Commands.DonationCommandTest
{
    public class CreateDonationCommandHandlerTests
    {
        private static Donor MakeDonor(string email, BloodTypeEnum type = BloodTypeEnum.O,
            RhFactorEnum rh = RhFactorEnum.Positive)
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
                email: email,
                birthDate: new DateTime(1990, 1, 1),
                gender: GenderEnum.Male,
                weight: 72.5,
                bloodType: type,
                rhFactor: rh,
                address: address);
        }

        private static Mock<IDonationEligibilityValidator> EligOk()
        {
            var mock = new Mock<IDonationEligibilityValidator>();
            mock.Setup(v => v.ValidateAsync(It.IsAny<Donor>(), It.IsAny<DateTime>()))
                .ReturnsAsync(ResultViewModel.Success());
            return mock;
        }

        private static Mock<IDonationEligibilityValidator> EligFail(string message = "Inelegível")
        {
            var mock = new Mock<IDonationEligibilityValidator>();
            mock.Setup(v => v.ValidateAsync(It.IsAny<Donor>(), It.IsAny<DateTime>()))
                .ReturnsAsync(ResultViewModel.Error(message));
            return mock;
        }


        [Fact(DisplayName =
            "Happy path: donor encontrado, elegível e estoque existente → atualiza, adiciona doação e persiste")]
        public async Task Should_Update_Existing_Stock_Add_Donation_And_Commit()
        {
            // Arrange
            var cmd = DonationFakers.CreateDonationCommandValid.Generate();
            var donor = MakeDonor(cmd.DonorEmail, BloodTypeEnum.A, RhFactorEnum.Negative);

            var stock = new BloodStock(donor.BloodType, donor.RhFactor, quantityMl: 1000);

            var uow = new Mock<IUnitOfWork>();

            var donorsRepo = new Mock<IDonorRepository>();
            donorsRepo.Setup(r => r.GetDonorByEmail(cmd.DonorEmail)).ReturnsAsync(donor);

            var stocksRepo = new Mock<IBloodStockRepository>();
            stocksRepo.Setup(r => r.GetByTypeAsync(donor.BloodType, donor.RhFactor)).ReturnsAsync(stock);
            stocksRepo.Setup(r => r.UpdateAsync(stock));

            var donationsRepo = new Mock<IDonationRepository>();
            donationsRepo.Setup(r => r.Add(It.IsAny<Donation>())).Returns(Task.CompletedTask);

            uow.SetupGet(x => x.Donors).Returns(donorsRepo.Object);
            uow.SetupGet(x => x.BloodStocks).Returns(stocksRepo.Object);
            uow.SetupGet(x => x.Donations).Returns(donationsRepo.Object);
            uow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var elig = EligOk();

            var handler = new CreateDonationCommandHandler(elig.Object, uow.Object);

            var previousQty = stock.QuantityMl;

            // Act
            var result = await handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBe(Guid.Empty);

            stock.QuantityMl.Should().Be(previousQty + cmd.QuantityMl);

            stocksRepo.Verify(r => r.UpdateAsync(stock), Times.Once);
            stocksRepo.Verify(r => r.AddAsync(It.IsAny<BloodStock>()), Times.Never);

            donationsRepo.Verify(r => r.Add(It.Is<Donation>(d =>
                d.DonorId == donor.Id &&
                d.DateDonation == cmd.DateDonation &&
                d.QuantityMl == cmd.QuantityMl
            )), Times.Once);

            uow.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact(DisplayName =
            "Happy path: donor encontrado, elegível e estoque inexistente → cria, adiciona doação e persiste")]
        public async Task Should_Create_New_Stock_Add_Donation_And_Commit()
        {
            // Arrange
            var cmd = DonationFakers.CreateDonationCommandValid.Generate();
            var donor = MakeDonor(cmd.DonorEmail, BloodTypeEnum.B, RhFactorEnum.Positive);

            var uow = new Mock<IUnitOfWork>();

            var donorsRepo = new Mock<IDonorRepository>();
            donorsRepo.Setup(r => r.GetDonorByEmail(cmd.DonorEmail)).ReturnsAsync(donor);

            var stocksRepo = new Mock<IBloodStockRepository>();
            stocksRepo.Setup(r => r.GetByTypeAsync(donor.BloodType, donor.RhFactor)).ReturnsAsync((BloodStock)null!);
            stocksRepo.Setup(r => r.AddAsync(It.IsAny<BloodStock>())).Returns(Task.CompletedTask);

            var donationsRepo = new Mock<IDonationRepository>();
            donationsRepo.Setup(r => r.Add(It.IsAny<Donation>())).Returns(Task.CompletedTask);

            uow.SetupGet(x => x.Donors).Returns(donorsRepo.Object);
            uow.SetupGet(x => x.BloodStocks).Returns(stocksRepo.Object);
            uow.SetupGet(x => x.Donations).Returns(donationsRepo.Object);
            uow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var elig = EligOk();

            var handler = new CreateDonationCommandHandler(elig.Object, uow.Object);

            // Act
            var result = await handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBe(Guid.Empty);

            stocksRepo.Verify(r => r.AddAsync(It.Is<BloodStock>(s =>
                s.BloodType == donor.BloodType &&
                s.RhFactor == donor.RhFactor &&
                s.QuantityMl == cmd.QuantityMl
            )), Times.Once);

            stocksRepo.Verify(r => r.UpdateAsync(It.IsAny<BloodStock>()), Times.Never);

            donationsRepo.Verify(r => r.Add(It.Is<Donation>(d =>
                d.DonorId == donor.Id &&
                d.DateDonation == cmd.DateDonation &&
                d.QuantityMl == cmd.QuantityMl
            )), Times.Once);

            uow.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact(DisplayName = "Donor não encontrado → retorna erro e não persiste")]
        public async Task Should_Return_Error_When_Donor_Not_Found()
        {
            // Arrange
            var cmd = DonationFakers.CreateDonationCommandValid.Generate();

            var uow = new Mock<IUnitOfWork>();

            var donorsRepo = new Mock<IDonorRepository>();
            donorsRepo.Setup(r => r.GetDonorByEmail(cmd.DonorEmail)).ReturnsAsync((Donor)null!);

            uow.SetupGet(x => x.Donors).Returns(donorsRepo.Object);

            var elig = EligOk(); // nem será chamado

            var handler = new CreateDonationCommandHandler(elig.Object, uow.Object);

            // Act
            var result = await handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("não encontrado");

            uow.Verify(x => x.BloodStocks, Times.Never());
            uow.Verify(x => x.Donations, Times.Never());
            uow.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact(DisplayName = "Elegibilidade falha → retorna erro e não mexe em estoque/doações")]
        public async Task Should_Return_Error_When_Ineligible()
        {
            // Arrange
            var cmd = DonationFakers.CreateDonationCommandValid.Generate();
            var donor = MakeDonor(cmd.DonorEmail);
            var uow = new Mock<IUnitOfWork>();
            var donorsRepo = new Mock<IDonorRepository>();
            donorsRepo.Setup(r => r.GetDonorByEmail(cmd.DonorEmail)).ReturnsAsync(donor);
            uow.SetupGet(x => x.Donors).Returns(donorsRepo.Object);
            var elig = EligFail("Intervalo mínimo não atingido");
            var handler = new CreateDonationCommandHandler(elig.Object, uow.Object);
            // Act
            var result = await handler.Handle(cmd, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().NotBeNull();
            uow.Verify(x => x.BloodStocks, Times.Never());
            uow.Verify(x => x.Donations, Times.Never());
            uow.Verify(x => x.CompleteAsync(), Times.Never);
        }
    }
}