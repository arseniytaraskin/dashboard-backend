using System;

namespace JeFile.Dashboard.Infrastructure;

public interface ILineRepository
{
    Task<LineMetadata> FindAsync(Guid lineId, CancellationToken cancellationToken);
}

public class LineMetadata
{
    public Guid Id { get; set; }
    public LineMode Mode { get; set; }
}

public class LineRepository : ILineRepository
{
    public Task<LineMetadata> FindAsync(Guid lineId, CancellationToken cancellationToken)
    {
        
        return Task.FromResult(new LineMetadata
        {
            Id = lineId,
            Mode = LineMode.Mobileapp | LineMode.Automated
        });
    }
}

[Flags]
public enum LineMode {
    None = 0,
    Mobileapp = 1,         // Доступно мобильное приложение
    Automated = 2,         // Автоматизированная линия
    ManualOverride = 4,    // Разрешено ручное управление
    Maintenance = 8,
}
