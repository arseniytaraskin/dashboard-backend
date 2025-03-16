using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.InterfacesGrain;

public interface IBookingChannelsGrain : IGrainWithIntegerKey
{
    Task<BookingChannels> GetBookingChannels();
    Task UpdateBookingChannels(MonitoringLineModel line);
}
