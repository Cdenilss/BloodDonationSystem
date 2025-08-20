using BloodDonationSystem.Application.Commands.BloodStockPutCommand.OutPut;
using BloodDonationSystem.Core.Common.Events.BloodStockEvents;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using BloodDonationSystem.Core.Repositories;
using FluentAssertions;
using Moq;

namespace BloodDonationSystem.Tests.Application.Commands.BloodStockCommandTest
{
    public class OutputBloodStockCommandHandlerTests
    {
        [Fact(DisplayName = "Baixa estoque e persiste")]
        public async Task Should_Draw_And_Persist()
        {
            var type = BloodTypeEnum.O;
            var rh = RhFactorEnum.Negative;
            var stock = new BloodStock(type, rh, 1000) { MinimumSafeQuantity = 300 };

            var repo = new Mock<IBloodStockRepository>();
            repo.Setup(r => r.GetByTypeAsync(type, rh)).ReturnsAsync(stock);

            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.BloodStocks).Returns(repo.Object);
            uow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var handler = new OutputBloodStockCommandHandler(uow.Object);
            var cmd = new OutputBloodStockCommand { BloodType = type, RhFactor = rh, QuantityMl = 200 };

            await handler.Handle(cmd, default);

            stock.QuantityMl.Should().Be(800);
            uow.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact(DisplayName = "Ao cruzar mínimo, domínio enfileira evento")]
        public async Task Should_Raise_Event_When_Cross_Minimum()
        {
            var type = BloodTypeEnum.A;
            var rh = RhFactorEnum.Positive;
            var stock = new BloodStock(type, rh, 350) { MinimumSafeQuantity = 300 };

            var repo = new Mock<IBloodStockRepository>();
            repo.Setup(r => r.GetByTypeAsync(type, rh)).ReturnsAsync(stock);

            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.BloodStocks).Returns(repo.Object);
            uow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var handler = new OutputBloodStockCommandHandler(uow.Object);
            var cmd = new OutputBloodStockCommand { BloodType = type, RhFactor = rh, QuantityMl = 100 };

            await handler.Handle(cmd, default);

            stock.QuantityMl.Should().Be(250);
            stock.DomainEvents.OfType<BloodStockBecameLowEvent>().Should().HaveCount(1);
        }

        [Fact(DisplayName = "Estoque não encontrado - lança/retorna erro")]
        public async Task Should_Throw_When_Stock_Not_Found()
        {
            var type = BloodTypeEnum.B;
            var rh = RhFactorEnum.Negative;

            var repo = new Mock<IBloodStockRepository>();
            repo.Setup(r => r.GetByTypeAsync(type, rh))
                .ReturnsAsync((BloodDonationSystem.Core.Entities.BloodStock)null!);

            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(x => x.BloodStocks).Returns(repo.Object);

            var handler = new OutputBloodStockCommandHandler(uow.Object);
            var cmd = new OutputBloodStockCommand { BloodType = type, RhFactor = rh, QuantityMl = 100 };

            Func<Task> act = async () => await handler.Handle(cmd, default);
            await act.Should()
                .ThrowAsync<InvalidOperationException>(); // ajuste se seu handler retorna Result em vez de lançar
        }
    }
}