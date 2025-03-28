using System;

namespace JeFile.Dashboard.Features.Model;
[GenerateSerializer]
public class OutOfService
{
[Id(0)]
public int OutOfServicePositionsCount { get; init; }
}
