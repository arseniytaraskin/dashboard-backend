using System;

namespace JeFile.Dashboard.Infrastructure.Data;
public class PriorityPositionsState
{
    public int TotalDayPrioriryPositions { get; set; }
    public int TotalDayNotPrioriryPositions { get; set; }
    public DateTime LastUpdated { get; set; }
}
