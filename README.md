# MenuFramework

## How to use in your mod :
Create a file called IMenuAPI in your mod, and copy in these contents :
```cs
using System;
using UnityEngine;
using UnityEngine.UI;

namespace QSB.Menus;

public interface IMenuAPI
{
	GameObject TitleScreen_MakeMenuOpenButton(string name, int index, Menu menuToOpen);
	GameObject TitleScreen_MakeSceneLoadButton(string name, int index, SubmitActionLoadScene.LoadableScenes sceneToLoad, PopupMenu confirmPopup = null);
	Button TitleScreen_MakeSimpleButton(string name, int index);
	GameObject PauseMenu_MakeMenuOpenButton(string name, Menu menuToOpen, Menu customMenu = null);
	GameObject PauseMenu_MakeSceneLoadButton(string name, SubmitActionLoadScene.LoadableScenes sceneToLoad, PopupMenu confirmPopup = null, Menu customMenu = null);
	Button PauseMenu_MakeSimpleButton(string name, Menu customMenu = null);
	Menu PauseMenu_MakePauseListMenu(string title);
	PopupMenu MakeTwoChoicePopup(string message, string confirmText, string cancelText);
	PopupInputMenu MakeInputFieldPopup(string message, string placeholderMessage, string confirmText, string cancelText);
	PopupMenu MakeInfoPopup(string message, string continueButtonText);
	void RegisterStartupPopup(string message);
}
```
Then, get the API reference by doing `ModHelper.Interaction.TryGetModApi<IMenuAPI>("_nebula.MenuFramework");`

## Documentation

### Title screen only :

#### TitleScreen_MakeMenuOpenButton
Creates a button on the title screen with index `index` and text `name`, which opens the menu `menuToOpen`.

#### TitleScreen_MakeSceneLoadButton
Creates a button on the title screen with index `index` and text `name`, which loads the scene `sceneToLoad`.
When the optional parameter `confirmPopup` is given, the button will first open this popup menu. If confirm is selected, the scene will load.

#### TitleScreen_MakeSimpleButton
Creates a button on the title screen with index `index` and text `name`. You can then do whatever you like with the button by doing `(button).onClick.AddListener( ... )`.

### Pause menu only :

For the pause menu methods, `customMenu` is the menu where the button will appear. If not given, the button will appear in the normal pause menu.
This is so you can create your own custom sub-lists in the pause menu.
First create a sub-menu with `PauseMenu_MakePauseListMenu`, then create a button that opens that menu. Then add buttons to that sub-menu by giving the created menu as the `customMenu` parameter.

#### PauseMenu_MakeMenuOpenButton
Same as title screen version.

#### PauseMenu_MakeSceneLoadButton
Same as title screen version.

#### PauseMenu_MakeSimpleButton
Same as title screen version.

#### PauseMenu_MakePauseListMenu
Makes a sub-menu for the pause menu, with the name of `title`.

### General menu stuff :

#### MakeTwoChoicePopup
Makes a popup with a message and two button choices, typically YES/NO or CONFIRM/CANCEL. `message` is the main message of the popup, `confirmText` and `cancelText` are what appears on the two buttons.

#### MakeInputFieldPopup
Makes a popup with a message, a text input field, and two button choices. Same parameters as `MakeTwoChoicePopup`, with the addition of `placeholderText` as what appears in the background of the input box before you begin typing.

#### MakeInfoPopup
Makes a popup with a message and only one button to close the popup. `message` is the main message of the popup, `continueButtonText` is what appears on the button - usually just "OK".

#### RegisterStartupPopup
Registers a message that you want to appear when the game loads. **This must be run in `Start()`!**

