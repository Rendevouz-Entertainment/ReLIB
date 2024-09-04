using Logger = ReLIB.logging.Logger;
using KSP.Game;
using KSP.Messages;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ReLIB.UI;
using HarmonyLib;
using UitkForKsp2;
using UnityEngine.UIElements;
using KSP.Modding;
using BepInEx;
using ReLIB.logging;
using System.Reflection;

namespace ReLIB;
public class CoroutineExecuter : MonoBehaviour { 
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

[BepInPlugin("com.rendevouzrs_entertainment.relib", "relib", ReLIB.ModVersion)]
[BepInDependency(UitkForKsp2.MyPluginInfo.PLUGIN_GUID, UitkForKsp2.MyPluginInfo.PLUGIN_VERSION)]
public sealed class ReLIBMod : BaseUnityPlugin
{
    public const string ModId = ReLIB.ModId;
    public const string ModVersion = ReLIB.ModVersion;
    public static bool IsDev = ReLIB.IsDev;
    public static bool Initilised = ReLIB.Initilised;
    public static Texture2D MIcon = ReLIB.MIcon;
    public static bool runALlogo = ReLIB.runALlogo;
    void Awake()
    {
        ReLIB.SoftDependancys["BepInEx"] = true;
        ReLIB.Awake();
        
    }

    //compatibility with older versions
    public static void EnableDebugMode()
    {
        ReLIB.EnableDebugMode();
    }
    public static void RunCr(IEnumerator cr)
    {
        ReLIB.RunCr(cr);
    }
}


//universal
public static class ReLIB
{
    public const string ModId = "com.rendevouzrs_entertainment.relib";
    public const string ModVersion = "1.1.0";
    public static bool IsDev = false;
    private static Logger logger = new Logger(ModId, ModVersion);
    private static CoroutineExecuter instance;
    public static bool Initilised = false;
    public static Texture2D MIcon { get; set; }
    public static bool runALlogo = false;
    public static Dictionary<string, bool> SoftDependancys = new Dictionary<string, bool>() { { "Quantum", NamespaceExists("SpaceWarp") }, { "SpaceWarp", NamespaceExists("SpaceWarp")}, { "BepInEx", false } };
    public static bool CRE = false;
    public static IEnumerator ChangeAppBar()
    {
        bool appBarStarted = false;
        while (!appBarStarted)
        {
            yield return new WaitForSeconds(5);
            try
            {
                GameManager.Instance.Game.Messages.Subscribe<GameStateChangedMessage>(AppBar.StateChange);
                logger.Log("Registar AppBar State Checker");
                appBarStarted = true;
            }
            catch (Exception e)
            {
                //logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
                appBarStarted = false;
            }
        }
    }
    public static void Awake()
    {
        EnableDebugMode();
        try
        {
            var bundle = AssetBundle.LoadFromFile($"{Paths.PluginPath}/ReLIB/assets/bundles/shadowutilitylib.bundle");
            if (!bundle)
            {
                logger.Log("Failed to load PanelSettings bundle!");
                return;
            }
            Manager.PanelSettings = bundle.LoadAllAssets<PanelSettings>()[0];
            if (!CRE)
            {
                CRE = true;
                instance = new GameObject("CoroutineExecuter").AddComponent<CoroutineExecuter>();
            }
            
        }
        catch (Exception e)
        {
            logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
        }
        LogManager.StartLogManager();
        AssetManager.LoadAllAssets();
        try
        {

            RunCr(ChangeAppBar());
        }
        catch (Exception e)
        {
            logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
        }
        try
        {
            Harmony.CreateAndPatchAll(typeof(DropdownUtils));
        }
        catch (Exception e)
        {
            logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
        }
        
        

        logger.Log($"Initialized");
        Initilised = true;
    }
    public static void EnableDebugMode()
    {
        IsDev = true;
        logger.Debug("Debug Mode Enabled");
    }
    public static void RunCr(IEnumerator cr)
    {
        try
        {
            instance.StartCoroutine(cr);
        }
        catch (Exception e)
        {
            logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
            if (!CRE)
            {
                CRE = true;
                instance = new GameObject("CoroutineExecuter").AddComponent<CoroutineExecuter>();
                RunCr(cr);
            }
        }

    }
    public static bool NamespaceExists(string desiredNamespace)//https://forum.unity.com/threads/run-bit-of-code-if-namespace-exists-c.437745/
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Namespace == desiredNamespace)
                    return true;
            }
        }
        return false;
    }
}