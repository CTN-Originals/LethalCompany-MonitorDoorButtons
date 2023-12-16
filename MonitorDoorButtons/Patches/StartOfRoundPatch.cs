using HarmonyLib;
using UnityEngine;



namespace MonitorDoorButtons.Patches 
{
	[HarmonyPatch(typeof(StartOfRound))]
	internal class StartOfRoundPatch {
		public static Transform MonitorDoorButtonStart; // The duplicated button that opens the door
		public static Transform MonitorDoorButtonStop; // The duplicated button that closes the door

		private Transform MonitorButton;
		private Transform DoorButtonStart;
		private Transform DoorButtonStop;

		[HarmonyPostfix]
		[HarmonyPatch("Start")]
		private static void StartPatch() {
			Plugin.Console.LogInfo($"StartOfRound.Start() called");
			GetMonitorButton();
			GetDoorButtons();
		}

		//_ Monitor Camera buttons: Environment/HangarShip/ShipModels2b/MonitorWall/Cube.001/
			//- CameraMonitorOnButton 
				//> Cube (2)
			//- CameraMonitorSwitchButton 
				//> Cube (2)
		private static void GetMonitorButton() {

		}

		//_ Door open/close panel: Environment/HangarShip/AnimatedShipDoor/HangarDoorButtonPanel/
			//- StartButton
				//> Cube (2)
			//- StopButton 
				//> Cube (3)
		private static void GetDoorButtons() {

		}
	}
}