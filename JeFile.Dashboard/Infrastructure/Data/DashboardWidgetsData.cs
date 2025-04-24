using System;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Infrastructure.Data;

public class DashboardWidgetsData
{
public LineStatistics LineStatistics { get; set; }
public int CheckpointsUpdatesCount { get; set; }
public PriorityPositions PriorityPositions { get; set; } 

public MobileAppStatus MobileAppStatus { get; set; }

public NotArrivedPositions NotArrivedPositions { get; set; }
public OutOfService OutOfService { get; set; }

public DateTime LastUpdated { get; set; }
}
