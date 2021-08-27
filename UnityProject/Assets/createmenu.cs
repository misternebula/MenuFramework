using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class createmenu : MonoBehaviour 
{
	public GameObject ExistingTwoButton;
	public GameObject ExistingTabButton;

	private GameObject newTwoButton;

	void Start () 
	{
		var MenuName = "TEST";

		newTwoButton = Instantiate(ExistingTwoButton);
		DontDestroyOnLoad(newTwoButton);
		newTwoButton.SetActive(false);

		var newButton = Instantiate(ExistingTabButton);
		newButton.name = "Button-" + name;
		newButton.transform.parent = GameObject.Find("OptionsCanvas").transform.Find("OptionsMenu-Panel").transform.Find("Tabs").transform.Find("TabButtons");
		newButton.transform.localScale = Vector3.one;

		var text = newButton.transform.Find("Text").GetComponent<Text>();
		text.text = MenuName;

		var newMenu = new GameObject(MenuName+"Menu");
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

		newMenu.SetActive(true);

		var label = "THIS IS A TEST LABEL";

		var newElement = Instantiate(ExistingTwoButton);
		newElement.name = "UIElement-"+label;
		newElement.SetActive(false);
		newElement.layer = 5;

		var menuType = GetMenuType(newMenu);
		if (menuType == CurrentMenuType.None)
		{
			return;
		}

		ChildUIElement(newElement, newMenu, menuType);

		var labelText = newElement.transform.Find("HorizontalLayoutGroup/LabelBlock/HorizontalLayoutGroup/Label").GetComponent<Text>();
		labelText.text = label;

		var trueTextElement = newElement.transform.Find("HorizontalLayoutGroup/ControlBlock/Panel-LeftButton/Button-ON/Text").GetComponent<Text>();
		trueTextElement.text = "TRUE";
		var falseTextElement = newElement.transform.Find("HorizontalLayoutGroup/ControlBlock/Panel-RightButton/Button-OFF/Text").GetComponent<Text>();
		falseTextElement.text = "FALSE";

		newElement.SetActive(true);
	}

	private void ChildUIElement(GameObject element, GameObject menuTab, CurrentMenuType type)
	{
		if (type == CurrentMenuType.NonScrolling)
		{
			element.transform.parent = menuTab.transform.GetChild(0);
			element.transform.localScale = Vector3.one;
			return;
		}
		element.transform.parent = menuTab.transform.GetChild(0).GetChild(0).GetChild(0);
		element.transform.localScale = Vector3.one;
	}

	private CurrentMenuType GetMenuType(GameObject menuTab)
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

public static class Extensions
{
	public static void SetLeft(this RectTransform rt, float left)
	{
		rt.offsetMin = new Vector2(left, rt.offsetMin.y);
	}

	public static void SetRight(this RectTransform rt, float right)
	{
		rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
	}

	public static void SetTop(this RectTransform rt, float top)
	{
		rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
	}

	public static void SetBottom(this RectTransform rt, float bottom)
	{
		rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
	}

	public static void SetHeight(this RectTransform rt, float height)
	{
		rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
	}

	public static void SetPosY(this RectTransform rt, float posY)
	{
		rt.anchoredPosition3D = new Vector3(rt.anchoredPosition3D.x, posY, rt.anchoredPosition3D.z);
	}

	public static void SetPosZ(this RectTransform rt, float posZ)
	{
		rt.anchoredPosition3D = new Vector3(rt.anchoredPosition3D.x, rt.anchoredPosition3D.y, posZ);
	}

	public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
	{
		var t = obj.GetType();
		if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
		{
			throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
		}

		t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
	}
}
