using UnityEngine;

namespace MenuFramework
{
	internal class StartupPopupManager : MonoBehaviour
	{
		public static StartupPopupManager Instance { get; private set; }

		private void Awake()
			=> Instance = this;

		public void RegisterStartupPopup(string message)
		{
			Main.Helper.MenuHelper.PopupMenuManager.RegisterStartupPopup(message);
		}
	}
}
