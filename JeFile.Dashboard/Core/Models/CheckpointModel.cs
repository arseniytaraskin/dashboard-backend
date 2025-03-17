using System;

namespace JeFile.Dashboard.Core.Models;
[GenerateSerializer]
public class CheckpointModel
{
    [Id(0)]
    public int UpdatesCount { get; set; }
}
