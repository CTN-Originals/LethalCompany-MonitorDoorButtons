using BepInEx;
using BepInEx.Logging;
using HarmonyLib;



//_ Door open/close panel: Environment/HangarShip/AnimatedShipDoor/HangarDoorButtonPanel/
	//- StartButton
		//> Cube (2)
	//- StopButton 
		//> Cube (3)
//_ Monitor Camera buttons: Environment/HangarShip/ShipModels2b/MonitorWall/Cube.001/
	//- CameraMonitorOnButton 
		//> Cube (2)
	//- CameraMonitorSwitchButton 
		//> Cube (2)

namespace MonitorDoorButtons
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
		private static readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

		public static ManualLogSource Console = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_NAME);

		public static Plugin Instance { get; private set; }

        private void Awake()
        {
			if (Instance == null) Instance = this;

            // Plugin startup logic
            Console.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			harmony.PatchAll();
        }
    }
}
