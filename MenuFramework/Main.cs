using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MenuFramework
{
	public class Main : ModBehaviour
	{
		public static IModHelper Helper { get; private set; }
		public static AssetBundle MenuBundle { get; private set; }
		public static GameObject ButtonPrefab { get; private set; }
		public static GameObject PauseListPrefab { get; private set; }
		public static Font AdobeSerifGothicStdExtraBold => (Font)Resources.Load("fonts/english - latin/Adobe - SerifGothicStd-ExtraBold");

		public override object GetApi() => new MenuAPI();

		public void Start()
		{
			Helper = ModHelper;

			if (AdobeSerifGothicStdExtraBold == null)
			{
				Helper.Console.WriteLine($"font is null!");
			}

			MenuBundle = Main.Helper.Assets.LoadBundle("assets/menuframework");
			SetUpButtonPrefab();
			SetUpPauseList();

			gameObject.AddComponent<TitleButtonManager>();
			gameObject.AddComponent<PopupMenuManager>();
			gameObject.AddComponent<PauseButtonManager>();

			LoadManager.OnCompleteSceneLoad += OnSceneLoad;

			TitleButtonManager.Instance.CustomizeTitleScreen();
		}

		void OnSceneLoad(OWScene from, OWScene to)
		{
			if (to == OWScene.TitleScreen)
			{
				TitleButtonManager.Instance.CustomizeTitleScreen();
			}
		}

		private void Update()
		{
			if (LoadManager.GetCurrentScene() == OWScene.SolarSystem || LoadManager.GetCurrentScene() == OWScene.EyeOfTheUniverse)
			{
				Helper.Console.WriteLine($"{Resources.FindObjectsOfTypeAll<PauseUI>().First().GetValue<Image>("_backdrop").enabled}, {Resources.FindObjectsOfTypeAll<PauseUI>().First().GetComponent<Canvas>().sortingOrder}");
				Helper.Console.WriteLine($"PauseMenuCanvas sortorder : {GameObject.Find("PauseMenuCanvas").GetComponent<Canvas>().sortingOrder}");
			}
		}

		private void SetUpButtonPrefab()
		{
			ButtonPrefab = MenuBundle.LoadAsset<GameObject>("assets/button-custombutton.prefab");
			ButtonPrefab.SetActive(false);

			var layoutGroup = ButtonPrefab.transform.GetChild(0);

			var text = layoutGroup.GetChild(1);
			text.GetComponent<Text>().font = AdobeSerifGothicStdExtraBold;

			var leftArrow = layoutGroup.transform.Find("LeftArrow").GetComponent<Image>();
			var rightArrow = layoutGroup.transform.Find("RightArrow").GetComponent<Image>();

			var uiStyleApplier = ButtonPrefab.AddComponent<UIStyleApplier>();
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

			ButtonPrefab.AddComponent<SelectableAudioPlayer>();

			var styleApplier = text.gameObject.AddComponent<TextStyleApplier>();
			styleApplier.spacing = 12f;
			styleApplier.font = text.GetComponent<Text>().font;
			styleApplier.fixedWidth = 0f;
		}

		private void SetUpPauseList()
		{
			PauseListPrefab = MenuBundle.LoadAsset<GameObject>("assets/custompausemenuitems.prefab");
			PauseListPrefab.SetActive(false);

			var menu = PauseListPrefab.AddComponent<Menu>();
			menu.SetValue("_menuActivationRoot", PauseListPrefab);
			PauseListPrefab.AddComponent<MenuCancelAction>();

			var blocker = PauseListPrefab.transform.GetChild(0);
			Main.Helper.Console.WriteLine($"{blocker.GetComponent<Canvas>().sortingOrder}");

			var labelText = PauseListPrefab.transform.GetChild(1).GetChild(0).GetChild(0);
			var styleApplier = labelText.gameObject.AddComponent<TextStyleApplier>();
			styleApplier.spacing = 12f;
			styleApplier.font = AdobeSerifGothicStdExtraBold;
			styleApplier.fixedWidth = 0f;
		}
	}
}
