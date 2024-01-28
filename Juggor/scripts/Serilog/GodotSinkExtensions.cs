using Serilog;
using Serilog.Configuration;

namespace Juggor.Serilog;

public static class GodotSinkExtensions
{
    private const string DefaultGodotSinkOutputTemplate = "[{Timestamp:HH:mm:ss}] {Message:lj}";

    public static LoggerConfiguration Godot(
        this LoggerSinkConfiguration configuration,
        string outputTemplate = DefaultGodotSinkOutputTemplate,
        IFormatProvider? formatProvider = null)
    {
        return configuration.Sink(new GodotSink(outputTemplate, formatProvider));
    }
}
