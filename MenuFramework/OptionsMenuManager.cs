using OWML.Common;
using OWML.ModHelper.Menus;
using OWML.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MenuFramework
{
	class OptionsMenuManager : MonoBehaviour
	{
		public static OptionsMenuManager Instance { get; private set; }

		private static Dictionary<Menu, List<Selectable>> MenuSelectables = new Dictionary<Menu, List<Selectable>>();

		private void Awake()
		{
			Instance = this;
			Main.Helper.HarmonyHelper.EmptyMethod<ModMenus>("OnEvent");
		}

		public Menu MakeNonScrollingOptionsTab(string name)
		{
			var newButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => x.name == "Button-Graphics" && x.transform.parent.name == "TabButtons" && x.transform.parent.parent.name == "Tabs").gameObject);
			newButton.name = $"Button-{name}";
			newButton.transform.parent = GameObject.Find("OptionsCanvas").transform.Find("OptionsMenu-Panel").transform.Find("Tabs").transform.Find("TabButtons");
			newButton.transform.localScale = Vector3.one;
			
			var text = newButton.transform.Find("Text").GetComponent<Text>();
			text.text = name;
			Destroy(text.GetComponent<LocalizedText>());

			var newMenu = new GameObject($"{name}Menu");
			newMenu.SetActive(false);
			newMenu.layer = 5;

			newMenu.transform.parent = GameObject.Find("OptionsCanvas").transform.Find("OptionsMenu-Panel").transform.Find("OptionsDisplayPanel");

			var rectTransform = newMenu.AddComponent<RectTransform>();
			rectTransform.anchorMin = new Vector2(0, 1);
			rectTransform.anchorMax = new Vector2(1, 1);
			rectTransform.pivot = new Vector2(0.5f, 1);
			rectTransform.SetLeft(100);
			rectTransform.SetRight(100);
			rectTransform.SetHeight(665);
			rectTransform.SetPosY(-50);
			rectTransform.SetPosZ(0);
			newMenu.transform.localScale = Vector3.one;

			newMenu.AddComponent<CanvasRenderer>();

			var menu = newMenu.AddComponent<Menu>();
			menu.SetValue("_addToMenuStackManager", false);
			menu.SetValue("_menuActivationRoot", newMenu);
			menu.SetValue("_selectableItemsRoot", newMenu);
			if (newButton.GetComponent<Button>() == null)
			{
				Main.Helper.Console.WriteLine("NO BUTTON!");
			}
			newButton.GetComponent<TabButton>().SetValue("_tabbedMenu", menu);

			var layoutGroup = new GameObject("LayoutGroup");
			layoutGroup.transform.parent = newMenu.transform;
			layoutGroup.layer = 5;

			var layoutRt = layoutGroup.AddComponent<RectTransform>();
			layoutRt.anchorMin = Vector2.zero;
			layoutRt.anchorMax = Vector2.one;
			layoutRt.pivot = new Vector2(0.5f, 0.5f);
			layoutRt.SetLeft(50);
			layoutRt.SetRight(0);
			layoutRt.SetTop(0);
			layoutRt.SetBottom(0);
			layoutRt.SetPosZ(0);
			layoutRt.localScale = Vector3.one;

			layoutGroup.AddComponent<CanvasRenderer>();

			var VLG = layoutGroup.AddComponent<VerticalLayoutGroup>();
			VLG.spacing = 0f;
			VLG.childAlignment = TextAnchor.UpperCenter;
			VLG.childControlHeight = true;
			VLG.childControlWidth = true;
			VLG.childForceExpandHeight = false;
			VLG.childForceExpandWidth = false;

			var tabbedOptionMenu = GameObject.Find("OptionsCanvas").transform.Find("OptionsMenu-Panel").GetComponent<TabbedOptionMenu>();
			var currentList = tabbedOptionMenu.GetValue<TabButton[]>("_menuTabs").ToList();
			currentList.Add(newButton.GetComponent<TabButton>());
			tabbedOptionMenu.SetValue("_menuTabs", currentList.ToArray());

			return menu;
		}

		public GameObject CreateTwoButtonToggle(string label, string trueText, string falseText, string tooltipText, bool savedValue, Menu menuTab)
		{
			var newElement = Instantiate(Main.TwoButtonElementPrefab);
			newElement.name = $"UIElement-{label}";
			newElement.SetActive(false);
			newElement.layer = 5;

			var menuType = GetMenuType(menuTab);
			if (menuType == CurrentMenuType.None)
			{
				Main.Helper.Console.WriteLine("INCORRECT MENU FORMAT");
				return null;
			}

			ChildUIElement(newElement, menuTab, menuType);

			var labelText = newElement.transform.Find("HorizontalLayoutGroup/LabelBlock/HorizontalLayoutGroup/Label").GetComponent<Text>();
			Destroy(labelText.GetComponent<LocalizedText>());
			labelText.text = label;

			var trueTextElement = newElement.transform.Find("HorizontalLayoutGroup/ControlBlock/Panel-LeftButton/Button-ON/Text").GetComponent<Text>();
			Destroy(trueTextElement.GetComponent<LocalizedText>());
			trueTextElement.text = trueText;
			var falseTextElement = newElement.transform.Find("HorizontalLayoutGroup/ControlBlock/Panel-RightButton/Button-OFF/Text").GetComponent<Text>();
			Destroy(falseTextElement.GetComponent<LocalizedText>());
			falseTextElement.text = falseText;

			var twoButton = newElement.GetComponent<TwoButtonToggleElement>();
			twoButton.SetValue("_tooltipTextType", UITextType.None);
			twoButton.SetValue("_testText", tooltipText); // Thanks Mobius! :P

			AddToNavigation(menuTab, newElement.GetComponent<Button>());
			UpdateNaviagation(menuTab);

			twoButton.Initialize(savedValue);
			newElement.SetActive(true);
			return newElement;
		}

		public GameObject CreateNonDisplaySliderElement(string label, string tooltipText, float savedValue, Menu menuTab)
		{
			var newElement = Instantiate(Main.NonDisplaySliderElementPrefab);
			newElement.name = $"UIElement-{label}";
			newElement.SetActive(false);
			newElement.layer = 5;

			var menuType = GetMenuType(menuTab);
			if (menuType == CurrentMenuType.None)
			{
				Main.Helper.Console.WriteLine("INCORRECT MENU FORMAT");
				return null;
			}

			ChildUIElement(newElement, menuTab, menuType);

			var labelText = newElement.transform.Find("HorizontalLayoutGroup/Panel-Label/HorizontalLayoutGroup/Label").GetComponent<Text>();
			Destroy(labelText.GetComponent<LocalizedText>());
			labelText.text = label;

			var sliderElement = newElement.GetComponent<SliderElement>();
			sliderElement.SetValue("_tooltipTextType", UITextType.None);
			sliderElement.SetValue("_testText", tooltipText); // Thanks Mobius! :P

			AddToNavigation(menuTab, newElement.GetComponent<Selectable>());
			UpdateNaviagation(menuTab);

			sliderElement.Initialize((int)savedValue);
			newElement.SetActive(true);
			return newElement;
		}

		public void CreateSpacer(float minHeight, Menu menuTab)
		{
			var newElement = new GameObject("Spacer");

			var menuType = GetMenuType(menuTab);
			if (menuType == CurrentMenuType.None)
			{
				Main.Helper.Console.WriteLine("INCORRECT MENU FORMAT");
				return;
			}

			ChildUIElement(newElement, menuTab, menuType);

			var layout = newElement.AddComponent<LayoutElement>();
			layout.minHeight = minHeight;

			newElement.SetActive(true);
		}

		public void CreateLabel(string label, Menu menuTab)
		{
			var newElement = Instantiate(Main.LabelElementPrefab);
			newElement.name = $"UIElement-{label}Label";
			newElement.SetActive(false);
			newElement.layer = 5;

			var menuType = GetMenuType(menuTab);
			if (menuType == CurrentMenuType.None)
			{
				Main.Helper.Console.WriteLine("INCORRECT MENU FORMAT");
				return;
			}

			ChildUIElement(newElement, menuTab, menuType);

			var labelText = newElement.transform.Find("Label").GetComponent<Text>();
			Destroy(labelText.GetComponent<LocalizedText>());
			labelText.text = label;

			newElement.SetActive(true);

			Main.Helper.Console.WriteLine($"{newElement.GetComponent<RectTransform>().sizeDelta}");
		}

		public GameObject CreateTextInput(string label, string tooltipText, string placeholderText, string savedValue, Menu menuTab)
		{
			var newElement = Instantiate(Main.TextInputElementPrefab);
			newElement.name = $"UIElement-TextInput";
			newElement.SetActive(false);
			newElement.layer = 5;

			var menuType = GetMenuType(menuTab);
			if (menuType == CurrentMenuType.None)
			{
				Main.Helper.Console.WriteLine("INCORRECT MENU FORMAT");
			}

			var labelText = newElement.transform.Find("HorizontalLayoutGroup/LabelBlock/HorizontalLayoutGroup/Label").GetComponent<Text>();
			//Destroy(labelText.GetComponent<LocalizedText>());
			labelText.text = label;

			var inputField = newElement.transform.Find("HorizontalLayoutGroup/ControlBlock/InputField").GetComponent<InputField>();
			var placeholder = inputField.placeholder;
			var placeholderTextComp = placeholder.GetComponent<Text>();
			placeholderTextComp.text = placeholderText;
			inputField.text = savedValue;

			ChildUIElement(newElement, menuTab, menuType);

			AddToNavigation(menuTab, newElement.GetComponent<Button>());
			UpdateNaviagation(menuTab);

			newElement.SetActive(true);
			return newElement;
		}

		private void AddToNavigation(Menu menuTab, Selectable selectable)
		{
			if (!MenuSelectables.ContainsKey(menuTab))
			{
				Main.Helper.Console.WriteLine($"Create entry for {menuTab.name}");
				MenuSelectables.Add(menuTab, new List<Selectable>());
			}
			Main.Helper.Console.WriteLine($"Add {selectable.name} to entry for {menuTab.name}");
			MenuSelectables[menuTab].Add(selectable);
		}

		private void UpdateNaviagation(Menu menuTab)
		{
			var selectableCount = MenuSelectables[menuTab].Count;
			var lastIndex = selectableCount - 1;
			if (selectableCount == 0)
			{
				Main.Helper.Console.WriteLine("Error - Cannot update navigation of menu that has 0 elements!", MessageType.Error);
				return;
			}
			menuTab.SetSelectOnActivate(MenuSelectables[menuTab][0]);

			if (selectableCount == 1)
			{
				return;
			}

			for (var i = 0; i < selectableCount; i++)
			{
				var element = MenuSelectables[menuTab][i];
				var selectable = element.GetComponent<Selectable>();
				Selectable onUp;
				Selectable onDown;
				if (i == 0)
				{
					onUp = MenuSelectables[menuTab][lastIndex];
					onDown = MenuSelectables[menuTab][i + 1];
				}
				else if (i == lastIndex)
				{
					onUp = MenuSelectables[menuTab][i - 1];
					onDown = MenuSelectables[menuTab][0];
				}
				else
				{
					onUp = MenuSelectables[menuTab][i - 1];
					onDown = MenuSelectables[menuTab][i + 1];
				}
				selectable.navigation = new Navigation()
				{
					mode = Navigation.Mode.Explicit,
					selectOnUp = onUp,
					selectOnDown = onDown
				};
			}
		}

		private void ChildUIElement(GameObject element, Menu menuTab, CurrentMenuType type)
		{
			if (type == CurrentMenuType.NonScrolling)
			{
				Main.Helper.Console.WriteLine($"Childing {element.name} to {menuTab.name}/{menuTab.transform.GetChild(0).name}");
				element.transform.parent = menuTab.transform.GetChild(0);
				element.transform.localScale = Vector3.one;
				return;
			}
			Main.Helper.Console.WriteLine($"Childing {element.name} to {menuTab.name}/{menuTab.transform.GetChild(0).name}/{menuTab.transform.GetChild(0).GetChild(0).name}/{menuTab.transform.GetChild(0).GetChild(0).GetChild(0).name}");
			element.transform.parent = menuTab.transform.GetChild(0).GetChild(0).GetChild(0);
			element.transform.localScale = Vector3.one;
		}

		private CurrentMenuType GetMenuType(Menu menuTab)
		{
			var child0 = menuTab.transform.GetChild(0);
			if (child0.name == "LayoutGroup")
			{
				return CurrentMenuType.NonScrolling;
			}
			else if (child0.name == "Scroll View")
			{
				return CurrentMenuType.Scrolling;
			}
			return CurrentMenuType.None;
		}

		private enum CurrentMenuType
		{
			None,
			NonScrolling,
			Scrolling
		}
	}
}
