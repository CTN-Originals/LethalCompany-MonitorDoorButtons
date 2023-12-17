using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MonitorDoorButtons
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
		private static readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

		public static ManualLogSource CLog = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_NAME);

		public static Plugin Instance { get; private set; }

        private void Awake()
        {
			if (Instance == null) Instance = this;

            // Plugin startup logic
            CLog.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			harmony.PatchAll();
        }
    }
}
