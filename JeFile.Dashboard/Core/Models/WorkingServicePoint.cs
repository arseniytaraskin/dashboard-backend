using System;

namespace JeFile.Dashboard.Core.Models;

[GenerateSerializer]
public class WorkingServicePoint
{
    [Id(0)]
    public List<long> EnabledServices { get; set; } = new();
}
