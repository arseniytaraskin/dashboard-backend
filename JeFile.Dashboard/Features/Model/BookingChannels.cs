using System;

namespace JeFile.Dashboard.Features.Model;
[GenerateSerializer]
public class BookingChannels
{
[Id(0)]
public int AppointmentPositions { get; init; }
[Id(1)]
public int MobileTodayPositions { get; init; }
[Id(2)]
public int ScreenTodayPositions { get; init; }
[Id(3)]
public int WebTodayPositions { get; init; }
[Id(5)] 

public int Total { get; set; }
}
