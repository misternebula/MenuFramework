using System.Linq;
using UnityEngine;

namespace MenuFramework
{
	internal class PopupMenuManager : MonoBehaviour
	{
		public static PopupMenuManager Instance { get; private set; }

		private GameObject _twoChoicePopupBase;
		private GameObject _inputPopupBase;

		private void Awake()
		{
			Instance = this;

			_inputPopupBase = Instantiate(Resources.FindObjectsOfTypeAll<PopupMenu>().First(x => x.name == "InputField-Popup" && x.transform.parent.name == "PopupCanvas").gameObject);
			DontDestroyOnLoad(_inputPopupBase);
			_inputPopupBase.SetActive(false);

			_twoChoicePopupBase = Instantiate(Resources.FindObjectsOfTypeAll<PopupMenu>().First(x => x.name == "TwoButton-Popup" && x.transform.parent.name == "PopupCanvas").gameObject);
			DontDestroyOnLoad(_twoChoicePopupBase);
			_twoChoicePopupBase.SetActive(false);
		}

		public PopupMenu CreateTwoChoicePopup(string message, string confirmText, string cancelText)
		{
			var newPopup = Instantiate(_twoChoicePopupBase);
			newPopup.transform.parent = GameObject.Find("PopupCanvas").transform;
			newPopup.transform.localPosition = Vector3.zero;
			newPopup.transform.localScale = Vector3.one;
			newPopup.GetComponentsInChildren<LocalizedText>().ToList().ForEach(x => Destroy(x));

			var popup = newPopup.GetComponent<PopupMenu>();
			popup.SetUpPopup(message, InputLibrary.menuConfirm, InputLibrary.cancel, new ScreenPrompt(confirmText), new ScreenPrompt(cancelText), true, true);
			return popup;
		}

		public PopupInputMenu CreateInputFieldPopup(string message, string placeholderMessage, string confirmText, string cancelText)
		{
			var newPopup = Instantiate(_inputPopupBase);
			newPopup.transform.parent = GameObject.Find("PopupCanvas").transform;
			newPopup.transform.localPosition = Vector3.zero;
			newPopup.transform.localScale = Vector3.one;
			newPopup.GetComponentsInChildren<LocalizedText>().ToList().ForEach(x => Destroy(x));

			var popup = newPopup.GetComponent<PopupInputMenu>();
			popup.SetUpPopup(message, InputLibrary.menuConfirm, InputLibrary.cancel, new ScreenPrompt(confirmText), new ScreenPrompt(cancelText), true, true);
			popup.SetInputFieldPlaceholderText(placeholderMessage);
			return popup;
		}
	}
}
