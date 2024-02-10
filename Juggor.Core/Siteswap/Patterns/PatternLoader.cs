using System.Numerics;
using Juggor.Core.Process;
using Juggor.Core.Style;

namespace Juggor.Core.Siteswap.Patterns;

public class PatternLoader
{
    private readonly Dictionary<string, ThrowStyle> styles = new();
    private int lineNumber = 0;

    public ProcessResult<IList<IPatternsElement>, string> Load(StreamReader sr)
    {
        var patterns = new List<IPatternsElement>();
        var juggleParameter = new JuggleParameter();

        var lines = sr.ReadToEnd().Replace("\r\n", "\n").Replace("\r", "\n").Split("\n");
        lines = lines.Select(line =>
        {
            int commentIndex = line.IndexOf(';');
            if (commentIndex >= 0)
            {
                line = line[..commentIndex];
            }

            return line.Trim();
        }).ToArray();

        // ; The first character on the line means...
        // ; ';' Comment.
        // ; '#' Set the parameter.
        // ; '/' Separator(Green color).
        // ; '%' Name of the style(30 characters or less) with style data,
        // ; or call the registered style.
        var group = new PatternsGroup("default");
        ThrowStyle throwStyle = ThrowStyle.Normal;

        while (lineNumber < lines.Length)
        {
            lineNumber++;
            string line = lines[lineNumber - 1];

            if (line.Length == 0)
            {
                continue;
            }

            if (line[0] == '/')
            {
                if (line.Length == 1)
                {
                    continue;
                }

                line = line[1..]
                    .Replace("[", string.Empty)
                    .Replace("]", string.Empty)
                    .Trim();

                if (group != null && group.Elements.Any())
                {
                    patterns.Add(group);
                }

                group = new PatternsGroup(line);
            }
            else if (line[0] == '#')
            {
                var result = LoadJuggleParameter(line[1..], ref juggleParameter);
                if (!result.IsSucceeded)
                {
                    return ProcessResult<IList<IPatternsElement>, string>.Error(result.ErrorValue);
                }
            }
            else if (line[0] == '%')
            {
                var result = LoadThrowStyle(lines);
                if (result.IsSucceeded)
                {
                    throwStyle = result.SuccessValue;
                    styles[throwStyle.Name] = throwStyle;
                }
                else
                {
                    return ProcessResult<IList<IPatternsElement>, string>.Error(result.ErrorValue);
                }
            }
            else
            {
                string[] itemElements =
                    line.Split(new char[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
                string label = string.Empty;

                if (itemElements.Length > 1)
                {
                    label = itemElements[1];
                }

                if (Siteswap.TryParse(new SiteswapParseContext(itemElements[0]), out Siteswap? ss))
                {
                    if (!ss!.IsValid())
                    {
                        return ProcessResult<IList<IPatternsElement>, string>.Error(
                            LineMessage($"\"{itemElements[0]}\" is not jugglable."));
                    }

                    group.Add(new PatternsItem(label, ss, throwStyle, juggleParameter));
                }
                else
                {
                    return ProcessResult<IList<IPatternsElement>, string>.Error(
                        LineMessage($"\"{itemElements[0]}\" is invalid."));
                }
            }
        }

        if (group != null && group.Elements.Any())
        {
            patterns.Add(group);
        }

        return ProcessResult<IList<IPatternsElement>, string>.Success(patterns);
    }

    private ProcessVoidResult<string> LoadJuggleParameter(string line, ref JuggleParameter juggleParameter)
    {
        string[] elems = line.Split('=', 2, StringSplitOptions.TrimEntries);
        if (elems.Length != 2)
        {
            return ProcessVoidResult<string>.Error(
                LineMessage($"JuggleParameter must be set in the form Name=Value."));
        }

        if (elems[0] == "GA")
        {
            if (float.TryParse(elems[1], out float ga))
            {
                ga = Math.Min(98f, Math.Max(0.1f, ga));
                juggleParameter.GravityRate = ga / 9.8f;
            }
            else
            {
                return ProcessVoidResult<string>.Error(
                    LineMessage($"L{lineNumber}: \"GA\" parameter must be a number"));
            }
        }
        else if (elems[0] == "SP")
        {
            if (float.TryParse(elems[1], out float sp))
            {
                sp = Math.Min(3.0f, Math.Max(0.1f, sp));
                juggleParameter.TempoRate = sp;
            }
            else
            {
                return ProcessVoidResult<string>.Error(
                    LineMessage($"\"SP\" parameter must be a number"));
            }
        }

        return ProcessVoidResult<string>.Success();
    }

    private ProcessResult<ThrowStyle, string> LoadThrowStyle(string[] lines)
    {
        string line = lines[lineNumber - 1];
        string styleName = line[1..];

        if (string.IsNullOrEmpty(styleName))
        {
            return ProcessResult<ThrowStyle, string>.Error(
                LineMessage($"Style name is required."));
        }

        if (styles.TryGetValue(styleName, out ThrowStyle? throwStyle))
        {
            return ProcessResult<ThrowStyle, string>.Success(throwStyle);
        }

        throwStyle = new ThrowStyle(styleName);

        while (lineNumber < lines.Length)
        {
            lineNumber++;
            line = lines[lineNumber - 1];

            if (line.Length == 0)
            {
                continue;
            }

            if (line[0] != '{')
            {
                if (throwStyle.ThrowCatchPoints.Any())
                {
                    lineNumber--;
                    return ProcessResult<ThrowStyle, string>.Success(throwStyle);
                }
                else
                {
                    return ProcessResult<ThrowStyle, string>.Error(
                        LineMessage($"At least one catch/throw position is required."));
                }
            }

            var result = LoadCatchThrowPosition(line);
            if (result.IsError)
            {
                return ProcessResult<ThrowStyle, string>.Error(result.ErrorValue);
            }

            var (catchPos, throwPos) = result.SuccessValue;
            throwStyle.Add(catchPos, throwPos);
        }

        lineNumber--;
        return ProcessResult<ThrowStyle, string>.Success(throwStyle);
    }

    private ProcessResult<(Vector2 CatchPos, Vector2 ThrowPos), string> LoadCatchThrowPosition(
        string line)
    {
        int p1 = line.IndexOf('{');
        int p2 = line.IndexOf('}');

        if (p1 == -1 || p2 == -1)
        {
            return ProcessResult<(Vector2, Vector2), string>.Error(
                LineMessage($"Catch position is invalid."));
        }

        Vector2 catchPos = LoadPosition(line[(p1 + 1)..p2]);
        line = line[(p2 + 1)..];

        p1 = line.IndexOf('{');
        p2 = line.IndexOf('}');

        if (p1 == -1 || p2 == -1)
        {
            return ProcessResult<(Vector2, Vector2), string>.Error(
                LineMessage($"Throw position is invalid."));
        }

        Vector2 throwPos = LoadPosition(line[(p1 + 1)..p2]);

        return ProcessResult<(Vector2, Vector2), string>.Success((catchPos, throwPos));
    }

    private Vector2 LoadPosition(string line)
    {
        var elements = line.Split(",", StringSplitOptions.TrimEntries);
        var pos = elements.Select(element =>
        {
            if (int.TryParse(element, out int value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        }).ToList();

        if (pos.Count < 2)
        {
            return default;
        }
        else
        {
            return new Vector2(pos[0], pos[1]);
        }
    }

    private string LineMessage(string message)
    {
        return $"L{lineNumber}: {message}";
    }
}
