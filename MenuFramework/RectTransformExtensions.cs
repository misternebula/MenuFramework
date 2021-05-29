using UnityEngine;

namespace MenuFramework
{
	// Stolen from https://answers.unity.com/questions/888257/access-left-right-top-and-bottom-of-recttransform.html
	public static class RectTransformExtensions
	{
		public static void SetLeft(this RectTransform rt, float left) 
			=> rt.offsetMin = new Vector2(left, rt.offsetMin.y);

		public static void SetRight(this RectTransform rt, float right) 
			=> rt.offsetMax = new Vector2(-right, rt.offsetMax.y);

		public static void SetTop(this RectTransform rt, float top) 
			=> rt.offsetMax = new Vector2(rt.offsetMax.x, -top);

		public static void SetBottom(this RectTransform rt, float bottom) 
			=> rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);

		public static void SetHeight(this RectTransform rt, float height)
			=> rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);

		public static void SetPosY(this RectTransform rt, float posY)
			=> rt.anchoredPosition3D = new Vector3(rt.anchoredPosition3D.x, posY, rt.anchoredPosition3D.z);
	}
}
