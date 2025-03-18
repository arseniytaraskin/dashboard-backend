using System;
using JeFile.Dashboard.Core.enums;

namespace JeFile.Dashboard.Core.Models;
[GenerateSerializer]
public class WidgetKey
{
    [Id(0)]
    public Guid LineId { get; set; } // Идентификатор линии

    [Id(1)]
    public WidgetType WidgetType { get; set; } // Тип виджета

    public WidgetKey(Guid lineId, WidgetType widgetType)
    {
        LineId = lineId;
        WidgetType = widgetType;
    }
}
