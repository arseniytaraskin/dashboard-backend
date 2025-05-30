namespace JeFile.Dashboard.Core.enums;

public enum MonitoringPositionState
{
    ServiceStarted,  // В обслуживании
ManualFinished,  // Завершена вручную
AutoFinished,    // Завершена автоматически
Removed,         // Удалена
Leaved,          // Покинута
Unknown,
InLine, // Неизвестное состояние
CalledToCheckPoint,
MovingToCheckPoint,
NearCheckPoint

}
