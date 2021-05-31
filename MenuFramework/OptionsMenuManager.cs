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
		/*
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
	}
	class OptionsMenuManager : MonoBehaviour
	{
		public static OptionsMenuManager Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
			//Main.Helper.HarmonyHelper.AddPrefix<TabButton>("InitializeNavigation", typeof(Patches), nameof(Patches.TabButton_InitializeNavigation));
			//Main.Helper.HarmonyHelper.AddPrefix<TabbedMenu>("GetSelectOnActivate", typeof(Patches), nameof(Patches.TabbedMenu_GetSelectOnActivate));
			//Main.Helper.HarmonyHelper.AddPrefix<Menu>("GetSelectOnActivate", typeof(Patches), nameof(Patches.Menu_GetSelectOnActivate));
			//Main.Helper.HarmonyHelper.AddPrefix<Menu>("SetSelectOnActivate", typeof(Patches), nameof(Patches.Menu_SetSelectOnActivate));
			Main.Helper.HarmonyHelper.AddPrefix<Menu>("EnableMenu", typeof(Patches), nameof(Patches.Menu_EnableMenu));
			Main.Helper.HarmonyHelper.AddPrefix<TabButton>("Enable", typeof(Patches), nameof(Patches.TabButton_Enable));
			Main.Helper.HarmonyHelper.AddPrefix<TabbedMenu>("Deactivate", typeof(Patches), nameof(Patches.TabbedMenu_Deactivate));
			Main.Helper.HarmonyHelper.AddPrefix<TabbedMenu>("SelectTabButton", typeof(Patches), nameof(Patches.TabbedMenu_SelectTabButton));
			Main.Helper.HarmonyHelper.AddPrefix<MenuStackManager>("Pop", typeof(Patches), nameof(Patches.MenuStackManager_Pop));
			Main.Helper.HarmonyHelper.AddPrefix<MenuStackManager>("Push", typeof(Patches), nameof(Patches.MenuStackManager_Push));
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
			newMenu.transform.localScale = Vector3.one;

			newMenu.AddComponent<CanvasRenderer>();

			var menu = newMenu.AddComponent<Menu>();
			menu.SetValue("_addToMenuStackManager", false);
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

			newMenu.SetActive(true);

			return menu;
		}

		public GameObject CreateTwoButtonToggle(string label, string trueText, string falseText, string tooltipText, Menu menuTab)
		{
			var newElement = new GameObject($"UIElement-{label}");
			newElement.SetActive(false);
			newElement.layer = 5;

			var menuType = GetMenuType(menuTab);
			if (menuType == CurrentMenuType.None)
			{
				Main.Helper.Console.WriteLine("INCORRECT MENU FORMAT");
				return null;
			}
			ChildUIElement(newElement, menuTab, menuType);
			newElement.AddComponent<RectTransform>();
			newElement.AddComponent<CanvasRenderer>();
			var twoButton = newElement.AddComponent<TwoButtonToggleElement>();
			twoButton.SetValue("_tooltipTextType", UITextType.None);
			twoButton.SetValue("_testText", tooltipText); // Thanks Mobius! :P

			var button = newElement.GetAddComponent<Button>();

			var layout = newElement.AddComponent<LayoutElement>();
			layout.minHeight = 70;
			layout.flexibleWidth = 1;

			if (menuTab.GetSelectOnActivate() == null)
			{
				menuTab.SetSelectOnActivate(button);
			}
			newElement.SetActive(true);
			return newElement;
		}

		private void ChildUIElement(GameObject element, Menu menuTab, CurrentMenuType type)
		{
			if (type == CurrentMenuType.NonScrolling)
			{
				Main.Helper.Console.WriteLine($"Childing {element.name} to {menuTab.name}/{menuTab.transform.GetChild(0).name}");
				element.transform.parent = menuTab.transform.GetChild(0);
				return;
			}
			Main.Helper.Console.WriteLine($"Childing {element.name} to {menuTab.name}/{menuTab.transform.GetChild(0).name}/{menuTab.transform.GetChild(0).GetChild(0).name}/{menuTab.transform.GetChild(0).GetChild(0).GetChild(0).name}");
			element.transform.parent = menuTab.transform.GetChild(0).GetChild(0).GetChild(0);
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
