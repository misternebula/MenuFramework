using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MenuFramework
{
	class OptionsMenuManager : MonoBehaviour
	{
		public static OptionsMenuManager Instance { get; private set; }

		private void Awake() => Instance = this;

		public Menu MakeNonScrollingOptionsTab(string name)
		{
			Main.Helper.Console.WriteLine("making new button");
			var newButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => x.name == "Button-Graphics" && x.transform.parent.name == "TabButtons" && x.transform.parent.parent.name == "Tabs").gameObject);
			newButton.name = $"Button-{name}";
			newButton.transform.parent = GameObject.Find("OptionsCanvas").transform.Find("OptionsMenu-Panel").transform.Find("Tabs").transform.Find("TabButtons");
			newButton.transform.localScale = Vector3.one;
			
			var text = newButton.transform.Find("Text").GetComponent<Text>();
			text.text = name;
			Destroy(text.GetComponent<LocalizedText>());

			Main.Helper.Console.WriteLine("making new menu");
			var newMenu = new GameObject($"{name}Menu");
			newMenu.SetActive(false);

			Main.Helper.Console.WriteLine("setting parent");
			newMenu.transform.parent = GameObject.Find("OptionsCanvas").transform.Find("OptionsMenu-Panel").transform.Find("OptionsDisplayPanel");

			Main.Helper.Console.WriteLine("making new menu rt");
			var rectTransform = newMenu.AddComponent<RectTransform>();
			rectTransform.anchorMin = new Vector2(0, 1);
			rectTransform.anchorMax = new Vector2(1, 1);
			rectTransform.pivot = new Vector2(0.5f, 1);
			rectTransform.SetLeft(100);
			rectTransform.SetRight(100);
			rectTransform.SetHeight(665);
			rectTransform.SetPosY(-50);
			newMenu.transform.localScale = Vector3.one;

			Main.Helper.Console.WriteLine("adding canvas renderer");
			newMenu.AddComponent<CanvasRenderer>();

			var menu = newMenu.AddComponent<Menu>();
			newButton.GetComponent<TabButton>().SetValue("_tabbedMenu", menu);

			var layoutGroup = new GameObject("LayoutGroup");
			layoutGroup.transform.parent = newMenu.transform;

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

			newMenu.SetActive(true);

			return menu;
		}

		public GameObject CreateTwoButtonToggle(string label, string trueText, string falseText, string tooltipText, Menu menuTab)
		{
		}
	}
}
