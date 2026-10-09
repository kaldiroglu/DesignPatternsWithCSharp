using System.Reflection;
using dev.kaldiroglu.Strategy.Hw.LateFee;
using dev.kaldiroglu.Strategy.Hw.Seating;
using dev.kaldiroglu.Strategy.Hw.Validation;
using Xunit;

namespace dev.kaldiroglu.Strategy.Tests.Hw;

/// <summary>
/// Worked solutions for the three homework problems, so that every figure on the homework
/// slides is one a test asserts. Ported from the Java <c>hw.HomeworkTest</c>; its three
/// <c>@Nested</c> classes are nested classes here too.
/// </summary>
public class HomeworkTests
{
    private const BindingFlags AllDeclaredFields =
        BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public
        | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    /// <summary>1 - seating a booking.</summary>
    public class Seating
    {
        private readonly SeatPlan _cabin = SeatPlan.Empty(3, 4);   // 3 rows, A to D

        [Fact(DisplayName = "three policies, three different sets of seats for the same party")]
        public void ThreePolicies()
        {
            var desk = new BookingDesk(new FirstAvailable());
            Assert.Equal(["1A", "1B"], desk.Seat(_cabin, 2));

            desk.SetPolicy(new WindowPreferred());
            Assert.Equal(["1A", "1D"], desk.Seat(_cabin, 2));

            desk.SetPolicy(new KeepTogether());
            Assert.Equal(["1A", "1B"], desk.Seat(_cabin, 2));
        }

        [Fact(DisplayName = "a policy may legitimately fail where the others succeed")]
        public void KeepTogetherCanRefuse()
        {
            // Two free seats, but never two in the same row.
            var scattered = _cabin.WithTaken(
                ["1A", "1B", "1C", "2A", "2B", "2C", "3A", "3B", "3C", "3D"]);

            Assert.Equal(2, scattered.Free().Count);
            Assert.Equal(["1D", "2D"], new FirstAvailable().Allocate(scattered, 2));
            // An empty list is an answer, not an error.
            Assert.Empty(new KeepTogether().Allocate(scattered, 2));
        }

        [Fact(DisplayName = "window seats are the first and last letter of a row")]
        public void WindowsAreTheEdges()
        {
            Assert.True(_cabin.IsWindow("2A"));
            Assert.True(_cabin.IsWindow("2D"));
            Assert.False(_cabin.IsWindow("2B"));
        }
    }

    /// <summary>2 - what an overdue item costs.</summary>
    public class LateFees
    {
        private readonly Loan _tenDaysLate = new("Design Patterns", 10, 50);

        [Fact(DisplayName = "three rules, three charges for the same loan")]
        public void ThreeRules()
        {
            var desk = new ReturnsDesk(new StandardFee());
            Assert.Equal(500, desk.Charge(_tenDaysLate));

            desk.SetRule(new CappedFee(300));
            Assert.Equal(300, desk.Charge(_tenDaysLate));

            desk.SetRule(new GraceThenDouble(3));
            Assert.Equal(700, desk.Charge(_tenDaysLate));   // 7 chargeable days at double
        }

        [Fact(DisplayName = "the grace rule charges nothing inside the grace period")]
        public void InsideTheGrace()
        {
            IFeeRule rule = new GraceThenDouble(3);
            Assert.Equal(0, rule.Charge(new Loan("Refactoring", 3, 50)));
            Assert.Equal(100, rule.Charge(new Loan("Refactoring", 4, 50)));
        }

        [Fact(DisplayName = "a rule that is not a multiplier is why the interface is a method")]
        public void NotEveryRuleIsARate()
        {
            // Doubling after a grace period cannot be written as a rate per day.
            Assert.Equal(500, new StandardFee().Charge(_tenDaysLate));
            Assert.Equal(700, new GraceThenDouble(3).Charge(_tenDaysLate));

            var twoDays = new Loan("Design Patterns", 2, 50);
            Assert.Equal(100, new StandardFee().Charge(twoDays));
            Assert.Equal(0, new GraceThenDouble(3).Charge(twoDays));
        }

        [Fact(DisplayName = "an item returned early is a mistake, and says so")]
        public void NegativeDaysAreRejected()
        {
            Assert.Throws<ArgumentException>(() => new Loan("Design Patterns", -1, 50));
        }
    }

    /// <summary>3 - what a market requires of a passphrase.</summary>
    public class Validation
    {
        [Fact(DisplayName = "the market decides the rules, and that is configuration")]
        public void MarketsDiffer()
        {
            var relaxed = new SignUpForm(new MinimumLength(8));
            var strict = new SignUpForm(new MinimumLength(12), new MixedCharacters(),
                NoCommonWords.TheUsualSuspects());

            Assert.True(relaxed.Accepts("hunter2024"));
            Assert.False(strict.Accepts("hunter2024"));
            Assert.Equal(1, relaxed.RuleCount);
            Assert.Equal(3, strict.RuleCount);
        }

        [Fact(DisplayName = "every complaint is collected, not just the first")]
        public void AllComplaints()
        {
            var strict = new SignUpForm(new MinimumLength(12), new MixedCharacters(),
                NoCommonWords.TheUsualSuspects());

            var complaints = strict.Complaints("password");

            Assert.Equal(4, complaints.Count);
            Assert.Contains("shorter than 12 characters", complaints);
            Assert.Contains("no digits", complaints);
            Assert.Contains("no punctuation", complaints);
            Assert.Contains("contains a word from the banned list", complaints);
        }

        [Fact(DisplayName = "a good passphrase satisfies every rule at once")]
        public void OneThatPasses()
        {
            var strict = new SignUpForm(new MinimumLength(12), new MixedCharacters(),
                NoCommonWords.TheUsualSuspects());

            Assert.True(strict.Accepts("kedi-42-balkon!"));
            Assert.Empty(strict.Complaints("kedi-42-balkon!"));
        }

        [Fact(DisplayName = "and the question the exercise really asks")]
        public void IsAClassWorthIt()
        {
            // MinimumLength is one comparison. NoCommonWords carries data and will grow. Both
            // are strategies here; this test shows the difference in what they hold.
            IPassphraseRule trivial = new MinimumLength(8);
            IPassphraseRule substantial = NoCommonWords.TheUsualSuspects();

            Assert.Empty(trivial.Complaints("longenough"));
            Assert.Single(substantial.Complaints("my-password-1"));
            Assert.Single(trivial.GetType().GetFields(AllDeclaredFields));
            Assert.Single(substantial.GetType().GetFields(AllDeclaredFields));
        }
    }
}
