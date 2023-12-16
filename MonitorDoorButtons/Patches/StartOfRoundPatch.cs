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

		private static void GetMonitorButton() {

		}

		private static void GetDoorButtons() {

		}
	}
}