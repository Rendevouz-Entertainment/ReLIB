using HarmonyLib;
using KSP.Api.CoreTypes;
using KSP.Game;
using KSP.Messages;
using KSP.UI;
using KSP.UI.Binding;
using RTG;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static ReLIB.AppBar;
using Image = UnityEngine.UI.Image;
using Logger = ReLIB.logging.Logger;

namespace ReLIB
{
    public static class AppBar
    {
        private static Logger logger = new Logger(ReLIBMod.ModId,"");
        private static string OABlocation = "OAB(Clone)/HUDSpawner/HUD/widget_SideBar/widget_sidebarNav/";
        private static string FlightLocation = "GameManager/Default Game Instance(Clone)/UI Manager(Clone)/Scaled Popup Canvas/Container/ButtonBar";
        public class BarButton
        {
            public Texture2D FlightIcon { get; set; }
            public Texture2D OABIcon { get; set; }
            public string Name { get; set; }
            public string ID { get; set; }
            public Action<bool> onToggleFlight {  get; set; }
            public Action<bool> onToggleOAB { get; set; }
            public bool[] Buttons { get; set; }
            public bool createdOab = false;
            public bool createdFlight = false;
            public BarButton(Texture2D FlightIcon, Texture2D OABIcon, string Name, string ID, Action<bool> onToggleFlight, Action<bool> onToggleOAB, bool[] Buttons)
            {
                this.FlightIcon = FlightIcon;
                this.OABIcon = OABIcon;
                this.Name = Name;
                this.ID = ID;
                this.onToggleFlight = onToggleFlight;
                this.onToggleOAB = onToggleOAB;
                this.Buttons = Buttons;
            }
            public bool AddOAB()
            {
                if (GameObject.Find(OABlocation) == null)
                {
                    logger.Debug("Failed to find OAB button bar");
                    return false;
                }
                try
                {
                    GameObject OABBar = GameObject.Find(OABlocation);
                    GameObject NewOABButton = GameObject.Instantiate(OABBar.GetAllChildren()[1], OABBar.transform);
                    NewOABButton.name = $"OAB-{ID}";
                    NewOABButton.GetComponent<Image>().sprite = Sprite.Create(OABIcon, new Rect(0f, 0f, OABIcon.width, OABIcon.height), new Vector2(0.5f, 0.5f));
                    NewOABButton.GetComponent<IndicatorTag>().RegisterTag(Name);
                    NewOABButton.GetComponent<ToggleExtended>().onValueChanged.RemoveAllListeners();
                    NewOABButton.GetComponent<ToggleExtended>().onValueChanged.AddListener(delegate (bool state)
                    {
                        onToggleOAB(state);
                    });
                }
                catch (Exception e)
                {
                    logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");
                    return false;
                }
                createdOab = true;
                return true;
            }
            public IEnumerator AddFlight()
            {
                if (GameObject.Find(FlightLocation) == null)
                {
                    logger.Debug("Failed to find Flight button bar");

                }
                yield return new WaitForSeconds(1);
                try
                {
                    GameObject FlightBar = GameObject.Find(FlightLocation);
                    GameObject NewFlightButton = GameObject.Instantiate(FlightBar.GetAllChildren()[5], FlightBar.transform);
                    NewFlightButton.name = $"Flight-{ID}";
                    NewFlightButton.GetComponent<Image>().sprite = Sprite.Create(FlightIcon, new Rect(0f, 0f, FlightIcon.width, FlightIcon.height), new Vector2(0.5f, 0.5f));
                    NewFlightButton.GetChild("Content").GetChild("GRP-icon").GetChild("ICO-asset").GetComponent<Image>().sprite = Sprite.Create(FlightIcon, new Rect(0f, 0f, FlightIcon.width, FlightIcon.height), new Vector2(0.5f, 0.5f));
                    NewFlightButton.GetComponent<UIValue_WriteBool_Toggle>().BindValue(new Property<bool>(initialValue: false));
                    NewFlightButton.GetComponent<ToggleExtended>().onValueChanged.RemoveAllListeners();
                    NewFlightButton.GetComponent<ToggleExtended>().onValueChanged.AddListener(delegate (bool state)
                    {
                        onToggleFlight(state);
                    });
                    createdFlight = true;
                }
                catch (Exception e)
                {
                    logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");

                }
                

            }
        }
        public static List<BarButton> barButtons = new List<BarButton>();
        public static void RegistarEvents()
        {

        }
        public static bool Add(Texture2D FlightIcon, Texture2D OABIcon, string Name, string ID, Action<bool> onToggleFlight, Action<bool> onToggleOAB, bool[] Buttons)
        {
            barButtons.Add(new BarButton(FlightIcon, OABIcon, Name, ID, onToggleFlight, onToggleOAB, Buttons));
            return true;
        }
        public static bool Add(Texture2D FlightIcon, Texture2D OABIcon,string Name, string ID, Action<bool> onToggle, bool[] Buttons)
        {
            return Add(FlightIcon, OABIcon, Name, ID, onToggle, onToggle, Buttons);
        }
        public static bool Add(Texture2D Icon, string Name, Action<bool> onToggle, bool[] Buttons)
        {

            return Add(Icon, Icon, Name, $"UIComponent-{Guid.NewGuid()}", onToggle, onToggle, Buttons);
        }
        public static bool Add(Texture2D Icon, string Name, Action<bool> onToggleFlight, Action<bool> onToggleOAB, bool[] Buttons)
        {

            return Add(Icon, Icon, Name, $"UIComponent-{Guid.NewGuid()}", onToggleFlight, onToggleOAB, Buttons);
        }

        public static void SetValue(string ID,bool Value)
        {
            try
            {
                if (GameObject.Find($"Flight-{ID}"))
                {
                    GameObject.Find($"Flight-{ID}")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(Value);
                }
                if (GameObject.Find($"OAB-{ID}"))
                {
                    GameObject.Find($"OAB-{ID}")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(Value);
                }
            }
            catch (Exception e)
            {
                logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");
            }

        }
        public static void StateChange(MessageCenterMessage messageCenterMessage)
        {
            try
            {
                GameStateChangedMessage gameStateChangedMessage = messageCenterMessage as GameStateChangedMessage;
                GameStateConfiguration gameStateConfiguration = GameManager.Instance.Game.GlobalGameState.GetGameState();
                barButtons.ForEach(barButton =>
                {
                    logger.Log($"{barButton.ID} {gameStateConfiguration.GameState.ToString()}");
                    if (GameObject.Find($"Flight-{barButton.ID}"))
                    {

                    }
                    else
                    {
                        if (gameStateConfiguration.GameState == GameState.FlightView && barButton.Buttons[1] == true)
                        {
                            ReLIBMod.RunCr(barButton.AddFlight());
                        }
                    }
                    if (GameObject.Find($"OAB-{barButton.ID}"))
                    {

                    }
                    else
                    {
                        if (gameStateConfiguration.GameState == GameState.VehicleAssemblyBuilder && barButton.Buttons[0] == true)
                        {
                            bool runagain = barButton.AddOAB();
                        }
                    }
                });
            }
            catch (Exception e)
            {
                logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
            }
        }
    }
}
