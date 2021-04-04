using OWML.Common;
using OWML.ModHelper;
using System.Collections.Generic;

namespace MenuFramework
{
	public class Main : ModBehaviour
	{
		public static IModHelper Helper { get; private set; }

		public override object GetApi() => new MenuAPI();

		private readonly Dictionary<ButtonWithHotkeyImageElement, string> buttonDict = new Dictionary<ButtonWithHotkeyImageElement, string>();

		public void Start()
		{
			Helper = ModHelper;

			gameObject.AddComponent<TitleButtonManager>();
			gameObject.AddComponent<PopupMenuManager>();
			gameObject.AddComponent<PauseButtonManager>();
		}
	}
}
