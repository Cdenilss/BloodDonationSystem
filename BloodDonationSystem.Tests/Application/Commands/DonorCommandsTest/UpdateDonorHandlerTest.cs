using BloodDonationSystem.Application.Commands.DonorsCommand.Put;
using BloodDonationSystem.Application.Services.ViaCep;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Tests.Shared.Fakers;
using FluentAssertions;
using Moq;

namespace BloodDonationSystem.Tests.Application.Commands.DonorCommandsTest
{
    public class UpdateDonorHandlerTests
    {
        private static Donor MakeDonor(string name = "Antigo", string email = "old@mail.com")
        {
            var address = new Address(
                street: "Rua A", city: "Cidade", state: "SP",
                zipCode: "01001000", number: "1", complement: null, district: "Centro");

            return new Donor(
                name, email, new System.DateTime(1990, 1, 1),
                GenderEnum.Male, 72.5, BloodTypeEnum.O, RhFactorEnum.Positive, address);
        }

        [Fact(DisplayName = "Sucesso - atualiza nome/email; atualiza address se CEP fornecido")]
        public async Task Should_Update_Donor_And_Address_When_Cep_Informed()
        {
            var existing = MakeDonor();

            var viaCepResponse = DonorFakers.ViaCepResponseValid.Generate();
            var viaCep = new Mock<IViaCepService>();
            viaCep.Setup(v => v.GetAddressByCepAsync(It.IsAny<string>()))
                .ReturnsAsync(viaCepResponse);

            var donorRepo = new Mock<IDonorRepository>();
            donorRepo.Setup(r => r.GetDonorByEmail(existing.Email)).ReturnsAsync(existing);
            donorRepo.Setup(r => r.Update(existing)).Returns(Task.CompletedTask);

            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.Donors).Returns(donorRepo.Object);
            uow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var handler = new DonorPutCommandHandler(uow.Object, viaCep.Object);

            var cmd = DonorFakers.DonorPutCommandValid.Generate();
            // Garantir que estamos buscando pelo e-mail do existing:
            cmd.Email = existing.Email;

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            // Nome/email foram atualizados
            existing.Name.Should().Be(cmd.Name);
            existing.Email.Should().Be(cmd.Email);

            // Address foi atualizado com dados do ViaCEP (observe que handler passa CEP como veio)
            existing.Address.Street.Should().Be(viaCepResponse.Logradouro);
            existing.Address.City.Should().Be(viaCepResponse.Localidade);
            existing.Address.State.Should().Be(viaCepResponse.Uf);
            existing.Address.District.Should().Be(viaCepResponse.Bairro);
            existing.Address.Number.Should().Be(cmd.AddressNumber);
            existing.Address.Complement.Should().Be(cmd.AddressComplement);
            existing.Address.ZipCode.Should().Be(viaCepResponse.Cep); // aqui NÃO normaliza no handler

            donorRepo.Verify(r => r.Update(existing), Times.Once);
            uow.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact(DisplayName = "Sucesso - sem CEP informado atualiza apenas nome/email")]
        public async Task Should_Update_Only_Name_And_Email_When_No_Cep()
        {
            var existing = MakeDonor();

            var viaCep = new Mock<IViaCepService>(); // não deve ser chamado

            var donorRepo = new Mock<IDonorRepository>();
            donorRepo.Setup(r => r.GetDonorByEmail(existing.Email)).ReturnsAsync(existing);
            donorRepo.Setup(r => r.Update(existing)).Returns(Task.CompletedTask);

            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.Donors).Returns(donorRepo.Object);
            uow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var handler = new DonorPutCommandHandler(uow.Object, viaCep.Object);

            var cmd = DonorFakers.DonorPutCommandValid.Generate();
            cmd.Email = existing.Email;
            cmd.Cep = "";

            var oldAddress = existing.Address;

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            existing.Name.Should().Be(cmd.Name);
            existing.Email.Should().Be(cmd.Email);
            existing.Address.Should().BeSameAs(oldAddress);

            viaCep.Verify(v => v.GetAddressByCepAsync(It.IsAny<string>()), Times.Never);
            donorRepo.Verify(r => r.Update(existing), Times.Once);
            uow.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact(DisplayName = "Doador inexistente - retorna erro")]
        public async Task Should_Return_Error_When_Donor_Not_Found()
        {
            var viaCep = new Mock<IViaCepService>();
            var donorRepo = new Mock<IDonorRepository>();
            donorRepo.Setup(r => r.GetDonorByEmail(It.IsAny<string>()))
                .ReturnsAsync((Donor)null!);

            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.Donors).Returns(donorRepo.Object);

            var handler = new DonorPutCommandHandler(uow.Object, viaCep.Object);
            var cmd = DonorFakers.DonorPutCommandValid.Generate();

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("não encontrado");
            donorRepo.Verify(r => r.Update(It.IsAny<Donor>()), Times.Never);
            uow.Verify(x => x.CompleteAsync(), Times.Never);
        }
    }
}