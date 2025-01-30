using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace ClinicalTrials.UnitTests;

public class AutoMoqTestDataAttribute : AutoDataAttribute
{
    private static Fixture FixtureFactory(int repeatCount)
    {
        var fixture = new Fixture
        {
            RepeatCount = repeatCount,
        };

        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));

        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });

        return fixture;
    }

    public AutoMoqTestDataAttribute(int repeatCount = 3) : base(() => FixtureFactory(repeatCount))
    {
    }
}
