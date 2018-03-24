using UnityEngine;

namespace DevHelper
{
    public partial class DevHelper : MonoBehaviour
    {
        public bool autoLoadSave = true;
        public string autoLoadSaveName = "Dev";

        public bool autoLoadScene = false;
        public string autoLoadSceneName = "VAB";

        //IButton DHReloadDatabase;
        private void Awake()
        {
            Debug.Log("[DevHelper]: Injector awake");
            DontDestroyOnLoad(this);
        }

        private bool bDoOnce = true;
        private void Update()
        {
            var menu = GameObject.Find("MainMenu");
            if (menu != null && bDoOnce)
            {
                Debug.Log("[DevHelper]: Found mainmenu");
                bDoOnce = false;

                if (autoLoadSave)
                {
                    HighLogic.CurrentGame = GamePersistence.LoadGame("persistent", "Dev", true, false);
                    if (HighLogic.CurrentGame != null)
                    {
                        HighLogic.SaveFolder = autoLoadSaveName;
                        //load to scene if needed
                        if (autoLoadScene)
                        {
                            switch (autoLoadSceneName)
                            {
                                case "VAB":
                                    HighLogic.CurrentGame.startScene = GameScenes.EDITOR;
                                    break;
                                //case "SPH":
                                //    HighLogic.CurrentGame.startScene = GameScenes.SPH;
                                //    break;
                                case "Tracking Station":
                                    HighLogic.CurrentGame.startScene = GameScenes.TRACKSTATION;
                                    break;
                                case "Space Center":
                                    HighLogic.CurrentGame.startScene = GameScenes.SPACECENTER;
                                    break;
                                default:
                                    HighLogic.CurrentGame.startScene = GameScenes.SPACECENTER;
                                    break;
                            }

                        }
                        HighLogic.CurrentGame.Start();
                    }
                }
                else
                {
                    //pop up load game dialog.
                    var mc = menu.GetComponent<MainMenu>();
                    mc.continueBtn.onPressed.Invoke();
                }
            }
        }
    
        private bool isTooLateToLoad = false;

        public void OnLevelWasLoaded(int level)
        {
            Debug.Log("[DevHelper]: OnLevelWasLoaded:" + level);

            if (PSystemManager.Instance != null && ScaledSpace.Instance == null)
            {
                isTooLateToLoad = true;
            }
        }
    }
}

public class DevHelperPartlessLoader : KSP.Testing.UnitTest
{
    public DevHelperPartlessLoader()
    {
        DevHelperPluginWrapper.Initialize();
    }
}

public static class DevHelperPluginWrapper
{
    public static GameObject DevHelper;

    public static void Initialize()
    {
        if (GameObject.Find("DevHelper") == null)
        {
            DevHelper = new GameObject(
                "DevHelper",
                new [] {typeof (DevHelper.DevHelper)});
            UnityEngine.Object.DontDestroyOnLoad(DevHelper);
        }
    }
}


