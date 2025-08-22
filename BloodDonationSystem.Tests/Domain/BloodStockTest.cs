using BloodDonationSystem.Core.Common.Events.BloodStockEvents;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using FluentAssertions;


namespace BloodDonationSystem.Tests.Domain
{
    public class BloodStockTests
    {
        private static BloodStock Create(BloodTypeEnum t, RhFactorEnum rh, int qty, int min)
        {
            var s = new BloodStock(t, rh, qty);
            s.MinimumSafeQuantity = min;
            return s;
        }

        [Fact(DisplayName = "Draw lança ArgumentException quando ml <= 0")]
        public void Draw_Should_Throw_When_Ml_Is_Zero_Or_Negative()
        {
            var stock = Create(BloodTypeEnum.O, RhFactorEnum.Positive, qty: 1000, min: 300);

            Action zero = () => stock.Draw(0);
            Action negative = () => stock.Draw(-10);

            zero.Should().Throw<ArgumentException>()
                .WithMessage("*maior que zero*")
                .And.ParamName.Should().Be("ml");

            negative.Should().Throw<ArgumentException>()
                .WithMessage("*maior que zero*")
                .And.ParamName.Should().Be("ml");

            stock.QuantityMl.Should().Be(1000);
            stock.DomainEvents.Should().BeEmpty();
        }

        [Fact(DisplayName = "Draw lança InvalidOperationException quando ml > disponível")]
        public void Draw_Should_Throw_When_Ml_Exceeds_Available()
        {
            var stock = Create(BloodTypeEnum.A, RhFactorEnum.Negative, qty: 150, min: 80);

            Action act = () => stock.Draw(200);

            act.Should().Throw<InvalidOperationException>();

            stock.QuantityMl.Should().Be(150);
            stock.DomainEvents.Should().BeEmpty();
        }

        [Fact(DisplayName = "Draw válido reduz quantidade e não dispara evento se acima do mínimo")]
        public void Draw_Should_Decrease_And_Not_Raise_When_Above_Minimum()
        {
            var stock = Create(BloodTypeEnum.B, RhFactorEnum.Positive, qty: 1000, min: 300);

            stock.Draw(200);

            stock.QuantityMl.Should().Be(800);
            stock.DomainEvents.Should().BeEmpty();
        }

        [Fact(DisplayName = "Draw válido reduz quantidade e dispara evento ao cruzar o mínimo")]
        public void Draw_Should_Raise_Event_When_Cross_Minimum()
        {
            var stock = Create(BloodTypeEnum.AB, RhFactorEnum.Negative, qty: 350, min: 300);

            stock.Draw(60);

            stock.QuantityMl.Should().Be(290);

            var evt = stock.DomainEvents.OfType<BloodStockBecameLowEvent>().SingleOrDefault();
            evt.Should().NotBeNull();
            evt.BloodType.Should().Be(BloodTypeEnum.AB);
            evt.RhFactor.Should().Be(RhFactorEnum.Negative);
            evt.MinimumSafeQuantity.Should().Be(300);
        }
    }
}