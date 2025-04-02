using System;

namespace JeFile.Dashboard.Features.Model;

[GenerateSerializer]
public class PriorityPositions
{
    [Id(0)]
    public int TotalDayPrioriryPositions { get; init; }

    [Id(1)]
    public int TotalDayNotPrioriryPositions { get; init; }
}
