using Xunit;
using BatterySimulatorApp;
using FluentAssertions;

namespace BatterySimulator.Tests
{
    public class SimulatorTests
    {
        [Fact]
        public void Clamp_ShouldLimitDischargeToMax()
        {
            var model = new BatteryModel();
            var sim = new Simulator(model);
            var result = sim.Run(new double[] { 1.5 });
            result.Should().HaveCount(1);
            result[0].AppliedMW.Should().BeApproximately(1.0, 1e-6);
        }

        [Fact]
        public void Clamp_ShouldLimitChargeToMax()
        {
            var model = new BatteryModel();
            var sim = new Simulator(model);
            var result = sim.Run(new double[] { -2.0 });
            result.Should().HaveCount(1);
            result[0].AppliedMW.Should().BeApproximately(-1.0, 1e-6);
        }

        [Fact]
        public void SoC_ShouldNotGoBelowZero()
        {
            var model = new BatteryModel { SoC = 0.0 };
            var sim = new Simulator(model);
            var result = sim.Run(new double[] { 1.0 });
            result[0].SoC.Should().Be(0.0);
            result[0].AppliedMW.Should().Be(0.0);
        }

        [Fact]
        public void SoC_ShouldNotExceedOne()
        {
            var model = new BatteryModel { SoC = 1.0 };
            var sim = new Simulator(model);
            var result = sim.Run(new double[] { -1.0 });
            result[0].SoC.Should().Be(1.0);
            result[0].AppliedMW.Should().Be(0.0);
        }
    }
}