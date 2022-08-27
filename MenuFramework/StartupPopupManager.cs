using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MenuFramework
{
	internal class StartupPopupManager : MonoBehaviour
	{
		public static StartupPopupManager Instance { get; private set; }

		private void Awake()
			=> Instance = this;

		private void Start()
		{
			Main.Helper.HarmonyHelper.AddPrefix<TitleScreenManager>(nameof(TitleScreenManager.DetermineStartupPopups), typeof(StartupPatches), nameof(StartupPatches.DetermineStartupPopups));
			Main.Helper.HarmonyHelper.AddPrefix<TitleScreenManager>(nameof(TitleScreenManager.OnUserConfirmStartupPopup), typeof(StartupPatches), nameof(StartupPatches.OnUserConfirmStartupPopup));

			if (typeof(TitleScreenManager).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).Any(x => x.Name == "ShowStartupPopupsAndShowMenu"))
			{
				Main.Helper.HarmonyHelper.AddPrefix<TitleScreenManager>("ShowStartupPopupsAndShowMenu", typeof(StartupPatches), nameof(StartupPatches.ShowStartupPopupsAndShowMenu));
			}
			else
			{
				Main.Helper.HarmonyHelper.AddPrefix<TitleScreenManager>("TryShowStartupPopupsAndShowMenu", typeof(StartupPatches), nameof(StartupPatches.TryShowStartupPopupsAndShowMenu));
				Main.Helper.HarmonyHelper.AddPrefix<TitleScreenManager>("TryShowStartupPopups", typeof(StartupPatches), nameof(StartupPatches.TryShowStartupPopups));
			}
		}

		public int ActivePopup;

		public List<int> PopupsToShow = new List<int>();

		public List<string> Popups = new List<string>()
		{
			UITextLibrary.GetString(UITextType.MenuMessage_InputUpdate),
			UITextLibrary.GetString(UITextType.MenuMessage_ReducedFrightOptionAvail),
			UITextLibrary.GetString(UITextType.MenuMessage_NewExhibit)
		};

		public void RegisterStartupPopup(string message)
		{
			PopupsToShow.Add(Popups.Count);
			Popups.Add(message);
		}
	}

	public static class StartupPatches
	{
		public static bool DetermineStartupPopups(TitleScreenManager __instance)
		{
			if (__instance._profileManager.currentProfileGameSave.version == "NONE")
			{
				StartupPopupManager.Instance.PopupsToShow.Add(0);
			}

			var ownsDlc = EntitlementsManager.IsDlcOwned() == EntitlementsManager.AsyncOwnershipStatus.Owned;

			if (ownsDlc && (__instance._profileManager.currentProfileGameSave.shownPopups & StartupPopups.ReducedFrights) == StartupPopups.None)
			{
				StartupPopupManager.Instance.PopupsToShow.Add(1);
			}

			if (ownsDlc && (__instance._profileManager.currentProfileGameSave.shownPopups & StartupPopups.NewExhibit) == StartupPopups.None)
			{
				StartupPopupManager.Instance.PopupsToShow.Add(2);
			}

			return false;
		}

		public static bool ShowStartupPopupsAndShowMenu(TitleScreenManager __instance)
		{
			var popupManger = StartupPopupManager.Instance;

			if (popupManger.PopupsToShow.Count == 0)
			{
				__instance._okCancelPopup.ResetPopup();
				__instance.SetUpMainMenu();
				__instance.FadeInMenuOptions();
				return false;
			}

			popupManger.ActivePopup = popupManger.PopupsToShow.OrderBy(x => x).First();
			var message = popupManger.Popups[popupManger.ActivePopup];

			__instance._inputModule.EnableInputs();
			__instance._titleMenuRaycastBlocker.blocksRaycasts = false;
			__instance._okCancelPopup.ResetPopup();
			__instance._okCancelPopup.SetUpPopup(message, InputLibrary.menuConfirm, null, __instance._continuePrompt, null, true, false);
			__instance._okCancelPopup.OnPopupConfirm += __instance.OnUserConfirmStartupPopup;
			__instance._okCancelPopup.EnableMenu(true);

			return false;
		}

		public static bool OnUserConfirmStartupPopup(TitleScreenManager __instance)
		{
			var popupManger = StartupPopupManager.Instance;

			popupManger.PopupsToShow.Remove(popupManger.ActivePopup);
			
			if (popupManger.ActivePopup <= 2)
			{
				switch (popupManger.ActivePopup)
				{
					case 0:
						PlayerData.SetShownPopups(StartupPopups.ResetInputs);
						break;
					case 1:
						PlayerData.SetShownPopups(StartupPopups.ReducedFrights);
						break;
					case 2:
						PlayerData.SetShownPopups(StartupPopups.NewExhibit);
						break;
				}

				PlayerData.SaveCurrentGame();
			}

			popupManger.ActivePopup = -1;
			__instance._okCancelPopup.OnPopupConfirm -= __instance.OnUserConfirmStartupPopup;
			__instance._inputModule.DisableInputs();
			__instance._titleMenuRaycastBlocker.blocksRaycasts = true;

			var titleType = __instance.GetType();

			if (titleType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).Any(x => x.Name == "ShowStartupPopupsAndShowMenu"))
			{
				__instance.GetType().GetMethod("ShowStartupPopupsAndShowMenu", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, null);
			}
			else
			{
				__instance.GetType().GetMethod("TryShowStartupPopupsAndShowMenu", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, null);
			}

			return false;
		}

		public static bool TryShowStartupPopupsAndShowMenu(TitleScreenManager __instance)
		{
			var popupManger = StartupPopupManager.Instance;

			if (popupManger.PopupsToShow.Count == 0)
			{
				__instance._okCancelPopup.ResetPopup();
				__instance.SetUpMainMenu();
				__instance.FadeInMenuOptions();
				return false;
			}

			__instance.GetType().GetMethod("TryShowStartupPopups", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, null);

			return false;
		}

		public static bool TryShowStartupPopups(TitleScreenManager __instance)
		{
			var popupManger = StartupPopupManager.Instance;

			popupManger.ActivePopup = popupManger.PopupsToShow.First();
			var message = popupManger.Popups[popupManger.ActivePopup];

			__instance._inputModule.EnableInputs();
			__instance._titleMenuRaycastBlocker.blocksRaycasts = false;
			__instance._okCancelPopup.ResetPopup();
			__instance._okCancelPopup.SetUpPopup(message, InputLibrary.menuConfirm, null, __instance._continuePrompt, null, true, false);
			__instance._okCancelPopup.OnPopupConfirm += __instance.OnUserConfirmStartupPopup;
			__instance._okCancelPopup.EnableMenu(true);

			return false;
		}
	}
}
