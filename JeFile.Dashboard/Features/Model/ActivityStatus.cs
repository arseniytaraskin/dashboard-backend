using System;
using JeFile.Dashboard.Core.Services;

namespace JeFile.Dashboard.Features.Model;

[GenerateSerializer]
public class ActivityStatus
{
    [Id(0)]
    public bool NotActive { get; init; }
    [Id(1)]
    public bool IsWorkingTime { get; init; }
    [Id(2)]
    public DashboardAttentionLevel AttentionLevel { get; init; }
}
