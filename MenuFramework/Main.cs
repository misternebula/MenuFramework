using OWML.Common;
using OWML.ModHelper;

namespace MenuFramework
{
	public class Main : ModBehaviour
	{
		public static IModHelper Helper { get; private set; }

		public override object GetApi() => new MenuAPI();

		public void Start()
		{
			Helper = ModHelper;

			gameObject.AddComponent<TitleButtonManager>();
			gameObject.AddComponent<PopupMenuManager>();
			gameObject.AddComponent<PauseButtonManager>();

			LoadManager.OnCompleteSceneLoad += OnSceneLoad;
		}

		void OnSceneLoad(OWScene from, OWScene to)
		{
			if (to == OWScene.TitleScreen)
			{
				TitleButtonManager.Instance.CustomizeTitleScreen();
			}
		}
	}
}
