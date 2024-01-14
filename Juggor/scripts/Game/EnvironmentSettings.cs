

namespace Juggor.Game;

public class EnvironmentSettings
{
    public static EnvironmentSettings Settings { get; set; } = new EnvironmentSettings();

    public float TempoRate { get; set; } = 1.0f;

    public float Tpm => 60 * TempoRate;

    public float GravityRate { get; set; } = 1.0f;

    public float Gravity => 200 * GravityRate;

    public Vector2 HandMovingScale { get; set; } = new Vector2(15, 15);

    public override string ToString()
    {
        return $"TempoRate={TempoRate}, Tpm={Tpm}, GravityRate={GravityRate}, Gravity={Gravity}";
    }
}
