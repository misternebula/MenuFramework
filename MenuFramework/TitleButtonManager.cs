using OWML.Utils;
using System;
using System.Linq;
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

		public Button MakeSimpleButton(string name, int index)
		{
			var button = CreateBase(name, index);
			button.SetActive(true);
			return button.GetComponent<Button>();
}

		public GameObject MakeMenuOpenButton(string name, int index, Menu menuToOpen)
		{
			if (LoadManager.GetCurrentScene() != OWScene.TitleScreen)
			{
				Main.Helper.Console.WriteLine("Error - Cannot create title button in this scene!", OWML.Common.MessageType.Error);
				return null;
			}

			var menuRootObject = CreateBase(name, index);

			var submitActionMenu = menuRootObject.AddComponent<SubmitActionMenu>();
			submitActionMenu._menuToOpen = menuToOpen;

			menuRootObject.SetActive(true);

			return menuRootObject;
		}

		public GameObject MakeSceneLoadButton(string name, int index, SubmitActionLoadScene.LoadableScenes sceneToLoad, PopupMenu confirmPopup = null)
		{
			if (LoadManager.GetCurrentScene() != OWScene.TitleScreen)
			{
				Main.Helper.Console.WriteLine("Error - Cannot create title button in this scene!", OWML.Common.MessageType.Error);
				return null;
			}

			var menuRootObject = CreateBase(name, index);

			var submitActionLoadScene = menuRootObject.AddComponent<CustomSubmitActionLoadScene>();
			submitActionLoadScene.SetSceneToLoad((CustomSubmitActionLoadScene.LoadableScenes)sceneToLoad);
			submitActionLoadScene.EnableConfirm(confirmPopup != null);
			submitActionLoadScene._confirmPopup = confirmPopup;
			submitActionLoadScene._loadingText = menuRootObject.GetComponentInChildren<Text>();

			menuRootObject.SetActive(true);
			return menuRootObject;
		}

		private GameObject CreateBase(string name, int index)
		{
			var newButton = Instantiate(Main.ButtonPrefab);

			// Make new button above dotted line and spacer
			newButton.transform.parent = GameObject.Find("MainMenuLayoutGroup").transform;
			newButton.transform.SetSiblingIndex(index + 2);
			newButton.transform.localScale = Vector3.one;
			newButton.name = $"Button-{name}";

			// Change text, and set mesh to dirty (maybe not needed?)
			newButton.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = name;
			newButton.transform.GetChild(0).GetChild(1).GetComponent<Text>().SetAllDirty();

			// Set to be invisible - the title menu animation will fade it back in
			newButton.GetComponent<CanvasGroup>().alpha = 0f;

			// Add to title menu animation
			var animController = GameObject.Find("TitleMenuManagers").GetComponent<TitleAnimationController>();
			//var array = animController._buttonFadeControllers;
			var list = animController._buttonFadeControllers.ToList();
			list.Insert(index, new CanvasGroupFadeController
			{
				group = newButton.GetComponent<CanvasGroup>()
			});
			animController._buttonFadeControllers = list.ToArray();

			return newButton;
		}
	}
}
