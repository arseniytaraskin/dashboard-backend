using System;

namespace JeFile.Dashboard.Features.Model;
[GenerateSerializer]
public class NotArrivedPositions
{
    [Id(0)]
    public int NotArrivedAppointments { get; init; }

    [Id(1)]
    public int NotArrivedTodayPositions { get; init; }

    [Id(2)]
    public int NotArrivedCalledPositions { get; init; }

    [Id(3)]
    public int FinishedAndInServicePositions { get; init; }

    //[Id(4)]
    public int Total => NotArrivedAppointments + NotArrivedTodayPositions + NotArrivedCalledPositions + FinishedAndInServicePositions;
}
