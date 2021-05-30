using UnityEngine;
using UnityEngine.UI;

namespace TestMod
{
	public interface IMenuAPI
	{
		// Title screen
		GameObject TitleScreen_MakeMenuOpenButton(string name, Menu menuToOpen);
		GameObject TitleScreen_MakeSceneLoadButton(string name, SubmitActionLoadScene.LoadableScenes sceneToLoad, PopupMenu confirmPopup = null);
		Button TitleScreen_MakeSimpleButton(string name);
		// Pause menu
		GameObject PauseMenu_MakeMenuOpenButton(string name, Menu menuToOpen, Menu customMenu = null);
		GameObject PauseMenu_MakeSceneLoadButton(string name, SubmitActionLoadScene.LoadableScenes sceneToLoad, PopupMenu confirmPopup = null, Menu customMenu = null);
		Button PauseMenu_MakeSimpleButton(string name, Menu customMenu = null);
		Menu PauseMenu_MakePauseListMenu(string title);
		// Options
		Menu OptionsMenu_MakeNonScrollingOptionsTab(string name);
		GameObject OptionsMenu_MakeTwoButtonToggle(string label, string trueText, string falseText, string tooltipText, Menu menuTab);
		// Misc
		PopupMenu MakeTwoChoicePopup(string message, string confirmText, string cancelText);
		PopupInputMenu MakeInputFieldPopup(string message, string placeholderMessage, string confirmText, string cancelText);
	}
}
