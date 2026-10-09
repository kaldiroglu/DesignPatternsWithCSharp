using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Xunit;

namespace dev.kaldiroglu.Observer.Tests;

/// <summary>
/// The .NET rows of the Observer deck's known-uses table, checked against the running
/// runtime. The Java KnownUsesTests checks the JDK rows (PropertyChangeSupport, Flow,
/// ActionListener), which do not exist in .NET.
/// </summary>
public class KnownUsesTests
{
    // INotifyPropertyChanged tells a listener the name of the property that changed.
    [Fact]
    public void INotifyPropertyChangedNamesTheProperty()
    {
        INotifyPropertyChanged subject = new ObservableCollection<int>();
        var names = new List<string?>();
        subject.PropertyChanged += (_, e) => names.Add(e.PropertyName);

        ((ObservableCollection<int>)subject).Add(102);

        Assert.Contains("Count", names);
    }

    // ObservableCollection tells its listeners when an item is added or removed.
    [Fact]
    public void ObservableCollectionTellsAddAndRemove()
    {
        var prices = new ObservableCollection<int>();
        var actions = new List<NotifyCollectionChangedAction>();
        prices.CollectionChanged += (_, e) => actions.Add(e.Action);

        prices.Add(102);
        prices.Remove(102);

        Assert.Equal(new[] { NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Remove }, actions);
    }

    // IObservable<T> has one method, Subscribe, which takes an IObserver<T> and returns the
    // IDisposable that unsubscribes; IObserver<T> has OnNext, OnError and OnCompleted.
    [Fact]
    public void IObservableIsThePushInterface()
    {
        var subscribe = Assert.Single(typeof(IObservable<int>).GetMethods());
        Assert.Equal("Subscribe", subscribe.Name);
        Assert.Equal(typeof(IDisposable), subscribe.ReturnType);
        Assert.Equal(typeof(IObserver<int>), Assert.Single(subscribe.GetParameters()).ParameterType);

        Assert.Equal(new[] { "OnCompleted", "OnError", "OnNext" },
            typeof(IObserver<int>).GetMethods().Select(m => m.Name).OrderBy(n => n));
    }
}
