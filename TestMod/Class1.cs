using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestMod
{
	public class Class1 : ModBehaviour
	{
		private FluidDetector _fluidDetector;
		private SaveFile _saveFile;
		private bool _isSolarSystemLoaded;
		private const string SAVE_FILE = "savefile.json";
		private bool _suitUpOnTravel = true;
		public IMenuAPI MenuApi;

		public void Start()
		{
			MenuApi = ModHelper.Interaction.GetModApi<IMenuAPI>("_nebula.MenuFramework");
			ModHelper.Events.Subscribe<Flashlight>(Events.AfterStart);
			ModHelper.Events.Event += OnEvent;

			_saveFile = ModHelper.Storage.Load<SaveFile>(SAVE_FILE);

			LoadManager.OnCompleteSceneLoad += OnSceneLoaded;

			MakeTitleMenus();
		}

		public override void Configure(IModConfig config) 
			=> _suitUpOnTravel = config.GetSettingsValue<bool>("suitUpOnTravel");

		private void OnSceneLoaded(OWScene originalScene, OWScene scene)
		{
			if (scene == OWScene.SolarSystem || scene == OWScene.EyeOfTheUniverse)
			{
				_isSolarSystemLoaded = true;
				SpawnAtInitialPoint();
			}
			else if (scene == OWScene.TitleScreen)
			{
				MakeTitleMenus();
			}
		}

		private void OnEvent(MonoBehaviour behaviour, Events ev)
		{
			if (behaviour.GetType() == typeof(Flashlight) && ev == Events.AfterStart)
			{
				Init();
				SpawnAtInitialPoint();
			}
		}

		private void MakeTitleMenus()
		{
			var twoChoicePopup = MenuApi.MakeTwoChoicePopup("This is a two-choice popup.", "Confirm option", "Cancel option");
			var inputPopup = MenuApi.MakeInputFieldPopup("This is a input field popup.", "Placeholder message", "Confirm option", "Cancel option");
			inputPopup.GetComponent<PopupInputMenu>().OnPopupConfirm += () => ModHelper.Console.WriteLine(inputPopup.GetComponent<PopupInputMenu>().GetInputText());

			MenuApi.TitleScreen_MakeMenuOpenButton("TWO CHOICE", twoChoicePopup.GetComponent<Menu>());
			MenuApi.TitleScreen_MakeMenuOpenButton("INPUT", inputPopup.GetComponent<Menu>());
			MenuApi.TitleScreen_MakeSceneLoadButton("LOAD EYE (CONFIRM)", SubmitActionLoadScene.LoadableScenes.EYE, twoChoicePopup.GetComponent<PopupMenu>());
			MenuApi.TitleScreen_MakeSceneLoadButton("LOAD EYE", SubmitActionLoadScene.LoadableScenes.EYE);

			var menu = MenuApi.OptionsMenu_MakeNonScrollingOptionsTab("TEST");
			MenuApi.OptionsMenu_MakeTwoButtonToggle("TEST", "TRUE", "FALSE", "TOOPTIP", menu);
		}

		private void Init()
		{
			_fluidDetector = Locator.GetPlayerCamera().GetComponentInChildren<FluidDetector>();

			var pauseButton = MenuApi.PauseMenu_MakeSimpleButton("TELEPORT TO...");

			var twoChoicePopup = MenuApi.MakeTwoChoicePopup("This is a two-choice popup.", "Confirm option", "Cancel option");
			MenuApi.PauseMenu_MakeMenuOpenButton("TWO CHOICE", twoChoicePopup.GetComponent<Menu>());

			var shipSpawnMenu = MenuApi.PauseMenu_MakePauseListMenu("Ship Spawn Points");
			var playerSpawnMenu = MenuApi.PauseMenu_MakePauseListMenu("Player Spawn Points");

			pauseButton.onClick.AddListener(OnClickPauseMenuButton);

			void OnClickPauseMenuButton()
				=> (PlayerState.IsInsideShip()
					? shipSpawnMenu
					: playerSpawnMenu).EnableMenu(true);

			var astroObjects = Resources.FindObjectsOfTypeAll<AstroObject>().ToList();
			var astroSpawnPoints = new Dictionary<AstroObject, SpawnPoint[]>();
			var noAstroSpawnPoints = Resources.FindObjectsOfTypeAll<SpawnPoint>().ToList();

			foreach (var astroObject in astroObjects)
			{
				var attachedSpawnPoints = astroObject.GetComponentsInChildren<SpawnPoint>(true);
				astroSpawnPoints[astroObject] = attachedSpawnPoints;
				noAstroSpawnPoints = noAstroSpawnPoints.Except(attachedSpawnPoints).ToList();
			}

			astroObjects.Sort((a, b) => astroSpawnPoints[a].Length.CompareTo(astroSpawnPoints[b].Length));

			void CloseMenu()
			{
				shipSpawnMenu.EnableMenu(false);
				playerSpawnMenu.EnableMenu(false);
				ModHelper.Menus.PauseMenu.Close();
			}

			void CreateSpawnPointButton(SpawnPoint spawnpoint, Menu menu, string name)
			{
				var newButton = MenuApi.PauseMenu_MakeSimpleButton(name, menu);
				newButton.onClick.AddListener(OnClick);

				void OnClick()
				{
					menu.EnableMenu(false);
					CloseMenu();
					SpawnAt(spawnpoint);
				}
			}

			void CreateSpawnPointList(List<SpawnPoint> spawnPoints, AstroObject astroObject, Menu buttonAttachMenu)
			{
				var newMenu = MenuApi.PauseMenu_MakePauseListMenu(GetAstroObjectName(astroObject));

				MenuApi.PauseMenu_MakeMenuOpenButton(GetAstroObjectName(astroObject), newMenu, buttonAttachMenu);

				for (var i = 0; i < spawnPoints.Count; i++)
				{
					var point = spawnPoints[i];
					CreateSpawnPointButton(point, newMenu, point.name);
				}
			}

			void CreateNoAstroSpawnPointList(List<SpawnPoint> spawnPoints, Menu buttonAttachMenu)
			{
				var newMenu = MenuApi.PauseMenu_MakePauseListMenu("NO ASTROOBJECTS");

				MenuApi.PauseMenu_MakeMenuOpenButton("No AstroObject...", newMenu, buttonAttachMenu);

				for (var i = 0; i < spawnPoints.Count; i++)
				{
					var point = spawnPoints[i];
					CreateSpawnPointButton(point, newMenu, point.name);
				}
			}

			foreach (var astroObject in astroObjects)
			{
				var allSpawnPoints = astroSpawnPoints[astroObject];
				if (allSpawnPoints.Length == 0)
				{
					continue;
				}

				var shipSpawnPoints = allSpawnPoints.Where(point => point.IsShipSpawn()).ToList();
				var playerSpawnPoints = allSpawnPoints.Where(point => !point.IsShipSpawn()).ToList();

				var astroName = GetAstroObjectName(astroObject);

				if (shipSpawnPoints.Count > 1)
				{
					CreateSpawnPointList(shipSpawnPoints, astroObject, shipSpawnMenu);
				}
				else if (shipSpawnPoints.Count == 1)
				{
					CreateSpawnPointButton(shipSpawnPoints[0], shipSpawnMenu, $"{astroName} - {shipSpawnPoints[0].name}");
				}

				if (playerSpawnPoints.Count > 1)
				{
					CreateSpawnPointList(playerSpawnPoints, astroObject, playerSpawnMenu);
				}
				else if (playerSpawnPoints.Count == 1)
				{
					CreateSpawnPointButton(playerSpawnPoints[0], playerSpawnMenu, $"{astroName} - {playerSpawnPoints[0].name}");
				}
			}

			var noAstroShipSpawns = noAstroSpawnPoints.Where(point => point.IsShipSpawn()).ToList();
			var noAstroPlayerSpawns = noAstroSpawnPoints.Where(point => !point.IsShipSpawn()).ToList();

			if (noAstroShipSpawns.Count > 1)
			{
				CreateNoAstroSpawnPointList(noAstroShipSpawns, shipSpawnMenu);
			}
			else if (noAstroShipSpawns.Count == 1)
			{
				CreateSpawnPointButton(noAstroShipSpawns[0], shipSpawnMenu, noAstroShipSpawns[0].name);
			}

			if (noAstroPlayerSpawns.Count > 1)
			{
				CreateNoAstroSpawnPointList(noAstroPlayerSpawns, playerSpawnMenu);
			}
			else if (noAstroPlayerSpawns.Count == 1)
			{
				CreateSpawnPointButton(noAstroPlayerSpawns[0], playerSpawnMenu, noAstroPlayerSpawns[0].name);
			}
		}

		private string GetAstroObjectName(AstroObject astroObject)
		{
			var astroNameEnum = astroObject.GetAstroObjectName();
			var astroName = astroNameEnum.ToString();

			if (astroNameEnum == AstroObject.Name.CustomString)
			{
				return astroObject.GetCustomName();
			}
			else if (astroNameEnum == AstroObject.Name.None || astroName == null || astroName == "")
			{
				return astroObject.name;
			}

			return astroName;
		}

		private void SpawnAt(SpawnPoint point)
		{
			var body = PlayerState.IsInsideShip() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			body.WarpToPositionRotation(point.transform.position, point.transform.rotation);
			body.SetVelocity(point.GetPointVelocity());
			point.AddObjectToTriggerVolumes(Locator.GetPlayerDetector().gameObject);
			point.AddObjectToTriggerVolumes(_fluidDetector.gameObject);
			point.OnSpawnPlayer();
			OWTime.Unpause(OWTime.PauseType.Menu);

			if (_suitUpOnTravel)
			{
				Locator.GetPlayerSuit().SuitUp();
			}
		}

		private void SpawnAtInitialPoint()
		{
			var spawnPointName = _saveFile.initialSpawnPoint;
			if (spawnPointName == "")
			{
				return;
			}
			var point = FindObjectsOfType<SpawnPoint>().First(x => x.gameObject.name == spawnPointName);
			FindObjectOfType<PlayerSpawner>().SetInitialSpawnPoint(point);
		}

		private void InstantWakeUp()
		{
			_isSolarSystemLoaded = false;
			// Skip wake up animation.
			var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>();
			cameraEffectController.OpenEyes(0, true);
			cameraEffectController.SetValue("_wakeLength", 0f);
			cameraEffectController.SetValue("_waitForWakeInput", false);

			// Skip wake up prompt.
			LateInitializerManager.pauseOnInitialization = false;
			Locator.GetPauseCommandListener().RemovePauseCommandLock();
			Locator.GetPromptManager().RemoveScreenPrompt(cameraEffectController.GetValue<ScreenPrompt>("_wakePrompt"));
			OWTime.Unpause(OWTime.PauseType.Sleeping);
			cameraEffectController.Invoke("WakeUp");

			// Enable all inputs immedeately.
			OWInput.ChangeInputMode(InputMode.Character);
			typeof(OWInput).SetValue("_inputFadeFraction", 0f);
			GlobalMessenger.FireEvent("TakeFirstFlashbackSnapshot");

			Locator.GetPlayerSuit().SuitUp();
		}

		public void LateUpdate()
		{
			if (_isSolarSystemLoaded && _saveFile.initialSpawnPoint != "")
			{
				InstantWakeUp();
			}
		}
	}
}
