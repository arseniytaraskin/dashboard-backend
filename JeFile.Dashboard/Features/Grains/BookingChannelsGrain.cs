using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.Grains;

public class BookingChannelsGrain : Grain, IBookingChannelsGrain
{
private BookingChannels _bookingChannels = new();

public Task<BookingChannels> GetBookingChannels()
{
    return Task.FromResult(_bookingChannels);
}

public Task UpdateBookingChannels(MonitoringLineModel line)
{
    var appointments = 0;
    var mobileToday = 0;
    var screenToday = 0;
    var webToday = 0;

    foreach (var position in line.Positions.Concat(line.RemovedPositions))
    {
        var servicesCount = position.PersonsQuantity;
        if (position.Identity == MonitoringPositionIdentity.Appointment)
        {
            appointments += servicesCount;
            continue;
        }
        if (position.Identity == MonitoringPositionIdentity.Break)
            continue;
        if (position.Identity == MonitoringPositionIdentity.MobileApp)
            mobileToday += servicesCount;
        if (position.Identity == MonitoringPositionIdentity.ScreenCall)
            screenToday += servicesCount;
        if (position.Identity == MonitoringPositionIdentity.WebFrame)
            webToday += servicesCount;
    }

    _bookingChannels = new BookingChannels
    {
        AppointmentPositions = appointments,
        MobileTodayPositions = mobileToday,
        ScreenTodayPositions = screenToday,
        WebTodayPositions = webToday
    };

    return Task.CompletedTask;
}
}
