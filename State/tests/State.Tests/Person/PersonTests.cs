using Xunit;

namespace dev.kaldiroglu.State.Tests.Person;

using Person = global::dev.kaldiroglu.State.Person.Person;
using IEmotionalState = global::dev.kaldiroglu.State.Person.IEmotionalState;
using SadState = global::dev.kaldiroglu.State.Person.SadState;
using PersonMain = global::dev.kaldiroglu.State.Person.Main;

/// <summary>The person: the caller changes the emotional state, and the person forwards greetings.</summary>
public class PersonTests
{
    [Fact]
    public void MainPrintsBothStates()
    {
        Assert.Equal(new[]
        {
            "Hello in happy state: Hello, friend!",
            "Goodbye in happy state: Bye, friend!",
            "",
            "Hello in sad state: Hello. Sniff, sniff.",
            "Goodbye in sad state: Bye. Sniff, sniff."
        }, Printed.By(PersonMain.Run));
    }

    [Fact]
    public void ThePersonIsNotAState()
    {
        Assert.False(typeof(IEmotionalState).IsAssignableFrom(typeof(Person)));
    }

    [Fact]
    public void AGreetingDoesNotChangeTheState()
    {
        var person = new Person(new SadState());
        person.SayHello();
        Assert.Equal("Bye. Sniff, sniff.", person.SayGoodbye());
    }
}
