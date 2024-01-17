using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TalkingHeads.Configuration;

namespace TalkingHeads
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log => _instance.Logger;
        public static new Config Config => _instance._config;

        private static Plugin _instance;

        private readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);
        private Config _config;

        private void Awake()
        {
            _instance = this;
            _config = new Config(base.Config);
            _harmony.PatchAll();

            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
