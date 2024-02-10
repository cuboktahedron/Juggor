namespace Juggor.Game;

public class EnvironmentSettings
{
    public static EnvironmentSettings Settings { get; set; } = new EnvironmentSettings();

    public float TempoRate { get; set; } = 1.0f;

    public float Tpm => 120 * TempoRate;

    public float GravityRate { get; set; } = 1.0f;

    public float Gravity => 980 * GravityRate;

    public Vector2 HandMovingScale { get; set; } = new Vector2(10, 10);

    public bool IsMirror { get; set; }

    public override string ToString()
    {
        var parameters = new string[]
        {
            $"TempoRate={TempoRate}",
            $"Tpm={Tpm}",
            $"GravityRate={GravityRate}",
            $"Gravity={Gravity}",
            $"IsMirror={IsMirror}",
        };

        return string.Join(", ", parameters);
    }
}
