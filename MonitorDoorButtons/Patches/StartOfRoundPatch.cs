using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using GameNetcodeStuff;
using System.ComponentModel;
using UnityEngine.EventSystems;

namespace MonitorDoorButtons.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	internal class StartOfRoundPatch {
		// The original objects as a reference
		private static Transform MonitorWall;
		private static Transform DoorPanel;
		private static InteractTrigger DoorStartButtonTrigger;
		private static InteractTrigger DoorStopButtonTrigger;
		private static TextMeshProUGUI DoorPanelMeter;

		// The duplicate objects that are created when the round starts
		public static Transform MonitorDoorPanel;
		private static Transform MonitorStartButton;
		private static Transform MonitorStopButton;
		private static InteractTrigger MonitorStartButtonTrigger;
		private static InteractTrigger MonitorStopButtonTrigger;
		private static TextMeshProUGUI MonitorDoorPanelMeter;


		[HarmonyPostfix]
		[HarmonyPatch("Start")]
		private static void StartPatch() {
			Plugin.Console.LogInfo($"StartOfRound.Start() called");
			GetObjectReferences();
			if (MonitorWall == null || DoorPanel == null) return;
			CreateMonitorDoorPanel();
		}

		[HarmonyPostfix]
		[HarmonyPatch("Update")]
		private static void UpdatePatch() {
			if (DoorPanelMeter == null || MonitorDoorPanelMeter == null) return;
			MonitorDoorPanelMeter.SetText(DoorPanelMeter.text);
		}

		//_ Monitor Camera buttons: Environment/HangarShip/ShipModels2b/MonitorWall/Cube.001/
			//- CameraMonitorOnButton
				//> Cube (2)
			//- CameraMonitorSwitchButton
				//> Cube (2)
		//_ Door open/close panel: Environment/HangarShip/AnimatedShipDoor/HangarDoorButtonPanel/
			//- StartButton
				//> Cube (2)
				//() AnimatedObjectTrigger.SetTrigger("OpenDoor")
			//- StopButton
				//> Cube (3)
				//() AnimatedObjectTrigger.SetTrigger("CloseDoor")
			//- ElevatorPanelScreen/Image/meter
				//() TextMeshProUGUI
		private static void GetObjectReferences() {
			MonitorWall = GameObject.Find("Environment/HangarShip/ShipModels2b/MonitorWall").transform;
			if (MonitorWall == null) {
				Plugin.Console.LogError($"StartOfRound.GetMonitorButton() could not find MonitorWall");
				return;
			}
			
			DoorPanel = GameObject.Find("Environment/HangarShip/AnimatedShipDoor/HangarDoorButtonPanel").transform;
			Transform DoorStartButton = DoorPanel.Find("StartButton").Find("Cube (2)");
			Transform DoorStopButton = DoorPanel.Find("StopButton").Find("Cube (3)");
			if (DoorPanel == null || DoorStartButton == null || DoorStopButton == null) {
				Plugin.Console.LogError($"StartOfRound.GetDoorPanel() could not find HangarDoorButtonPanel references");
				return;
			}
			DoorStartButtonTrigger = DoorStartButton.GetComponent<InteractTrigger>();
			DoorStopButtonTrigger = DoorStopButton.GetComponent<InteractTrigger>();
			DoorPanelMeter = DoorPanel.Find("ElevatorPanelScreen/Image/meter").GetComponent<TextMeshProUGUI>();

			Plugin.Console.LogInfo($"StartOfRound.GetDoorPanel() found HangarDoorButtonPanel: {DoorPanel.name}");
			Plugin.Console.LogInfo($"StartOfRound.GetMonitorButton() found MonitorWall: {MonitorWall.name}");
		}

		//| Transform
			//> LocalPosition (-0,2 -1,7 0,15)
			//> EulerAngles (90 90 0)
		private static void CreateMonitorDoorPanel() {
			if (MonitorWall.Find("MonitorDoorPanel") != null) {
				Plugin.Console.LogError($"StartOfRound.CreateMonitorDoorPanel() MonitorDoorPanel already exists");
				return;
			}
			MonitorDoorPanel = GameObject.Instantiate(DoorPanel, MonitorWall);
			MonitorDoorPanel.name = "MonitorDoorPanel";
			MonitorDoorPanel.localPosition = new Vector3(-0.2f, -1.7f, 0.15f);
			MonitorDoorPanel.localEulerAngles = new Vector3(90f, 90f, 0f);
			Plugin.Console.LogInfo($"StartOfRound.CreateMonitorDoorPanel() created MonitorDoorPanel: {MonitorDoorPanel.name}");

			MonitorStartButton = MonitorDoorPanel.Find("StartButton").Find("Cube (2)");
			MonitorStopButton = MonitorDoorPanel.Find("StopButton").Find("Cube (3)");
			if (MonitorStartButton == null || MonitorStopButton == null) {
				Plugin.Console.LogError($"StartOfRound.CreateMonitorDoorPanel() could not find MonitorDoorPanel references");
				return;
			}

			// InteractTrigger trigger = new InteractTrigger();
			// trigger.onInteract = CustomTrigger(MonitorStartButton, "Open");

			MonitorStartButtonTrigger = MonitorStartButton.GetComponent<InteractTrigger>();
			MonitorStartButtonTrigger.onInteract = DoorStartButtonTrigger.onInteract;
			// MonitorStartButtonTrigger.onInteract = CustomTrigger(DoorStartButtonTrigger, MonitorStartButton, "Open");
			
			MonitorStopButtonTrigger = MonitorStopButton.GetComponent<InteractTrigger>();
			MonitorStopButtonTrigger.onInteract = DoorStopButtonTrigger.onInteract;
			// MonitorStopButtonTrigger.onInteract = CustomTrigger(DoorStopButtonTrigger, MonitorStopButton, "Close");

			MonitorDoorPanelMeter = MonitorDoorPanel.Find("ElevatorPanelScreen/Image/meter").GetComponent<TextMeshProUGUI>();
		}

		private static InteractEvent CustomTrigger(InteractTrigger originalTrigger, Transform trigger, string state = "Open") {
			trigger.GetComponent<AnimatedObjectTrigger>().triggerAnimator.SetTrigger(state + "Door");

			Plugin.Console.LogMessage($"StartOfRound.CustomTrigger() called for {trigger.name} with state {state}");

			return originalTrigger.onInteract;
		}
	}
}