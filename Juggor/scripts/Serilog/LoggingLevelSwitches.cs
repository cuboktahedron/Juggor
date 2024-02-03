using Serilog.Core;
using Serilog.Events;

namespace Juggor.Serilog;

public class LoggingLevelSwitches
{
    public static readonly LoggingLevelSwitch JuggorCoreLevelSwitch
        = new(LogEventLevel.Information);

    public static readonly LoggingLevelSwitch JuggorLevelSwitch
        = new(LogEventLevel.Information);
}
