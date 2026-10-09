namespace dev.kaldiroglu.Observer.Tests.Gof
{
    using global::dev.kaldiroglu.Observer.Gof.Solution;
    using Xunit;
    using GofMain = global::dev.kaldiroglu.Observer.Gof.Main;
    using ProblemClockTimer = global::dev.kaldiroglu.Observer.Gof.Problem.ClockTimer;

    /// <summary>GoF's clock timer (Design Patterns, pp. 293-303), before and after the pattern.</summary>
    public class ClockTimerTests
    {
        // Before the pattern, the timer draws both of its clocks on every tick.
        [Fact]
        public void TheTimerDrawsItsClocks()
        {
            var timer = new ProblemClockTimer();
            timer.Tick();
            timer.Tick();

            Assert.Equal(new[]
            {
                "digital 00:00:01", "analog second hand at 6 degrees",
                "digital 00:00:02", "analog second hand at 12 degrees",
            }, timer.Screen);
        }

        // With the pattern, the clocks draw the same screen.
        [Fact]
        public void TheClocksDrawTheSameScreen()
        {
            var before = new ProblemClockTimer();
            var timer = new ClockTimer();
            var screen = new List<string>();
            new DigitalClock(timer, screen);
            new AnalogClock(timer, screen);

            for (int i = 0; i < 3; i++)
            {
                before.Tick();
                timer.Tick();
            }

            Assert.Equal(before.Screen, screen);
        }

        // After the analog clock is closed, on the third tick only the digital clock draws.
        [Fact]
        public void ClosingAClockDetachesIt()
        {
            var timer = new ClockTimer();
            var screen = new List<string>();
            new DigitalClock(timer, screen);
            var analog = new AnalogClock(timer, screen);
            Assert.Equal(2, timer.ObserverCount);

            timer.Tick();
            timer.Tick();
            analog.Close();
            timer.Tick();

            Assert.Equal(1, timer.ObserverCount);
            Assert.Equal("digital 00:00:03", screen[^1]);
            Assert.Equal(5, screen.Count);
        }

        // The clocks pull the time: the timer passes only itself.
        [Fact]
        public void ThePullModel()
        {
            var update = typeof(IObserver).GetMethod("Update", [typeof(Subject)]);
            Assert.NotNull(update);
            Assert.Equal(new[] { typeof(Subject) }, update!.GetParameters().Select(p => p.ParameterType));

            var timer = new ClockTimer();
            for (int i = 0; i < 3725; i++)
            {
                timer.Tick();
            }

            Assert.Equal(1, timer.Hour);
            Assert.Equal(2, timer.Minute);
            Assert.Equal(5, timer.Second);
        }

        // The timer knows nothing about clocks.
        [Fact]
        public void TheTimerKnowsNoClock()
        {
            for (Type? type = typeof(ClockTimer); type != null && type != typeof(object); type = type.BaseType)
            {
                var fieldTypes = Printed.DeclaredFieldTypes(type);
                Assert.DoesNotContain(typeof(DigitalClock), fieldTypes);
                Assert.DoesNotContain(typeof(AnalogClock), fieldTypes);
            }
        }

        // Main prints both screens and one observer left.
        [Fact]
        public void MainOutput()
        {
            var lines = Printed.By(GofMain.Run);

            Assert.Equal(new[]
            {
                "Timer draws its clocks: [digital 00:00:01, analog second hand at 6 degrees, "
                    + "digital 00:00:02, analog second hand at 12 degrees]",
                "Clocks observe the timer: [digital 00:00:01, analog second hand at 6 degrees, "
                    + "digital 00:00:02, analog second hand at 12 degrees, digital 00:00:03]",
                "Observers left: 1",
            }, lines);
        }
    }
}
