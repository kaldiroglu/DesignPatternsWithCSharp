namespace dev.kaldiroglu.Mediator.Tests.Hw.BookingForm;

using dev.kaldiroglu.Mediator.Hw.BookingForm;
using Xunit;

/// <summary>Homework 3: the booking form decides, in one method, whether Book is enabled.</summary>
public class BookingFormTest
{
    /// <summary>Book is enabled only when a room and a date are chosen and the people fit.</summary>
    [Fact]
    public void BookNeedsAllThree()
    {
        BookingForm form = new BookingForm();
        form.ChooseRoom("Small");
        Assert.False(form.BookEnabled);
        form.ChooseDate("2026-10-12");
        Assert.False(form.BookEnabled);   // no attendees yet
        form.SetAttendees(4);
        Assert.True(form.BookEnabled);
        form.ClickBook();
        Assert.Equal(new[] { "Small on 2026-10-12 for 4" }, form.Booked);
    }

    /// <summary>Too many people for the room disables Book and warns, and a larger room fixes both.</summary>
    [Fact]
    public void TheRoomIsTooSmall()
    {
        BookingForm form = new BookingForm();
        form.ChooseDate("2026-10-12");
        form.SetAttendees(6);
        form.ChooseRoom("Small");
        Assert.False(form.BookEnabled);
        Assert.Equal("Small holds 4 people", form.Warning);
        form.ClickBook();
        Assert.Empty(form.Booked);
        form.ChooseRoom("Large");
        Assert.True(form.BookEnabled);
        Assert.Equal("", form.Warning);
    }
}
