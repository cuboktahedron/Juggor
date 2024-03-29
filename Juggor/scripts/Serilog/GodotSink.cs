using System.Globalization;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Parsing;

namespace Juggor.Serilog;

public class GodotSink : ILogEventSink
{
    private readonly ITextFormatter formatter;

    public GodotSink(string outputTemplate, IFormatProvider? formatProvider)
    {
        formatter = new TemplateRenderer(outputTemplate, formatProvider);
    }

    public void Emit(LogEvent logEvent)
    {
        using TextWriter writer = new StringWriter();
        formatter.Format(logEvent, writer);
        writer.Flush();

        string color = logEvent.Level switch
        {
            LogEventLevel.Information => Colors.Cyan.ToHtml(),
            LogEventLevel.Warning => Colors.Yellow.ToHtml(),
            LogEventLevel.Error => Colors.Red.ToHtml(),
            LogEventLevel.Fatal => Colors.Purple.ToHtml(),
            _ => Colors.LightGray.ToHtml(),
        };

        foreach (string line in writer.ToString()?.Split('\n') ?? Array.Empty<string>())
        {
            GD.PrintRich($"[color=#{color}]{line}[/color]");
        }

        if (logEvent.Exception is null)
        {
            return;
        }

        if (logEvent.Level >= LogEventLevel.Error)
        {
            GD.PushError(logEvent.Exception);
        }
        else
        {
            GD.PushWarning(logEvent.Exception);
        }
    }

    private class TemplateRenderer : ITextFormatter
    {
        private readonly Renderer[] renderers;
        private readonly IFormatProvider? formatProvider;

        public TemplateRenderer(string outputTemplate, IFormatProvider? formatProvider)
        {
            this.formatProvider = formatProvider;

            MessageTemplate template = new MessageTemplateParser().Parse(outputTemplate);
            renderers = template.Tokens.Select(token =>
                token switch
                {
                    PropertyToken propertyToken => propertyToken.PropertyName switch
                    {
                        OutputProperties.MessagePropertyName
                            => (logEvent, output) => logEvent.RenderMessage(output, formatProvider),
                        OutputProperties.NewLinePropertyName
                            => (_, output) => output.Write('\n'),
                        OutputProperties.TimestampPropertyName
                            => RenderTimestamp(propertyToken.Format),
                        _
                            => RenderProperty(propertyToken.PropertyName, propertyToken.Format),
                    },
                    _ => null,
                })
                .OfType<Renderer>()
                .ToArray();
        }

        private delegate void Renderer(LogEvent logEvent, TextWriter output);

        public void Format(LogEvent logEvent, TextWriter output)
        {
            foreach (var renderer in renderers)
            {
                renderer.Invoke(logEvent, output);
            }
        }

        private Renderer RenderTimestamp(string? format)
        {
            Func<LogEvent, string> f =
                formatProvider?.GetFormat(typeof(ICustomFormatter)) is ICustomFormatter formatter
                    ? (logEvent) => formatter.Format(format, logEvent.Timestamp, formatProvider)
                    : (logEvent) => logEvent.Timestamp
                        .ToString(format, formatProvider ?? CultureInfo.InvariantCulture);

            return (logEvent, output) => output.Write(f(logEvent));
        }

        private Renderer RenderProperty(string propertyName, string? format)
        {
            return (LogEvent logEvent, TextWriter output) =>
            {
                if (logEvent.Properties.TryGetValue(propertyName, out var propertyValue))
                {
                    propertyValue.Render(output, format, formatProvider);
                }
            };
        }
    }
}
