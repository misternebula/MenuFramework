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
		//public static Font AdobeSerifGothicStdDynamic => (Font)Resources.Load("fonts/english - latin/Adobe - SerifGothicStd_Dynamic");

		public override object GetApi() => new MenuAPI();

		public void Start()
		{
			Helper = ModHelper;

			MenuBundle = Helper.Assets.LoadBundle("assets/menuframework");

			if (MenuBundle == null)
			{
				Helper.Console.WriteLine("Couldn't load asset bundle!", MessageType.Fatal);
				return;
			}

			foreach (var item in MenuBundle.GetAllAssetNames())
			{
				Helper.Console.WriteLine(item);
			}

			SetUpButtonPrefab();
			SetUpPauseList();
			SetUpTwoButtonElement();
			SetUpNonDisplaySliderElement();
			SetUpLabelElement();
			SetUpTextInputElement();

			gameObject.AddComponent<TitleButtonManager>();
			gameObject.AddComponent<PopupMenuManager>();
			gameObject.AddComponent<PauseButtonManager>();
			gameObject.AddComponent<StartupPopupManager>();
			//gameObject.AddComponent<OptionsMenuManager>();
		}

		private void SetUpButtonPrefab()
		{
			ButtonPrefab = MenuBundle.LoadAsset<GameObject>("assets/button-custombutton.prefab");

			if (ButtonPrefab == null)
			{
				Helper.Console.WriteLine("Couldn't load assets/button-custombutton.prefab from bundle", MessageType.Fatal);
				return;
			}

			ButtonPrefab.SetActive(false);

			var layoutGroup = ButtonPrefab.transform.GetChild(0);

			var text = layoutGroup.GetChild(1);
			text.GetComponent<Text>().font = AdobeSerifGothicStdExtraBold;

			var leftArrow = layoutGroup.transform.Find("LeftArrow").GetComponent<Image>();
			var rightArrow = layoutGroup.transform.Find("RightArrow").GetComponent<Image>();

			var uiStyleApplier = ButtonPrefab.AddComponent<UIStyleApplier>();
			uiStyleApplier._textItems = new Text[1] { text.GetComponent<Text>() };
			uiStyleApplier._foregroundGraphics =
				new Graphic[3] {
					text.GetComponent<Text>(),
					leftArrow,
					rightArrow
				};
			uiStyleApplier._backgroundGraphics = new Graphic[0];
			uiStyleApplier._onOffGraphicList =
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
				};

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

			var canvas = PauseListPrefab.GetComponent<Canvas>();
			if (canvas != null)
			{
				Destroy(canvas);
			}

			var menu = PauseListPrefab.AddComponent<Menu>();
			menu._menuActivationRoot = PauseListPrefab;
			PauseListPrefab.AddComponent<MenuCancelAction>();

			var labelText = PauseListPrefab.transform.GetChild(1).GetChild(0).GetChild(0);
			var styleApplier = labelText.gameObject.AddComponent<TextStyleApplier>();
			styleApplier.spacing = 12f;
			styleApplier.font = AdobeSerifGothicStdExtraBold;
			styleApplier.fixedWidth = 0f;
		}

		private void SetUpTwoButtonElement()
		{
			//TwoButtonElementPrefab = Instantiate(Resources.FindObjectsOfTypeAll<TwoButtonToggleElement>().First(x => x.name == "UIElement-InvertPlayer").gameObject);
			//DontDestroyOnLoad(TwoButtonElementPrefab);
			//TwoButtonElementPrefab.SetActive(false);
		}

		private void SetUpNonDisplaySliderElement()
		{
			//NonDisplaySliderElementPrefab = Instantiate(Resources.FindObjectsOfTypeAll<SliderElement>().First(x => x.name == "UIElement-FlightSensitivity").gameObject);
			//DontDestroyOnLoad(NonDisplaySliderElementPrefab);
			//NonDisplaySliderElementPrefab.SetActive(false);
		}

		private void SetUpLabelElement()
		{
			//LabelElementPrefab = Instantiate(Resources.FindObjectsOfTypeAll<RectTransform>().First(x => x.name == "UIElement-FreezeTimeWhileReadingLabel").gameObject);
			//DontDestroyOnLoad(LabelElementPrefab);
			//LabelElementPrefab.SetActive(false);
		}

		private void SetUpTextInputElement()
		{
			/*
			TextInputElementPrefab = MenuBundle.LoadAsset<GameObject>("assets/uielement-textinput.prefab");
			TextInputElementPrefab.SetActive(false);

			var text = TextInputElementPrefab.transform.Find("HorizontalLayoutGroup/LabelBlock/HorizontalLayoutGroup/Label");
			var leftArrow = TextInputElementPrefab.transform.Find("HorizontalLayoutGroup/LabelBlock/HorizontalLayoutGroup/LeftArrow").GetComponent<Image>();
			var rightArrow = TextInputElementPrefab.transform.Find("HorizontalLayoutGroup/LabelBlock/HorizontalLayoutGroup/RightArrow").GetComponent<Image>();

			var uiStyleApplier = TextInputElementPrefab.AddComponent<UIStyleApplier>();
			uiStyleApplier._textItems = new Text[1] { text.GetComponent<Text>() };
			uiStyleApplier._foregroundGraphics =
				new Graphic[3] {
					text.GetComponent<Text>(),
					leftArrow,
					rightArrow
				};
			uiStyleApplier._onOffGraphicList =
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
				};
			*/
		}
	}
}
