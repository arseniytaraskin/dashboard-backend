using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using Microsoft.AspNetCore.Mvc;

namespace JeFile.Dashboard.Controllers;

[ApiController]
[Route("api/bookingchannel")]
public class BookingChannelsController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;

    public BookingChannelsController(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetBookingChannels()
    {
        var grain = _grainFactory.GetGrain<IBookingChannelsGrain>(0);
        var data = await grain.GetBookingChannels();
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBookingChannels([FromBody] MonitoringLineModel model)
    {
        var grain = _grainFactory.GetGrain<IBookingChannelsGrain>(0);
        await grain.UpdateBookingChannels(model);
        return NoContent();
    }
}
