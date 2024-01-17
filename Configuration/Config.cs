using BepInEx.Configuration;

namespace TalkingHeads.Configuration
{
    public class Config
    {
        public float ScaleSpeed => _scaleSpeed.Value;
        public float AmplitudeMultiplier => _amplitudeMultiplier.Value;

        private readonly ConfigEntry<float> _scaleSpeed;
        private readonly ConfigEntry<float> _amplitudeMultiplier;

        public Config(ConfigFile cfg)
        {
            _scaleSpeed = cfg.Bind<float>(
                "General",
                "ScaleSpeed",
                20,
                "Scale change rate"
            );

            _amplitudeMultiplier = cfg.Bind<float>(
                "General",
                "AmplitudeMultiplier",
                4,
                "Voice amplitude to scale multiplier"
            );
        }
    }
}
