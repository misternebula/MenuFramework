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

		private void Awake() 
			=> Instance = this;

		public void CustomizeTitleScreen()
		{
			var mainMenuLayoutGroup = GameObject.Find("MainMenuLayoutGroup");
			foreach (Transform transform in mainMenuLayoutGroup.transform)
			{
				if (transform.GetComponent<Button>() != null)
				{
					var layoutElement = transform.GetComponent<LayoutElement>();

					layoutElement.minHeight = 44.25f;
					layoutElement.flexibleHeight = 1f;
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
			var newButton = Instantiate(Main.ButtonPrefab);

			// Make new button above dotted line and spacer
			newButton.transform.parent = GameObject.Find("MainMenuLayoutGroup").transform;
			newButton.transform.SetSiblingIndex(newButton.transform.GetSiblingIndex() - 2);
			newButton.transform.localScale = Vector3.one;

			// Change text, and set mesh to dirty (maybe not needed?)
			newButton.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = name;
			newButton.transform.GetChild(0).GetChild(1).GetComponent<Text>().SetAllDirty();

			// Set to be invisible - the title menu animation will fade it back in
			newButton.GetComponent<CanvasGroup>().alpha = 0f;

			// Add to title menu animation
			var animController = GameObject.Find("TitleMenuManagers").GetComponent<TitleAnimationController>();
			var array = animController.GetValue<CanvasGroupFadeController[]>("_buttonFadeControllers");
			var newLength = array.Length + 1;
			Array.Resize(ref array, newLength);
			array[newLength - 1] = new CanvasGroupFadeController
			{
				group = newButton.GetComponent<CanvasGroup>()
			};
			animController.SetValue("_buttonFadeControllers", array);

			return newButton;
		}
	}
}
