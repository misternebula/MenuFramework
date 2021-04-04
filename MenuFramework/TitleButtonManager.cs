using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace MenuFramework
{
	internal class TitleButtonManager : MonoBehaviour
	{
		public static TitleButtonManager Instance { get; private set; }

		private void Awake() => Instance = this;

		public void CustomizeTitleScreen()
		{
			var mainMenuLayoutGroup = GameObject.Find("MainMenuLayoutGroup");
			foreach (Transform transform in mainMenuLayoutGroup.transform)
			{
				if (transform.GetComponent<Button>() != null)
				{
					var layoutElement = transform.GetComponent<LayoutElement>();
					layoutElement.minHeight = -1f;
				}
			}
		}

		public Button MakeSimpleButton(string name)
		{
			var button = CreateBase(name);
			button.SetActive(true);
			return button.GetComponent<Button>();
		}

		public GameObject MakeMenuOpenButton(string name, Menu menuToOpen)
		{
			if (LoadManager.GetCurrentScene() != OWScene.TitleScreen)
			{
				Main.Helper.Console.WriteLine("Error - Cannot create title button in this scene!", OWML.Common.MessageType.Error);
				return null;
			}
			var menuRootObject = CreateBase(name);

			var submitActionMenu = menuRootObject.AddComponent<SubmitActionMenu>();
			submitActionMenu.SetValue("_menuToOpen", menuToOpen);

			menuRootObject.SetActive(true);
			return menuRootObject;
		}

		public GameObject MakeSceneLoadButton(string name, SubmitActionLoadScene.LoadableScenes sceneToLoad, PopupMenu confirmPopup = null)
		{
			if (LoadManager.GetCurrentScene() != OWScene.TitleScreen)
			{
				Main.Helper.Console.WriteLine("Error - Cannot create title button in this scene!", OWML.Common.MessageType.Error);
				return null;
			}
			var menuRootObject = CreateBase(name);

			var submitActionLoadScene = menuRootObject.AddComponent<SubmitActionLoadScene>();
			submitActionLoadScene.SetSceneToLoad(sceneToLoad);
			submitActionLoadScene.EnableConfirm(confirmPopup != null);
			submitActionLoadScene.SetValue("_confirmPopup", confirmPopup);
			submitActionLoadScene.SetValue("_loadingText", menuRootObject.GetComponentInChildren<Text>());

			menuRootObject.SetActive(true);
			return menuRootObject;
		}

		private GameObject CreateBase(string name)
		{
			var titleButton = new GameObject($"Button-{name}");
			var mainMenuLayoutGroup = GameObject.Find("MainMenuLayoutGroup");
			titleButton.transform.parent = mainMenuLayoutGroup.transform;
			titleButton.transform.localPosition = Vector3.zero;
			titleButton.transform.localScale = Vector3.one;
			titleButton.transform.SetSiblingIndex(titleButton.transform.GetSiblingIndex() - 2);
			titleButton.SetActive(false);

			var rect = titleButton.AddComponent<RectTransform>();
			rect.position = Vector3.zero;
			rect.sizeDelta = new Vector2(500f, 44.25f);

			var button = titleButton.AddComponent<Button>();
			button.interactable = true;
			button.transition = Selectable.Transition.None;
			button.navigation = new Navigation
			{
				mode = Navigation.Mode.Vertical
			};

			var layoutElement = titleButton.AddComponent<LayoutElement>();

			var canvasGroup = titleButton.AddComponent<CanvasGroup>();
			canvasGroup.alpha = 0f;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.ignoreParentGroups = false;

			var animController = GameObject.Find("TitleMenuManagers").GetComponent<TitleAnimationController>();
			var array = animController.GetValue<CanvasGroupFadeController[]>("_buttonFadeControllers");
			var newLength = array.Length + 1;
			Array.Resize(ref array, newLength);
			array[newLength - 1] = new CanvasGroupFadeController
			{
				group = canvasGroup
			};
			animController.SetValue("_buttonFadeControllers", array);

			titleButton.AddComponent<SelectableAudioPlayer>();

			var verticalLayoutGroup = titleButton.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 0;
			verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childControlWidth = true;

			var image = titleButton.AddComponent<Image>();
			image.raycastTarget = true;
			image.color = new Color(255, 255, 255, 0);

			CreateButtonVisuals(titleButton, name);

			return titleButton;
		}

		private void CreateButtonVisuals(GameObject button, string name)
		{
			var newLayoutGroup = Instantiate(GameObject.Find("Button-Options/LayoutGroup"));
			newLayoutGroup.SetActive(false);
			newLayoutGroup.transform.parent = button.transform;
			newLayoutGroup.transform.localPosition = Vector3.zero;
			newLayoutGroup.transform.localScale = Vector3.one;
			var text = newLayoutGroup.transform.Find("Text");
			Destroy(text.GetComponent<LocalizedText>());

			text.GetComponent<Text>().text = name;
			text.GetComponent<Text>().SetAllDirty();
			text.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 44.25f);

			var leftArrow = newLayoutGroup.transform.Find("LeftArrow").GetComponent<Image>();
			var rightArrow = newLayoutGroup.transform.Find("RightArrow").GetComponent<Image>();

			leftArrow.GetComponent<LayoutElement>().preferredHeight = 40f;
			leftArrow.GetComponent<LayoutElement>().preferredWidth = 40f;

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

			newLayoutGroup.SetActive(true);
		}
	}
}
