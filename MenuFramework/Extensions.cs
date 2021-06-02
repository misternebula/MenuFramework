using System;
using System.Reflection;
using UnityEngine;

namespace MenuFramework
{
	public static class Extensions
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

		public static void SetPosZ(this RectTransform rt, float posZ)
			=> rt.anchoredPosition3D = new Vector3(rt.anchoredPosition3D.x, rt.anchoredPosition3D.y, posZ);

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
}
