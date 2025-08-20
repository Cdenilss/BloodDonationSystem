using BloodDonationSystem.Application.Commands.DonorsCommand.Insert;
using BloodDonationSystem.Application.Services.ViaCep;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Tests.Shared.Fakers;
using FluentAssertions;
using Moq;

namespace BloodDonationSystem.Tests.Application.Commands.DonorCommandsTest
{
    public class CreateDonorHandlerTests
    {
        [Fact(DisplayName = "Happy path - cria Donor, mapeia Address e persiste")]
        public async Task Should_Create_Donor_With_Mapped_Address_And_Persist()
        {
            var viaCepResponse = DonorFakers.ViaCepResponseValid.Generate();

            var viaCep = new Mock<IViaCepService>();
            viaCep.Setup(v => v.GetAddressByCepAsync(It.IsAny<string>()))
                .ReturnsAsync(viaCepResponse);

            var donorRepo = new Mock<IDonorRepository>();
            donorRepo.Setup(r => r.Add(It.IsAny<Donor>())).Returns(Task.CompletedTask);

            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.Donors).Returns(donorRepo.Object);
            uow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var handler = new CreateDonorHandler(uow.Object, viaCep.Object);
            var cmd = DonorFakers.CreateDonorCommandValid.Generate();

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBe(Guid.Empty);

            donorRepo.Verify(r => r.Add(It.Is<Donor>(d =>
                d.Name == cmd.Name &&
                d.Email == cmd.Email &&
                d.Address.ZipCode == "01001000"
            )), Times.Once);

            uow.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact(DisplayName = "ViaCep inválido - retorna erro e não persiste")]
        public async Task Should_Return_Error_When_ViaCep_Invalid()
        {
            var viaCep = new Mock<IViaCepService>();
            viaCep.Setup(v => v.GetAddressByCepAsync(It.IsAny<string>()))
                .ReturnsAsync(DonorFakers.InvalidViaCep());

            var donorRepo = new Mock<IDonorRepository>();
            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.Donors).Returns(donorRepo.Object);

            var handler = new CreateDonorHandler(uow.Object, viaCep.Object);
            var cmd = DonorFakers.CreateDonorCommandValid.Generate();

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            donorRepo.Verify(r => r.Add(It.IsAny<Donor>()), Times.Never);
            uow.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact(DisplayName = "CEP com hífen deve ser normalizado no Address")]
        public async Task Should_Normalize_ZipCode_On_Address()
        {
            var viaCepResponse = DonorFakers.ViaCepResponseValid.Generate();

            var viaCep = new Mock<IViaCepService>();
            viaCep.Setup(v => v.GetAddressByCepAsync(It.IsAny<string>()))
                .ReturnsAsync(viaCepResponse);

            var donorRepo = new Mock<IDonorRepository>();
            donorRepo.Setup(r => r.Add(It.IsAny<Donor>())).Returns(Task.CompletedTask);

            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.Donors).Returns(donorRepo.Object);
            uow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var handler = new CreateDonorHandler(uow.Object, viaCep.Object);
            var cmd = DonorFakers.CreateDonorCommandValid.Generate();

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            donorRepo.Verify(r => r.Add(It.Is<Donor>(d =>
                d.Address.ZipCode == "01001000"
            )), Times.Once);
        }
    }
}