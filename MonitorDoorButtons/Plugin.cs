using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MonitorDoorButtons
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
		private static readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

		public static ManualLogSource CLog;
		static bool betterMonitors;
		static bool biggerButtons;

		private void Awake() {

			//Config
			var configBetterMonitors = base.Config.Bind(
				"General",
				"ButtonsOnMonitor",
				false,
				"Moves the buttons onto the monitors above display controls and removes the back panel and power meter; required for GI bettermonitors compatability"
			);
			var configBiggerButtons = base.Config.Bind(
				"General",
				"BiggerButtons",
				false,
				"Makes the monitor buttons bigger, requires ButtonsOnMonitor to be true"
			);

			betterMonitors = configBetterMonitors.Value;
			biggerButtons = configBiggerButtons.Value;

			// Plugin startup logic
			CLog = Logger;
			CLog.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			harmony.PatchAll();
		}
		public static bool? GetConfig(int index){
			if (index==1) { return betterMonitors; }
			if (index==2) { return biggerButtons; }
			return null;
		}
		
	}
}
