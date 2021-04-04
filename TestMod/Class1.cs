using OWML.ModHelper;

namespace TestMod
{
	public class Class1 : ModBehaviour
	{
		public IMenuAPI MenuApi;

		public void Start()
		{
			LoadManager.OnCompleteSceneLoad += OnSceneLoaded;
			MenuApi = ModHelper.Interaction.GetModApi<IMenuAPI>("_nebula.MenuFramework");
			MakeTitleMenus();
		}

		private void MakePauseMenus()
		{
			var customMenu = MenuApi.PauseMenu_MakePauseListMenu("TELEPORT");

			MenuApi.PauseMenu_MakeMenuOpenButton("TELEPORT TO...", customMenu);
			var shipButton = MenuApi.PauseMenu_MakeSimpleButton("SHIP", customMenu);

			shipButton.onClick.AddListener(WarpToShip);
		}

		private void WarpToShip()
		{
			var playerSpawner = Locator.GetPlayerBody().GetComponent<PlayerSpawner>();
			playerSpawner.DebugWarp(playerSpawner.GetSpawnPoint(SpawnLocation.Ship));
		}

		private void MakeTitleMenus()
		{
			var twoChoicePopup = MenuApi.MakeTwoChoicePopup("Do thing?", "Yes", "No");
			var inputPopup = MenuApi.MakeInputFieldPopup("Enter message :", "Put message here!", "Confirm", "Cancel");
			inputPopup.GetComponent<PopupInputMenu>().OnPopupConfirm += () => ModHelper.Console.WriteLine(inputPopup.GetComponent<PopupInputMenu>().GetInputText());

			MenuApi.TitleScreen_MakeMenuOpenButton("two choice", twoChoicePopup.GetComponent<Menu>());
			MenuApi.TitleScreen_MakeMenuOpenButton("input", inputPopup.GetComponent<Menu>());
			MenuApi.TitleScreen_MakeSceneLoadButton("confirm load eye", SubmitActionLoadScene.LoadableScenes.EYE, twoChoicePopup.GetComponent<PopupMenu>());
			MenuApi.TitleScreen_MakeSceneLoadButton("load eye", SubmitActionLoadScene.LoadableScenes.EYE);
		}

		private void OnSceneLoaded(OWScene from, OWScene to)
		{
			switch (to)
			{
				case OWScene.EyeOfTheUniverse:
				case OWScene.SolarSystem:
					MakePauseMenus();
					break;
				case OWScene.TitleScreen:
					MakeTitleMenus();
					break;
			}
		}
	}
}
