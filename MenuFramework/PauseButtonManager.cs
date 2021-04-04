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
			if (LoadManager.GetCurrentScene() != OWScene.SolarSystem && LoadManager.GetCurrentScene() != OWScene.EyeOfTheUniverse)
			{
				Main.Helper.Console.WriteLine("Error - Cannot create pause menu in this scene!", OWML.Common.MessageType.Error);
				return null;
			}
			var originalPauseList = Resources.FindObjectsOfTypeAll<Menu>().First(x => x.name == "PauseMenuItems").gameObject;
			var pauseMenuItems = Instantiate(originalPauseList);
			pauseMenuItems.transform.parent = originalPauseList.transform.parent;
			pauseMenuItems.transform.localPosition = Vector3.zero;
			pauseMenuItems.transform.localScale = Vector3.one;
			pauseMenuItems.name = "CUSTOM_MENU";

			var text = pauseMenuItems.GetComponentInChildren<Text>();
			Destroy(text.GetComponent<LocalizedText>());
			text.text = title;

			var layout = pauseMenuItems.transform.Find("PauseMenuItemsLayout");
			foreach (Transform transform in layout)
			{
				if (transform.GetComponent<Button>() != null)
				{
					transform.gameObject.SetActive(false);
				}
			}
			layout.name = "CUSTOM_LAYOUT";
			return pauseMenuItems.GetComponent<Menu>();
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

			var submitActionLoadScene = menuRootObject.AddComponent<SubmitActionLoadScene>();
			submitActionLoadScene.SetSceneToLoad(sceneToLoad);
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
			var pauseButton = new GameObject($"Button-{name}");
			var mainMenuLayoutGroup = customMenu.transform.GetChild(1).GetComponent<VerticalLayoutGroup>();
			pauseButton.transform.parent = mainMenuLayoutGroup.transform;
			pauseButton.transform.localPosition = Vector3.zero;
			pauseButton.transform.localScale = Vector3.one;
			pauseButton.transform.SetSiblingIndex(pauseButton.transform.GetSiblingIndex() - 2);
			pauseButton.SetActive(false);

			var rect = pauseButton.AddComponent<RectTransform>();
			rect.position = Vector3.zero;

			var button = pauseButton.AddComponent<Button>();
			button.interactable = true;
			button.transition = Selectable.Transition.None;
			button.navigation = new Navigation
			{
				mode = Navigation.Mode.Vertical
			};

			var layoutElement = pauseButton.AddComponent<LayoutElement>();
			layoutElement.minHeight = 60f;

			var canvasGroup = pauseButton.AddComponent<CanvasGroup>();
			canvasGroup.alpha = 1f;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.ignoreParentGroups = false;

			pauseButton.AddComponent<SelectableAudioPlayer>();

			var verticalLayoutGroup = pauseButton.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 0;
			verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childControlWidth = true;

			CreateButtonVisuals(pauseButton, name);

			return pauseButton;
		}

		private void CreateButtonVisuals(GameObject button, string name)
		{
			// TODO : cache this
			var newLayoutGroup = Instantiate(Resources.FindObjectsOfTypeAll<HorizontalLayoutGroup>().First(x => x.name == "HorizontalLayoutGroup" && x.transform.parent.name == "Button-Options").gameObject);
			newLayoutGroup.SetActive(true);
			newLayoutGroup.transform.parent = button.transform;
			newLayoutGroup.transform.localPosition = Vector3.zero;
			var text = newLayoutGroup.transform.Find("Text");
			Destroy(text.GetComponent<LocalizedText>());
			text.GetComponent<Text>().text = name;

			var leftArrow = newLayoutGroup.transform.Find("LeftArrow").GetComponent<Image>();
			var rightArrow = newLayoutGroup.transform.Find("RightArrow").GetComponent<Image>();

			var uiStyleApplier = button.AddComponent<UIStyleApplier>();
			uiStyleApplier.SetValue("_textItems", new Text[1] { text.GetComponent<Text>() });
			uiStyleApplier.SetValue("_foregroundGraphics",
				new Graphic[3] {
					text.GetComponent<Text>(),
					leftArrow,
					rightArrow
				});
			uiStyleApplier.SetValue("_backgroundGraphics", new Graphic[0]);
			uiStyleApplier.SetValue("_onOffGraphicList",
				new UIStyleApplier.OnOffGraphic[2]
				{
					new UIStyleApplier.OnOffGraphic()
					{
						graphic = leftArrow,
						visibleHighlighted = true
					},
					new UIStyleApplier.OnOffGraphic()
					{
						graphic = rightArrow,
						visibleHighlighted = true
					}
				});
		}
	}
}
