using OWML.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MenuFramework
{
	internal class PauseButtonManager : MonoBehaviour
	{
		public static PauseButtonManager Instance { get; private set; }

		private void Awake() => Instance = this;

		public Menu MakePauseListMenu(string title)
		{
			var newMenu = Instantiate(Main.PauseListPrefab);

			newMenu.transform.parent = GameObject.Find("PauseMenuBlock").transform;
			newMenu.transform.localScale = Vector3.one;
			newMenu.transform.localPosition = Vector3.zero;
			newMenu.GetComponent<RectTransform>().SetLeft(0);
			newMenu.GetComponent<RectTransform>().SetRight(0);
			newMenu.GetComponent<RectTransform>().SetTop(0);
			newMenu.GetComponent<RectTransform>().SetBottom(0);

			var text = newMenu.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
			text.text = title;
			newMenu.gameObject.name = title;

			return newMenu.GetComponent<Menu>();
		}

		public Button MakeSimpleButton(string name, Menu customMenu = null)
		{
			var button = CreateBase(name, customMenu);
			button.SetActive(true);
			return button.GetComponent<Button>();
		}

		public GameObject MakeMenuOpenButton(string name, Menu menuToOpen, Menu customMenu = null)
		{
			if (LoadManager.GetCurrentScene() != OWScene.SolarSystem && LoadManager.GetCurrentScene() != OWScene.EyeOfTheUniverse)
			{
				Main.Helper.Console.WriteLine("Error - Cannot create pause button in this scene!", OWML.Common.MessageType.Error);
				return null;
			}
			var menuRootObject = CreateBase(name, customMenu);

			var submitActionMenu = menuRootObject.AddComponent<SubmitActionMenu>();
			submitActionMenu.SetValue("_menuToOpen", menuToOpen);

			menuRootObject.SetActive(true);
			return menuRootObject;
		}

		public GameObject MakeSceneLoadButton(string name, SubmitActionLoadScene.LoadableScenes sceneToLoad, PopupMenu confirmPopup = null, Menu customMenu = null)
		{
			if (LoadManager.GetCurrentScene() != OWScene.SolarSystem && LoadManager.GetCurrentScene() != OWScene.EyeOfTheUniverse)
			{
				Main.Helper.Console.WriteLine("Error - Cannot create pause button in this scene!", OWML.Common.MessageType.Error);
				return null;
			}
			var menuRootObject = CreateBase(name, customMenu);

			var submitActionLoadScene = menuRootObject.AddComponent<CustomSubmitActionLoadScene>();
			submitActionLoadScene.SetSceneToLoad((CustomSubmitActionLoadScene.LoadableScenes)sceneToLoad);
			submitActionLoadScene.EnableConfirm(confirmPopup != null);
			submitActionLoadScene.SetValue("_confirmPopup", confirmPopup);
			submitActionLoadScene.SetValue("_loadingText", menuRootObject.GetComponentInChildren<Text>());

			menuRootObject.SetActive(true);
			return menuRootObject;
		}

		private GameObject CreateBase(string name, Menu customMenu = null)
		{
			if (customMenu == null)
			{
				customMenu = Resources.FindObjectsOfTypeAll<Menu>().First(x => x.name == "PauseMenuItems");
			}

			var pauseButton = Instantiate(Main.ButtonPrefab);

			// Make new button above dotted line
			var mainMenuLayoutGroup = customMenu.transform.GetChild(1).GetComponent<VerticalLayoutGroup>();
			pauseButton.transform.parent = mainMenuLayoutGroup.transform;
			pauseButton.transform.localPosition = Vector3.zero;
			pauseButton.transform.localScale = Vector3.one;
			pauseButton.transform.SetSiblingIndex(pauseButton.transform.GetSiblingIndex() - 1); // -1 because no spacer in pause menu
			pauseButton.SetActive(false);

			// Change text, and set mesh to dirty (maybe not needed?)
			pauseButton.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = name;
			pauseButton.transform.GetChild(0).GetChild(1).GetComponent<Text>().SetAllDirty();

			if (customMenu.GetSelectOnActivate() == null)
			{
				customMenu.SetSelectOnActivate(pauseButton.GetComponent<Button>());
			}

			return pauseButton;
		}
	}
}
