using System;

namespace JeFile.Dashboard.Features.Model;

[GenerateSerializer]
public class LineStatistics
{
    [Id(0)]
    public int PositionsInService { get; init; }
    [Id(1)]
    public int PositionsInLine { get; init; }
    [Id(2)]
    public int FinishedPositions { get; init; }
}
