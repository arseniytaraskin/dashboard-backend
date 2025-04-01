using System;

namespace JeFile.Dashboard.Features.Model;

[GenerateSerializer]
public class MobileAppStatus
{
    [Id(0)]
    public bool Available { get; init; }
}
