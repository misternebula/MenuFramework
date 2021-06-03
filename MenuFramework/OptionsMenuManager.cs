using OWML.Common;
using OWML.Common.Menus;
using OWML.ModHelper.Menus;
using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MenuFramework
{
	public static class Patches
	{
		public static bool TabButton_InitializeNavigation(
			TabButton __instance,
			ref Button ____mySelectable,
			ref Selectable ____firstSelectable,
			Menu ____tabbedMenu,
			ref bool ____navigationInitialized)
		{
			Main.Helper.Console.WriteLine($"{__instance.name} - Init Navigation", OWML.Common.MessageType.Info);
			if (____mySelectable == null)
			{
				Main.Helper.Console.WriteLine($"{__instance.name} - mySelectable null 1", OWML.Common.MessageType.Error);
				____mySelectable = __instance.GetRequiredComponent<Button>();
				if (____mySelectable == null)
				{
					Main.Helper.Console.WriteLine($"{__instance.name} - mySelectable null 2!!", OWML.Common.MessageType.Error);
					return false;
				}
				else
				{
					Main.Helper.Console.WriteLine($"{__instance.name} - mySelectable ok");
				}
			}
			else
			{
				Main.Helper.Console.WriteLine($"{__instance.name} - mySelectable ok");
			}

			if (____firstSelectable == null)
			{
				Main.Helper.Console.WriteLine($"{__instance.name} - firstSelectable null 1", OWML.Common.MessageType.Error);
				____firstSelectable = ____tabbedMenu.GetSelectOnActivate();
				if (____firstSelectable == null)
				{
					Main.Helper.Console.WriteLine($"{__instance.name} - firstSelectable null 2!! menu:{____tabbedMenu.name}", OWML.Common.MessageType.Error);
					return false;
				}
				else
				{
					Main.Helper.Console.WriteLine($"{__instance.name} - firstSelectable ok");
				}
			}
			else
			{
				Main.Helper.Console.WriteLine($"{__instance.name} - firstSelectable ok");
			}

			if (!____navigationInitialized)
			{
				Main.Helper.Console.WriteLine($"{__instance.name} - nav not init-ed");
				____navigationInitialized = true;

				var selectOnActivate = ____tabbedMenu.GetSelectOnActivate();
				if (selectOnActivate == null)
				{
					Main.Helper.Console.WriteLine($"{__instance.name} - selectOnActivate null!", OWML.Common.MessageType.Error);
					return false;
				}

				var navigation = ____mySelectable.navigation;
				navigation.selectOnDown = selectOnActivate;
				____mySelectable.navigation = navigation;
				var navigation2 = selectOnActivate.navigation;
				selectOnActivate.navigation = navigation2;
			}
			else
			{
				Main.Helper.Console.WriteLine($"{__instance.name} - nav already init-ed");
			}

			return false;
		}

		/*
		public static bool TabbedMenu_GetSelectOnActivate(TabbedMenu __instance, ref Selectable __result, ref Selectable ____selectOnActivate, TabButton ____lastSelectedTabButton)
		{
			Main.Helper.Console.WriteLine($"{__instance.name} - override getselectonactivate", OWML.Common.MessageType.Info);
			__instance.Invoke("Initialize");
			____selectOnActivate = ____lastSelectedTabButton.GetSelectableMenuOptionOnOpen();
			Main.Helper.Console.WriteLine($"{__instance.name} - last selected tab button is : {____lastSelectedTabButton.name}");
			Main.Helper.Console.WriteLine($"{__instance.name} - return null? : {____selectOnActivate == null}");
			__result = ____selectOnActivate;
			return false;
		}

		
		public static bool Menu_GetSelectOnActivate(Menu __instance, ref Selectable __result, Selectable ____selectOnActivate)
		{
			Main.Helper.Console.WriteLine($"{__instance.name} - getselectonactivate", OWML.Common.MessageType.Info);
			Main.Helper.Console.WriteLine($"{__instance.name} - return null? : {____selectOnActivate == null}");
			__result = ____selectOnActivate;
			return false;
		}

		public static bool Menu_SetSelectOnActivate(Menu __instance, ref Selectable ____selectOnActivate, Selectable s)
		{
			Main.Helper.Console.WriteLine($"{__instance.name} - setselectonactivate", OWML.Common.MessageType.Info);
			Main.Helper.Console.WriteLine($"{__instance.name} - s null? : {s == null}");
			____selectOnActivate = s;
			Main.Helper.Console.WriteLine($"{__instance.name} - selectOnActivate null? : {____selectOnActivate == null}");
			return false;
		}
		*/

		public static bool Menu_EnableMenu(Menu __instance, bool value)
		{
			if (MenuStackManager.SharedInstance.Peek() == null)
			{
				if (value)
				{
					Main.Helper.Console.WriteLine($"ENABLE {__instance.name} (top of stack : none)", OWML.Common.MessageType.Info);
				}
				else
				{
					Main.Helper.Console.WriteLine($"DISABLE {__instance.name} (top of stack : none)", OWML.Common.MessageType.Warning);
				}
			}
			else
			{
				if (value)
				{
					Main.Helper.Console.WriteLine($"ENABLE {__instance.name} (top of stack : {MenuStackManager.SharedInstance.Peek().name})", OWML.Common.MessageType.Info);
				}
				else
				{
					Main.Helper.Console.WriteLine($"DISABLE {__instance.name} (top of stack : {MenuStackManager.SharedInstance.Peek().name})", OWML.Common.MessageType.Warning);
				}
			}
			
			return true;
		}

		public static bool TabButton_Enable(TabButton __instance, bool value)
		{
			if (value)
			{
				Main.Helper.Console.WriteLine($"TAB-BUTTON ENABLE {__instance.name}", OWML.Common.MessageType.Info);
			}
			else
			{
				Main.Helper.Console.WriteLine($"TAB-BUTTON DISABLE {__instance.name}", OWML.Common.MessageType.Warning);
			}
			return true;
		}

		public static bool TabbedMenu_Deactivate(TabbedMenu __instance)
		{
			Main.Helper.Console.WriteLine($"DEACTIVATE {__instance.name}", OWML.Common.MessageType.Warning);
			return true;
		}

		public static bool TabbedMenu_SelectTabButton(TabbedMenu __instance, TabButton tabButton)
		{
			Main.Helper.Console.WriteLine($"SelectTabButton {__instance.name} - {tabButton.name}", OWML.Common.MessageType.Info);
			return true;
		}

		public static bool MenuStackManager_Pop()
		{
			Main.Helper.Console.WriteLine($"POP {MenuStackManager.SharedInstance.Peek().name}", OWML.Common.MessageType.Error);
			return true;
		}

		public static bool MenuStackManager_Push(Menu menu)
		{
			if (MenuStackManager.SharedInstance.Peek() != null)
			{
				Main.Helper.Console.WriteLine($"PUSH {menu.name} disabling {MenuStackManager.SharedInstance.Peek().name}", OWML.Common.MessageType.Success);
			}
			else
			{
				Main.Helper.Console.WriteLine($"PUSH {menu.name}", OWML.Common.MessageType.Success);
			}
			return true;
		}

		public static bool ModMenu_Initialize(ModMenu __instance, Menu menu, LayoutGroup layoutGroup, ref LayoutGroup ___Layout, ref List<IModInputBase> ____inputs)
		{
			Main.Helper.Console.WriteLine($"init");
			if (__instance.Menu == menu)
			{
				Main.Helper.Console.WriteLine($"__instance.Menu already set to menu", MessageType.Warning);
			}
			if (menu == null)
			{
				Main.Helper.Console.WriteLine("NULL MENU!");
				return false;
			}
			else
			{
				Main.Helper.Console.WriteLine($"{menu.name}");
				Main.Helper.Console.WriteLine("menu ok");
			}
			__instance.SetPrivatePropertyValue("Menu", menu);
			if (__instance.Menu != menu)
			{
				Main.Helper.Console.WriteLine($"__instance.Menu was not set!!", MessageType.Error);
				__instance.SetPrivatePropertyValue("Menu", menu);
			}
			else
			{
				Main.Helper.Console.WriteLine($"__instance.Menu was set", MessageType.Success);
			}
			if (layoutGroup == null)
			{
				Main.Helper.Console.WriteLine("NULL LAYOUT!");
				return false;
			}
			else
			{
				Main.Helper.Console.WriteLine("layout ok");
			}
			___Layout = layoutGroup;

			Main.Helper.Console.WriteLine("get components in children");
			if (__instance.Menu == null)
			{
				Main.Helper.Console.WriteLine("menu is null after setting to not null??");
				return false;
			}
			var componentsInChildren = __instance.Menu.GetComponentsInChildren<ButtonWithHotkeyImageElement>(true);
			Main.Helper.Console.WriteLine("select");
			var promptButtons = componentsInChildren.Select(x => x.GetComponent<Button>()).ToList();

			Main.Helper.Console.WriteLine("set basebuttons");
			__instance.SetValue("BaseButtons", new List<IModButtonBase>()
				.Concat(promptButtons.Select(x => new ModPromptButton(x, __instance, __instance.GetValue<IModConsole>("Console"))).Cast<IModButtonBase>())
				.Concat(__instance.Menu.GetComponentsInChildren<Button>(true).Except(promptButtons).Select(x => new ModTitleButton(x, __instance)).Cast<IModButtonBase>())
				.ToList());
			Main.Helper.Console.WriteLine("get inputs");
			____inputs = new List<IModInputBase>()
				.Concat(__instance.Menu.GetComponentsInChildren<TwoButtonToggleElement>(true).Select(x => new ModToggleInput(x, __instance)).Cast<IModInputBase>())
				.Concat(__instance.Menu.GetComponentsInChildren<SliderElement>(true).Select(x => new ModSliderInput(x, __instance)).Cast<IModInputBase>())
				.Concat(__instance.Menu.GetComponentsInChildren<OptionsSelectorElement>(true).Select(x => new ModSelectorInput(x, __instance)).Cast<IModInputBase>())
				.ToList();
			__instance.SetValue("Seperators", new List<IModSeparator>());
			Main.Helper.Console.WriteLine($"- FINISH init {menu.name}");
			return false;
		}

		public static bool Menu_InitializeMenu(Menu __instance, ref GameObject ____selectableItemsRoot, ref GameObject ____menuActivationRoot)
		{
			Main.Helper.Console.WriteLine($"INITIALIZE {__instance.name}");
			if (____selectableItemsRoot == null)
			{
				Main.Helper.Console.WriteLine(" - Selectable Items Root null!");
			}
			if (____menuActivationRoot == null)
			{
				Main.Helper.Console.WriteLine(" - Menu Activation Root null!");
			}
			return true;
		}
	}
	class OptionsMenuManager : MonoBehaviour
	{
		public static OptionsMenuManager Instance { get; private set; }

		private static Dictionary<Menu, List<Selectable>> MenuSelectables = new Dictionary<Menu, List<Selectable>>();

		private void Awake()
		{
			Instance = this;
			Main.Helper.HarmonyHelper.AddPrefix<TabButton>("InitializeNavigation", typeof(Patches), nameof(Patches.TabButton_InitializeNavigation));
			//Main.Helper.HarmonyHelper.AddPrefix<TabbedMenu>("GetSelectOnActivate", typeof(Patches), nameof(Patches.TabbedMenu_GetSelectOnActivate));
			//Main.Helper.HarmonyHelper.AddPrefix<Menu>("GetSelectOnActivate", typeof(Patches), nameof(Patches.Menu_GetSelectOnActivate));
			//Main.Helper.HarmonyHelper.AddPrefix<Menu>("SetSelectOnActivate", typeof(Patches), nameof(Patches.Menu_SetSelectOnActivate));
			Main.Helper.HarmonyHelper.AddPrefix<Menu>("EnableMenu", typeof(Patches), nameof(Patches.Menu_EnableMenu));
			Main.Helper.HarmonyHelper.AddPrefix<TabButton>("Enable", typeof(Patches), nameof(Patches.TabButton_Enable));
			Main.Helper.HarmonyHelper.AddPrefix<TabbedMenu>("Deactivate", typeof(Patches), nameof(Patches.TabbedMenu_Deactivate));
			Main.Helper.HarmonyHelper.AddPrefix<TabbedMenu>("SelectTabButton", typeof(Patches), nameof(Patches.TabbedMenu_SelectTabButton));
			Main.Helper.HarmonyHelper.AddPrefix<MenuStackManager>("Pop", typeof(Patches), nameof(Patches.MenuStackManager_Pop));
			Main.Helper.HarmonyHelper.AddPrefix<MenuStackManager>("Push", typeof(Patches), nameof(Patches.MenuStackManager_Push));
			Main.Helper.HarmonyHelper.AddPrefix<Menu>("InitializeMenu", typeof(Patches), nameof(Patches.Menu_InitializeMenu));

			Main.Helper.HarmonyHelper.EmptyMethod<ModMenus>("OnEvent");

			var args = new[] { typeof(Menu), typeof(LayoutGroup) };
			var method = typeof(ModMenu).GetMethod("Initialize", args);
			Main.Helper.HarmonyHelper.AddPrefix(method, typeof(Patches), nameof(Patches.ModMenu_Initialize));
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

		public GameObject CreateTextInput(string placeholderText, string savedValue, Menu menuTab)
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

			var placeholderTextComponent = newElement.transform.Find("HorizontalLayoutGroup/ControlBlock/InputField/Placeholder").GetComponent<Text>();
			placeholderTextComponent.text = placeholderText;

			var textTextComponent = newElement.transform.Find("HorizontalLayoutGroup/ControlBlock/InputField/Text").GetComponent<Text>();
			textTextComponent.text = savedValue;

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
