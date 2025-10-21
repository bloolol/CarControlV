// Decompiled with JetBrains decompiler
// Type: CarHud
// Assembly: CarControlV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B7421D23-5D33-4568-92E3-0E28AED6BB79
// Assembly location: C:\Users\Chris-No Internet\Downloads\8108e7-CarControlV v1.2\CarControlV v1.2\CarControlV.dll

using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using NAudio.Wave;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class CarHud : Script
{
    private bool menuEnabled = false;
    private Vehicle vehicle;
    private Ped player;
    private float screenWidth = 1280f;
    private float screenHeight = 720f;
    private float menuWidth = 400f;
    private float menuHeight = 240f;
    private float buttonSize = 60f;
    private float buttonSpacing = 8f;
    private float menuX;
    private float menuY;
    private bool textureDictLoaded = false;
    private const string TextureDict = "carhud";
    private bool interiorLightOn = false;
    private bool leftIndicatorOn = false;
    private bool rightIndicatorOn = false;
    private bool showingMoreSeats = false;
    private Keys toggleKey = Keys.F6;
    //window animation
    private ScriptSettings config;
    private int gvd = ScriptSettings.Load("scripts\\CarControlV\\CarControlV.ini").GetValue<int>("SETTINGS", "GLOBAL_VOLUME_DOWN", 15);
    private WaveFileReader WavereaderDown;
    private WaveChannel32 wavechanDown;
    private DirectSoundOut DSODown;
    private float volumeDown;
    private string windowAnimDict = ScriptSettings.Load("scripts\\CarControlV\\CarControlV.ini").GetValue<string>("WINDOWS_SETTINGS", "ANIM_DICT", "amb@code_human_in_car_mp_actions@arse_pick@bodhi@rds@base");
    private string windowAnimDict2 = ScriptSettings.Load("scripts\\CarControlV\\CarControlV.ini").GetValue<string>("WINDOWS_SETTINGS", "ANIM_DICT", "amb@code_human_in_car_mp_actions@arse_pick@bodhi@rps@base");
    private string windowAnimName = ScriptSettings.Load("scripts\\CarControlV\\CarControlV.ini").GetValue<string>("WINDOWS_SETTINGS", "ANIM_NAME", "enter");
    private float windowAnimSpeed = ScriptSettings.Load("scripts\\CarControlV\\CarControlV.ini").GetValue<float>("WINDOWS_SETTINGS", "ANIM_SPEED", 24f);
    private float windowAnimStopPoint = ScriptSettings.Load("scripts\\CarControlV\\CarControlV.ini").GetValue<float>("WINDOWS_SETTINGS", "ANIM_STOP_POINT", 0.9f);
    //ignition animation
    private ScriptSettings config2 = ScriptSettings.Load("scripts\\CarControlV\\EngineControlAnim.ini");
    private WaveFileReader WavereaderDown2;
    private WaveChannel32 wavechanDown2;
    private DirectSoundOut DSODown2;
    private float volumeDown2;
    private GTA.Control VehicleExitControl = GTA.Control.VehicleExit;
    private bool toggledEnginefromMenu = false;
    private Vehicle currentVehicle;
    private Vehicle prevVehicle;
    private bool HasBooted;
    //handbrake animation
    private ScriptSettings config3 = ScriptSettings.Load("scripts\\CarControlV\\Handbrake.ini");
    private string ANIMDICT = ScriptSettings.Load("scripts\\CarControlV\\Handbrake.ini").GetValue<string>("SETTINGS", nameof(ANIMDICT), "weapon@w_sp_jerrycan");
    private string ANIMNAME = ScriptSettings.Load("scripts\\CarControlV\\Handbrake.ini").GetValue<string>("SETTINGS", nameof(ANIMNAME), "unholster");
    private float ANIMSPEED = ScriptSettings.Load("scripts\\CarControlV\\Handbrake.ini").GetValue<float>("SETTINGS", nameof(ANIMSPEED), 8f);
    private bool handOnHandbrake = false;
    private bool handbrakeKeyPressed = false;
    private bool getHashBool2 = false;
    private bool staticHandbrake = false;
    private bool brakeOn = false;
    private bool leaveHandOnBrake = false;
    private Vehicle brakedVehicle;
    private Vehicle VehicleTryingToEnter2;
    private Ped driver2;
    private int GameTimeRef4 = 0;
    private int GameTimeRef5 = 0;
    private WaveFileReader WavereaderDown3;
    private WaveChannel32 wavechanDown3;
    private DirectSoundOut DSODown3;
    private float volumeDown3;
    private uint VehHash2;
    //indicator animation
    private ScriptSettings config4 = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini");
    private ScriptSettings indicatorStat = ScriptSettings.Load("scripts\\CarControlV\\trafficControlSupport.ini");
    private bool indicatorRightWheelBack = false;
    private bool indicatorLeftWheelBack = false;
    private bool enterVehicleAgain = false;
    private bool indicatorLeft = false;
    private bool indicatorRight = false;
    private bool hazardLights = false;
    private bool indicatorSignal = false;
    private bool drawIcon = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<bool>("Settings", "ICON_DRAW", true);
    private bool PlaySounds = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<bool>("Settings", "indicator_sounds", true);
    private float TickRate = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<float>("Settings", "Indicator_Ticking_Rate", 1f);
    private int x = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<int>("Settings", nameof(x), 1250);
    private int y = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<int>("Settings", nameof(y), 391);
    private int Width = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<int>("Settings", nameof(Width), 29);
    private int Height = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<int>("Settings", nameof(Height), 29);
    private float curSteering = 0.0f;
    private bool ControllerSupport = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<bool>("Settings", "CONTROLLER_SUPPORT", true);
    private string AnimDictHeadlights = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<string>("Settings", "ANIM_DICT_HEADLIGHTS", "gestures@m@car@low@casual@ds");
    private string AnimNameHeadlights = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<string>("Settings", "ANIM_NAME_HEADLIGHTS", "gesture_me");
    private string AnimDictBlinkers = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<string>("Settings", "ANIM_DICT_BLINKERS", "veh@driveby@first_person@passenger_left_handed@smg");
    private string AnimNameBlinkers = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<string>("Settings", "ANIM_NAME_BLINKERS", "outro_0");
    private int BlinkersAnimStopAfterSeconds = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<int>("Settings", "STOP_HEADLIGHTS_ANIM_AFTER_MILISECONDS", 250);
    private string AnimDictHazzard = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<string>("Settings", "ANIM_DICT_HAZZARD", "gestures@m@sitting@generic@casual");
    private string AnimNameHazzard = ScriptSettings.Load("scripts\\CarControlV\\Indicators.ini").GetValue<string>("Settings", "ANIM_NAME_HAZZARD", "gesture_hand_right");
    private Ped driver3;
    private int GameTimeRef6 = 0;
    private int autoTurnOffLoop = 0;
    private int i2 = 0;
    private float l = 0.0f;
    private float r = 0.0f;
    private WaveFileReader WavereaderDown4;
    private WaveChannel32 wavechanDown4;
    private DirectSoundOut DSODown4;
    private float volumeDown4;
    //seatbelt animation
    private string ScriptName = "Seatbelt+Better Player Crash Damage";
    private string ScriptVer = "1.2";
    private int GameTimeRef3 = Game.GameTime;
    private int DisableControlsTime = Game.GameTime;
    private bool ControlsDisabled;
    private float OldSpeed;
    private bool UseManualSeatbelt = true;
    private bool UseAutoSeatbelt = true;
    private bool UseArmorEffects = true;
    private WaveChannel32 streaming;
    private DirectSoundOut output = (DirectSoundOut)null;
    private DirectSoundOut ChimeMP3 = (DirectSoundOut)null;
    private Mp3FileReader reader;
    private float VolumeSeatbelt;
    private float VolumeChime;
    private bool PlayChimeSound = false;
    private bool IsChimeSoundsEnabled = true;
    private bool Onstart = false;
    private bool IsSeatbeltSoundsEnabled = true;
    public CarHud()
    {
        if (Game.IsPaused)
            return;
        //Car Hud
        this.Tick += new EventHandler(this.OnTick);
        this.KeyUp += new KeyEventHandler(this.OnKeyUp);
        this.menuX = (float)(((double)this.screenWidth - (double)this.menuWidth) / 2.0);
        this.menuY = (float)((double)this.screenHeight - (double)this.menuHeight - 20.0);
        this.LoadConfig();
        //Closest Passenger Seat 
        this.Tick += new EventHandler(this.PassengerFunc);
        //Handbrake
        this.Tick += new EventHandler(this.Handbrake);
        //No Car Rolling
        this.Tick += new EventHandler(this.NoCarRolling);
        //Indicator
        this.Tick += new EventHandler(this.Indicators);
        //Seatbelt damage multiplier
        this.Tick += new EventHandler(this.Seatbelt);
        //Engine Control 
        this.Tick += new EventHandler(this.EngineAnimation);
        this.Tick += new EventHandler(this.EngineShutOff);
    }

    private void LoadConfig()
    {
        string path = "scripts\\CarControlV.ini";
        if (File.Exists(path))
        {
            foreach (string readAllLine in File.ReadAllLines(path))
            {
                Keys result;
                if (readAllLine.StartsWith("ToggleKey=") && Enum.TryParse<Keys>(readAllLine.Substring("ToggleKey=".Length).Trim(), out result))
                    this.toggleKey = result;
            }
        }
        else
            File.WriteAllText(path, "ToggleKey=F6");
    }

    private void OnTick(object sender, EventArgs e)
    {
       
        this.player = Game.Player.Character;
        this.vehicle = this.player.CurrentVehicle;
        if (!this.textureDictLoaded)
        {
            Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, (InputArgument)"carhud", (InputArgument)false);
            if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, (InputArgument)"carhud"))
            {
                this.textureDictLoaded = true;
            }
            else
            {
                Script.Wait(10);
                return;
            }
        }
        if (this.menuEnabled && (Entity)this.vehicle == (Entity)null)
            this.menuEnabled = false;
        if (!this.menuEnabled)
            return;
        Function.Call(Hash.SET_MOUSE_CURSOR_THIS_FRAME);
        Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)1, (InputArgument)true);
        Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)2, (InputArgument)true);
        Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)24, (InputArgument)true);
        Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)69, (InputArgument)true);
        Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)68, (InputArgument)true);
        Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)91, (InputArgument)true);
        Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)92, (InputArgument)true);
        this.DrawMenu();
        this.HandleMenuInteraction();
        
    }
    




    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != this.toggleKey || !((Entity)this.vehicle != (Entity)null))
            return;
        this.menuEnabled = !this.menuEnabled;
    }

    private void DrawMenu()
    {
        if (!this.textureDictLoaded)
            return;
        float num1 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, (InputArgument)0, (InputArgument)239);
        float num2 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, (InputArgument)0, (InputArgument)240);
        float screenMouseX = num1 * this.screenWidth;
        float screenMouseY = num2 * this.screenHeight;
        if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, (InputArgument)"carhud"))
            Function.Call(Hash.DRAW_SPRITE, (InputArgument)"carhud", (InputArgument)"carhud_bg", (InputArgument)((this.menuX + this.menuWidth / 2f) / this.screenWidth), (InputArgument)((this.menuY + this.menuHeight / 2f) / this.screenHeight), (InputArgument)(float)((double)this.menuWidth / (double)this.screenWidth + 0.054999999701976776), (InputArgument)(this.menuHeight / this.screenHeight), (InputArgument)0.0f, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue);
        else
            this.DrawRect((this.menuX + this.menuWidth / 2f) / this.screenWidth, (this.menuY + this.menuHeight / 2f) / this.screenHeight, (float)((double)this.menuWidth / (double)this.screenWidth + 0.054999999701976776), this.menuHeight / this.screenHeight, 0, 0, 0, 200);
        float num3 = (float)(6.0 * (double)this.buttonSize + 5.0 * (double)this.buttonSpacing);
        float num4 = (float)(3.0 * (double)this.buttonSize + 2.0 * (double)this.buttonSpacing);
        float startX = this.menuX + (float)(((double)this.menuWidth - (double)num3) / 2.0);
        float startY = this.menuY + (float)(((double)this.menuHeight - (double)num4) / 2.0);
        if (this.showingMoreSeats)
            this.DrawMoreSeatsMenu(startX, startY, screenMouseX, screenMouseY);
        else
            this.DrawRegularMenu(startX, startY, screenMouseX, screenMouseY);
    }

    private void DrawRegularMenu(float startX, float startY, float screenMouseX, float screenMouseY)
    {
        if ((Entity)this.vehicle == (Entity)null)
            return;
        int num1 = Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, (InputArgument)(Entity)this.vehicle);
        bool active1 = (Entity)this.vehicle != (Entity)null && Function.Call<bool>(Hash.GET_IS_VEHICLE_ENGINE_RUNNING, (InputArgument)(Entity)this.vehicle);
        bool isHovered1 = this.IsPointInRect(screenMouseX, screenMouseY, startX, startY, this.buttonSize, this.buttonSize);
        this.DrawButton(startX, startY, this.buttonSize, this.buttonSize, "ENGINE START STOP", active1, "engine", 0, isHovered1);
        float num2 = startX + (this.buttonSize + this.buttonSpacing);
        float num3 = this.buttonSize / 3f;
        bool isHovered2 = this.IsPointInRect(screenMouseX, screenMouseY, num2, startY, num3, this.buttonSize);
        this.DrawButton(num2, startY, num3, this.buttonSize, "Left Indicator", this.leftIndicatorOn, "indicatorLeft", -1, isHovered2);
        float num4 = num2 + num3;
        bool isHovered3 = this.IsPointInRect(screenMouseX, screenMouseY, num4, startY, num3, this.buttonSize);
        bool active2 = this.leftIndicatorOn && this.rightIndicatorOn;
        this.DrawButton(num4, startY, num3, this.buttonSize, "Both Indicators", active2, "indicatorBoth", -1, isHovered3);
        float num5 = num4 + num3;
        bool isHovered4 = this.IsPointInRect(screenMouseX, screenMouseY, num5, startY, num3, this.buttonSize);
        this.DrawButton(num5, startY, num3, this.buttonSize, "Right Indicator", this.rightIndicatorOn, "indicatorRight", -1, isHovered4);
        bool active3 = (Entity)this.vehicle != (Entity)null && (double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)4) > 0.0;
        bool isHovered5 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), startY, this.buttonSize, this.buttonSize);
        this.DrawButton(startX + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), startY, this.buttonSize, this.buttonSize, "Hood", active3, "hood", 1, isHovered5);
        bool active4 = (Entity)this.vehicle != (Entity)null && (double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)5) > 0.0;
        bool isHovered6 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), startY, this.buttonSize, this.buttonSize);
        this.DrawButton(startX + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), startY, this.buttonSize, this.buttonSize, "Trunk", active4, "trunk", 2, isHovered6);
        bool isHovered7 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), startY, this.buttonSize, this.buttonSize);
        this.DrawButton(startX + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), startY, this.buttonSize, this.buttonSize, "Interior Light", this.interiorLightOn, "interiorLight", 15, isHovered7);
        bool isHovered8 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), startY, this.buttonSize, this.buttonSize);
        this.DrawButton(startX + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), startY, this.buttonSize, this.buttonSize, "More Seats", false, "NEXT", 16, isHovered8);
        float num6 = startY + this.buttonSize + this.buttonSpacing;
        bool active5 = (Entity)this.vehicle != (Entity)null && !Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, (InputArgument)(Entity)this.vehicle, (InputArgument)0);
        bool isHovered9 = this.IsPointInRect(screenMouseX, screenMouseY, startX, num6, this.buttonSize, this.buttonSize);
        this.DrawButton(startX, num6, this.buttonSize, this.buttonSize, "Window FL", active5, "windowL1", 3, isHovered9);
        bool active6 = (Entity)this.vehicle != (Entity)null && (double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)0) > 0.0;
        bool isHovered10 = this.IsPointInRect(screenMouseX, screenMouseY, startX + this.buttonSize + this.buttonSpacing, num6, this.buttonSize, this.buttonSize);
        this.DrawButton(startX + this.buttonSize + this.buttonSpacing, num6, this.buttonSize, this.buttonSize, "Door FL", active6, "doorL1", 4, isHovered10);
        bool active7 = (Entity)this.vehicle != (Entity)null && (Entity)this.vehicle.GetPedOnSeat(VehicleSeat.Driver) == (Entity)this.player;
        bool flag1 = true;
        bool isHovered11 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num6, this.buttonSize, this.buttonSize);
        string partIcon1 = flag1 ? "seat" : "noneSeats";
        this.DrawButton(startX + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num6, this.buttonSize, this.buttonSize, "Seat Driver", active7, partIcon1, 5, isHovered11);
        bool active8 = (Entity)this.vehicle != (Entity)null && (Entity)Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, (InputArgument)(Entity)this.vehicle, (InputArgument)0) == (Entity)this.player;
        bool flag2 = num1 > 0;
        bool isHovered12 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num6, this.buttonSize, this.buttonSize);
        string partIcon2 = flag2 ? "seat" : "noneSeats";
        this.DrawButton(startX + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num6, this.buttonSize, this.buttonSize, "Seat Passenger", active8, partIcon2, 6, isHovered12);
        bool active9 = (Entity)this.vehicle != (Entity)null && (double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)1) > 0.0;
        bool isHovered13 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num6, this.buttonSize, this.buttonSize);
        this.DrawButton(startX + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num6, this.buttonSize, this.buttonSize, "Door FR", active9, "doorL2", 13, isHovered13);
        bool active10 = (Entity)this.vehicle != (Entity)null && !Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, (InputArgument)(Entity)this.vehicle, (InputArgument)1);
        bool isHovered14 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num6, this.buttonSize, this.buttonSize);
        this.DrawButton(startX + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num6, this.buttonSize, this.buttonSize, "Window FR", active10, "windowL2", 7, isHovered14);
        float num7 = num6 + this.buttonSize + this.buttonSpacing;
        bool active11 = (Entity)this.vehicle != (Entity)null && !Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, (InputArgument)(Entity)this.vehicle, (InputArgument)2);
        bool flag3 = (Entity)this.vehicle != (Entity)null && Function.Call<bool>(Hash.GET_IS_DOOR_VALID, (InputArgument)(Entity)this.vehicle, (InputArgument)2);
        bool isHovered15 = this.IsPointInRect(screenMouseX, screenMouseY, startX, num7, this.buttonSize, this.buttonSize);
        string partIcon3 = flag3 ? "windowR1" : "nonewindowR1";
        this.DrawButton(startX, num7, this.buttonSize, this.buttonSize, "Window RL", active11, partIcon3, 8, isHovered15);
        bool active12 = (Entity)this.vehicle != (Entity)null && (double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)2) > 0.0;
        bool isHovered16 = this.IsPointInRect(screenMouseX, screenMouseY, startX + this.buttonSize + this.buttonSpacing, num7, this.buttonSize, this.buttonSize);
        string partIcon4 = flag3 ? "doorR1" : "nonedoorR1";
        this.DrawButton(startX + this.buttonSize + this.buttonSpacing, num7, this.buttonSize, this.buttonSize, "Door RL", active12, partIcon4, 9, isHovered16);
        bool active13 = (Entity)this.vehicle != (Entity)null && (Entity)Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, (InputArgument)(Entity)this.vehicle, (InputArgument)1) == (Entity)this.player;
        bool flag4 = num1 > 1;
        bool isHovered17 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num7, this.buttonSize, this.buttonSize);
        string partIcon5 = flag4 ? "seat" : "noneSeats";
        this.DrawButton(startX + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num7, this.buttonSize, this.buttonSize, "Seat RL", active13, partIcon5, 10, isHovered17);
        bool active14 = (Entity)this.vehicle != (Entity)null && (Entity)Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, (InputArgument)(Entity)this.vehicle, (InputArgument)2) == (Entity)this.player;
        bool flag5 = num1 > 2;
        bool isHovered18 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num7, this.buttonSize, this.buttonSize);
        string partIcon6 = flag5 ? "seat" : "noneSeats";
        this.DrawButton(startX + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num7, this.buttonSize, this.buttonSize, "Seat RR", active14, partIcon6, 11, isHovered18);
        bool active15 = (Entity)this.vehicle != (Entity)null && (double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)3) > 0.0;
        bool flag6 = (Entity)this.vehicle != (Entity)null && Function.Call<bool>(Hash.GET_IS_DOOR_VALID, (InputArgument)(Entity)this.vehicle, (InputArgument)3);
        bool isHovered19 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num7, this.buttonSize, this.buttonSize);
        string partIcon7 = flag6 ? "doorR2" : "nonedoorR2";
        this.DrawButton(startX + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num7, this.buttonSize, this.buttonSize, "Door RR", active15, partIcon7, 14, isHovered19);
        bool active16 = (Entity)this.vehicle != (Entity)null && !Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, (InputArgument)(Entity)this.vehicle, (InputArgument)3);
        bool isHovered20 = this.IsPointInRect(screenMouseX, screenMouseY, startX + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num7, this.buttonSize, this.buttonSize);
        string partIcon8 = flag6 ? "windowR2" : "nonewindowR2";
        this.DrawButton(startX + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num7, this.buttonSize, this.buttonSize, "Window RR", active16, partIcon8, 12, isHovered20);
            }

    private void DrawMoreSeatsMenu(
      float startX,
      float startY,
      float screenMouseX,
      float screenMouseY)
    {
        if ((Entity)this.vehicle == (Entity)null)
            return;
        int num1 = Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, (InputArgument)(Entity)this.vehicle);
        bool flag1 = num1 > 2;
        float num2 = startX;
        float num3 = startY + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing));
        bool isHovered1 = this.IsPointInRect(screenMouseX, screenMouseY, num2, num3, this.buttonSize, this.buttonSize);
        this.DrawButton(num2, num3, this.buttonSize, this.buttonSize, "Back", false, "BACK", -1, isHovered1);
        bool active17 = (Entity)this.vehicle != (Entity)null && !Game.Player.Character.CanFlyThroughWindscreen;
        bool isHovered21 = this.IsPointInRect(screenMouseX, screenMouseY, num2 + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num3, this.buttonSize, this.buttonSize);
        string partIcon9 = "seatbelt";
        this.DrawButton(num2 + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), num3, this.buttonSize, this.buttonSize, "Seatbelt", active17, partIcon9, 17, isHovered21);

        for (int index = 0; index < 12; ++index)
        {
            int buttonId = index + 3;
            bool flag2 = flag1 && buttonId < num1;
            bool active = (Entity)this.vehicle != (Entity)null && (Entity)Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, (InputArgument)(Entity)this.vehicle, (InputArgument)buttonId) == (Entity)this.player && (Game.Player.Character.SeatIndex != VehicleSeat.Driver || Game.Player.Character.SeatIndex != VehicleSeat.Passenger);
            string label = "Seat " + (buttonId + 2).ToString();
            int num4 = index / 6;
            int num5 = index % 6;
            float num6 = startX + (float)num5 * (this.buttonSize + this.buttonSpacing);
            float num7 = startY + (float)num4 * (this.buttonSize + this.buttonSpacing);
            bool isHovered2 = this.IsPointInRect(screenMouseX, screenMouseY, num6, num7, this.buttonSize, this.buttonSize);
            string partIcon = flag2 ? "seat" : "noneSeats";
            this.DrawButton(num6, num7, this.buttonSize, this.buttonSize, label, active, partIcon, buttonId, isHovered2);
        }
    }

    private void DrawButton(
      float x,
      float y,
      float width,
      float height,
      string label,
      bool active,
      string partIcon,
      int buttonId,
      bool isHovered)
    {
        float num1 = x / this.screenWidth;
        float num2 = y / this.screenHeight;
        float num3 = width / this.screenWidth;
        float num4 = height / this.screenHeight;
        Function.Call(Hash.DRAW_SPRITE, (InputArgument)"carhud", (InputArgument)(isHovered ? "buttonHover" : (active ? "buttonon" : "buttonoff")), (InputArgument)(num1 + num3 / 2f), (InputArgument)(num2 + num4 / 2f), (InputArgument)num3, (InputArgument)num4, (InputArgument)0.0f, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue);
        if (string.IsNullOrEmpty(partIcon))
            return;
        if (partIcon == "NEXT" || partIcon == "BACK")
        {
            Function.Call(Hash.SET_TEXT_FONT, (InputArgument)0);
            Function.Call(Hash.SET_TEXT_SCALE, (InputArgument)0.3f, (InputArgument)0.3f);
            Function.Call(Hash.SET_TEXT_COLOUR, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue);
            Function.Call(Hash.SET_TEXT_CENTRE, (InputArgument)true);
            Function.Call(Hash.SET_TEXT_OUTLINE);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, (InputArgument)"STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (InputArgument)partIcon);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, (InputArgument)(num1 + num3 / 2f), (InputArgument)(float)((double)num2 + (double)num4 / 2.0 - 0.014999999664723873));
        }
        else
            Function.Call(Hash.DRAW_SPRITE, (InputArgument)"carhud", (InputArgument)partIcon, (InputArgument)(num1 + num3 / 2f), (InputArgument)(num2 + num4 / 2f), (InputArgument)(num3 * 0.8f), (InputArgument)(num4 * 0.65f), (InputArgument)0.0f, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue);
    }

    private void DrawSeatButton(
      float x,
      float y,
      float width,
      float height,
      string label,
      bool active,
      string partIcon,
      int seatIndex,
      bool isHovered,
      bool seatExists)
    {
        float num1 = x / this.screenWidth;
        float num2 = y / this.screenHeight;
        float num3 = width / this.screenWidth;
        float num4 = height / this.screenHeight;
        Function.Call(Hash.DRAW_SPRITE, (InputArgument)"carhud", (InputArgument)(seatExists ? (!isHovered ? (active ? "buttonon" : "buttonoff") : "buttonHover") : "buttoninactive"), (InputArgument)(num1 + num3 / 2f), (InputArgument)(num2 + num4 / 2f), (InputArgument)num3, (InputArgument)num4, (InputArgument)0.0f, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue);
        if (string.IsNullOrEmpty(partIcon))
            return;
        string str = partIcon;
        if (partIcon == "noneSeats" && !Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, (InputArgument)"carhud"))
            str = "buttonoff";
        Function.Call(Hash.DRAW_SPRITE, (InputArgument)"carhud", (InputArgument)str, (InputArgument)(num1 + num3 / 2f), (InputArgument)(num2 + num4 / 2f), (InputArgument)(num3 * 0.8f), (InputArgument)(num4 * 0.65f), (InputArgument)0.0f, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue, (InputArgument)(int)byte.MaxValue);
    }

    private void HandleMenuInteraction()
    {
        if (!this.menuEnabled || (Entity)this.vehicle == (Entity)null)
            return;
        float num1 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, (InputArgument)0, (InputArgument)239);
        float num2 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, (InputArgument)0, (InputArgument)240);
        float x = num1 * this.screenWidth;
        float y = num2 * this.screenHeight;
        float num3 = (float)(6.0 * (double)this.buttonSize + 5.0 * (double)this.buttonSpacing);
        float num4 = (float)(3.0 * (double)this.buttonSize + 2.0 * (double)this.buttonSpacing);
        float rectX1 = this.menuX + (float)(((double)this.menuWidth - (double)num3) / 2.0);
        float rectY1 = this.menuY + (float)(((double)this.menuHeight - (double)num4) / 2.0);
        if (!Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, (InputArgument)0, (InputArgument)237))
            return;
        float num5 = this.menuX + this.menuWidth;
        float num6 = this.menuY + this.menuHeight;
        if ((double)x < (double)this.menuX || (double)x > (double)num5 || (double)y < (double)this.menuY || (double)y > (double)num6)
            this.menuEnabled = false;
        else if (this.showingMoreSeats)
        {
            float rectX2 = rectX1;
            float rectY2 = rectY1 + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing));
            if (this.IsPointInRect(x, y, rectX2, rectY2, this.buttonSize, this.buttonSize))
            {
                this.showingMoreSeats = false;
            }
            else
            {
                int num7 = Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, (InputArgument)(Entity)this.vehicle);
                for (int index = 0; index < 12; ++index)
                {
                    int num8 = index / 6;
                    int num9 = index % 6;
                    float rectX3 = rectX1 + (float)num9 * (this.buttonSize + this.buttonSpacing);
                    float rectY3 = rectY1 + (float)num8 * (this.buttonSize + this.buttonSpacing);
                    if (this.IsPointInRect(x, y, rectX3, rectY3, this.buttonSize, this.buttonSize))
                    {
                        int seatIndex = index + 3;
                        if (seatIndex < num7)
                        {
                            this.SwitchSeat(seatIndex);
                            break;
                        }
                        break;
                    }
                }
            }

            if (this.IsPointInRect(x,y, rectX2 + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY2, this.buttonSize, this.buttonSize))
            {
                this.HandleManualSeatbelt();
                
            }
        }
        else
        {
            float rectX4 = rectX1 + (this.buttonSize + this.buttonSpacing);
            float rectWidth = this.buttonSize / 3f;
            float rectX5 = rectX4 + rectWidth;
            float rectX6 = rectX5 + rectWidth;
            if (this.IsPointInRect(x, y, rectX4, rectY1, rectWidth, this.buttonSize))
            {
                this.ToggleLeftIndicator();
                indicatorAnim();
            }
            else if (this.IsPointInRect(x, y, rectX5, rectY1, rectWidth, this.buttonSize))
                this.ToggleBothIndicators();
            else if (this.IsPointInRect(x, y, rectX6, rectY1, rectWidth, this.buttonSize))
            {
                this.ToggleRightIndicator();
                indicatorAnim();
            }
            else if (this.IsPointInRect(x, y, rectX1, rectY1, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.Driver)
                this.ToggleEngine();
            else if (this.IsPointInRect(x, y, rectX1 + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY1, this.buttonSize, this.buttonSize))
                this.ToggleHood();
            else if (this.IsPointInRect(x, y, rectX1 + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY1, this.buttonSize, this.buttonSize))
                this.ToggleTrunk();
            else if (this.IsPointInRect(x, y, rectX1 + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY1, this.buttonSize, this.buttonSize))
                this.ToggleInteriorLight();
            else if (this.IsPointInRect(x, y, rectX1 + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY1, this.buttonSize, this.buttonSize))
                this.showingMoreSeats = true;
            float rectY4 = rectY1 + this.buttonSize + this.buttonSpacing;
            if (this.IsPointInRect(x, y, rectX1, rectY4, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.Driver)
            {
                this.ToggleWindow(0);
                windowToggleAnim(Game.Player.Character);
            }
            else if (this.IsPointInRect(x, y, rectX1 + this.buttonSize + this.buttonSpacing, rectY4, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.Driver)
            {
                this.ToggleDoor(0);
                doorToggleAnim(Game.Player.Character);
            }
            else if (this.IsPointInRect(x, y, rectX1 + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY4, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.Passenger)
                this.SwitchSeat2();
            else if (this.IsPointInRect(x, y, rectX1 + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY4, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.Driver)
                this.SwitchSeat2();
            else if (this.IsPointInRect(x, y, rectX1 + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY4, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.Passenger)
                this.ToggleDoor(1);
            else if (this.IsPointInRect(x, y, rectX1 + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY4, this.buttonSize, this.buttonSize) && (Game.Player.Character.SeatIndex == VehicleSeat.Passenger || Game.Player.Character.SeatIndex == VehicleSeat.Driver))
            {
                this.ToggleWindow(1);
                if (Game.Player.Character.SeatIndex == VehicleSeat.Passenger)
                    windowToggleAnimPassengerSide(Game.Player.Character);
                else
                    windowToggleAnim(Game.Player.Character);
            }
            float rectY5 = rectY4 + this.buttonSize + this.buttonSpacing;
            if (this.IsPointInRect(x, y, rectX1, rectY5, this.buttonSize, this.buttonSize) && (Game.Player.Character.SeatIndex == VehicleSeat.RightRear || Game.Player.Character.SeatIndex == VehicleSeat.Driver))
            {
                this.ToggleWindow(2);
                if (Game.Player.Character.SeatIndex == VehicleSeat.RightRear)
                windowToggleAnimPassengerSide(Game.Player.Character);
                else
                    windowToggleAnim(Game.Player.Character);
            }
            else if (this.IsPointInRect(x, y, rectX1 + this.buttonSize + this.buttonSpacing, rectY5, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.RightRear)
                this.ToggleDoor(2);
            else if (this.IsPointInRect(x, y, rectX1 + (float)(2.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY5, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.RightRear)
                this.SwitchSeat2();
            else if (this.IsPointInRect(x, y, rectX1 + (float)(3.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY5, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.LeftRear)
                this.SwitchSeat2();
            else if (this.IsPointInRect(x, y, rectX1 + (float)(4.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY5, this.buttonSize, this.buttonSize) && Game.Player.Character.SeatIndex == VehicleSeat.LeftRear)
                this.ToggleDoor(3);
            else if (this.IsPointInRect(x, y, rectX1 + (float)(5.0 * ((double)this.buttonSize + (double)this.buttonSpacing)), rectY5, this.buttonSize, this.buttonSize) && (Game.Player.Character.SeatIndex == VehicleSeat.LeftRear || Game.Player.Character.SeatIndex == VehicleSeat.Driver))
            {
                this.ToggleWindow(3);

                windowToggleAnim(Game.Player.Character);
            }

        }
    }

    private void DrawRect(
      float x,
      float y,
      float width,
      float height,
      int r,
      int g,
      int b,
      int a)
    {
        Function.Call(Hash.DRAW_RECT, (InputArgument)x, (InputArgument)y, (InputArgument)width, (InputArgument)height, (InputArgument)r, (InputArgument)g, (InputArgument)b, (InputArgument)a);
    }

    private bool IsPointInRect(
      float x,
      float y,
      float rectX,
      float rectY,
      float rectWidth,
      float rectHeight)
    {
        return (double)x >= (double)rectX && (double)x <= (double)rectX + (double)rectWidth && (double)y >= (double)rectY && (double)y <= (double)rectY + (double)rectHeight;
    }

    private void ToggleEngine()
    {
        if (!((Entity)Game.Player.Character.CurrentVehicle != (Entity)null))
        {
            toggledEnginefromMenu = false;
            return;
        }

        if (Function.Call<bool>(Hash.GET_IS_VEHICLE_ENGINE_RUNNING, (InputArgument)(Entity)Game.Player.Character.CurrentVehicle))
        {
            Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)75, (InputArgument)true);
            Game.DisableControlThisFrame(GTA.Control.VehicleExit);
            /*
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, (InputArgument)(Entity)Game.Player.Character.CurrentVehicle, (InputArgument)false, (InputArgument)false, (InputArgument)true);
            Game.Player.Character.CurrentVehicle.IsEngineRunning = false;
            Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, (InputArgument)(Entity)this.vehicle, (InputArgument)true);
            */
            this.IgnitionKeyRemoveFX();
         
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, new InputArgument[4]
            {
            (InputArgument) Game.Player.Character.CurrentVehicle,
            (InputArgument) false,
            (InputArgument) false,
            (InputArgument) false
            });
            Game.Player.Character.CurrentVehicle.IsEngineRunning = false;
            Game.Player.Character.Task.ClearAll();
            Script.Wait(1000);
            Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, new InputArgument[2]
            {
            (InputArgument) Game.Player.Character.CurrentVehicle,
            (InputArgument) true
            });
            toggledEnginefromMenu = false;
            HasBooted = false;
        }
        else
        {
            Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)75, (InputArgument)true);
            Game.DisableControlThisFrame(GTA.Control.VehicleExit);
            /*
            Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, (InputArgument)(Entity)this.vehicle, (InputArgument)false);
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, (InputArgument)(Entity)Game.Player.Character.CurrentVehicle, (InputArgument)true, (InputArgument)false, (InputArgument)false);
            Game.Player.Character.CurrentVehicle.IsEngineRunning = true;
            */
            //Game.Player.Character.CurrentVehicle.CanEngineDegrade = false;

            Game.Player.Character.Task.ClearAll();
            Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, new InputArgument[2]
            {
          (InputArgument) Game.Player.Character.CurrentVehicle,
          (InputArgument) false
            });
            Script.Wait(500);
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, new InputArgument[4]
            {
          (InputArgument) Game.Player.Character.CurrentVehicle,
          (InputArgument) true,
          (InputArgument) false,
          (InputArgument) true,
            });
            Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, new InputArgument[2]
            {
          (InputArgument) Game.Player.Character.CurrentVehicle,
          (InputArgument) true
            });
            Script.Wait(50);
            Game.Player.Character.Task.ClearAll();
            this.IgnitionKeyInsertFX();
            Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, new InputArgument[2]
            {
          (InputArgument) Game.Player.Character.CurrentVehicle,
          (InputArgument) false
            });
            toggledEnginefromMenu = true;
            HasBooted = true;
        }
        
    }

    private void ToggleHood()
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        if ((double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)4) == 0.0)
            Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, (InputArgument)(Entity)this.vehicle, (InputArgument)4, (InputArgument)false, (InputArgument)false);
        else
            Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, (InputArgument)(Entity)this.vehicle, (InputArgument)4, (InputArgument)false);
    }

    private void ToggleTrunk()
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        if ((double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)5) == 0.0)
            Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, (InputArgument)(Entity)this.vehicle, (InputArgument)5, (InputArgument)false, (InputArgument)false);
        else
            Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, (InputArgument)(Entity)this.vehicle, (InputArgument)5, (InputArgument)false);
    }

    private void ToggleDoor(int doorIndex)
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        if ((double)Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, (InputArgument)(Entity)this.vehicle, (InputArgument)doorIndex) == 0.0)
            Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, (InputArgument)(Entity)this.vehicle, (InputArgument)doorIndex, (InputArgument)false, (InputArgument)false);
        else
            Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, (InputArgument)(Entity)this.vehicle, (InputArgument)doorIndex, (InputArgument)false);
    }

    private void ToggleWindow(int windowIndex)
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, (InputArgument)(Entity)this.vehicle, (InputArgument)windowIndex))
        {
            Function.Call(Hash.ROLL_DOWN_WINDOW, (InputArgument)(Entity)this.vehicle, (InputArgument)windowIndex);
            WindowSoundFX();

        }
        else
        {
            Function.Call(Hash.ROLL_UP_WINDOW, (InputArgument)(Entity)this.vehicle, (InputArgument)windowIndex);
            WindowSoundFX();
        }
    }

    private void SwitchSeat(int seatIndex)
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        Function.Call(Hash.SET_PED_INTO_VEHICLE, (InputArgument)(Entity)this.player, (InputArgument)(Entity)this.vehicle, (InputArgument)seatIndex);
    }

    private void SwitchSeat2()
    {
        Function.Call(Hash.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT, new InputArgument[2]
            {
      (InputArgument) (Entity) Game.Player.Character,
      (InputArgument) (Entity) Game.Player.Character.CurrentVehicle
            });

    }

    private void ToggleInteriorLight()
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        headlightsAnim();
        this.interiorLightOn = !this.interiorLightOn;
        Function.Call(Hash.SET_VEHICLE_INTERIORLIGHT, (InputArgument)this.vehicle.Handle, (InputArgument)this.interiorLightOn);
        if (interiorLightOn == true)
            IndicatorSoundEffects();
        else
            IndicatorOffSoundEffects();

    }

    private void ToggleLeftIndicator()
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        if (this.rightIndicatorOn)
        {
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)0, (InputArgument)false);
            this.rightIndicatorOn = false;
            //this.IndicatorOffSoundEffects();
        }
        if (this.leftIndicatorOn)
        {
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)1, (InputArgument)false);
            this.leftIndicatorOn = false;
            indicatorSignal = false;
            indicatorAnim();
            this.IndicatorOffSoundEffects();
        }
        else
        {
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)1, (InputArgument)true);
            this.leftIndicatorOn = true;
            indicatorAnim();
            indicatorSignal = true;
            this.IndicatorSoundEffects();
            
        }
    }

    private void ToggleRightIndicator()
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        if (this.leftIndicatorOn)
        {
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)1, (InputArgument)false);
            this.leftIndicatorOn = false;
            //this.IndicatorOffSoundEffects();
            
        }
        if (this.rightIndicatorOn)
        {
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)0, (InputArgument)false);
            this.rightIndicatorOn = false;
            indicatorAnim();
            this.IndicatorOffSoundEffects();
            indicatorSignal = false;
        }
        else
        {
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)0, (InputArgument)true);
            this.rightIndicatorOn = true;
            indicatorAnim();
            this.IndicatorSoundEffects();
            indicatorSignal = true;

        }
    }

    private void ToggleBothIndicators()
    {
        if (!((Entity)this.vehicle != (Entity)null))
            return;
        if (this.leftIndicatorOn && this.rightIndicatorOn)
        {
            indicatorAnim();
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)0, (InputArgument)false);
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)1, (InputArgument)false);
            this.leftIndicatorOn = false;
            this.rightIndicatorOn = false;
            this.IndicatorOffSoundEffects();
            indicatorSignal = false;
        }
        else
        {
            indicatorAnim();
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)0, (InputArgument)true);
            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, (InputArgument)(Entity)this.vehicle, (InputArgument)1, (InputArgument)true);
            this.leftIndicatorOn = true;
            this.rightIndicatorOn = true;
            this.IndicatorSoundEffects();
            indicatorSignal = true;
        }
    }
    //Closest Passenger Seat 
    private void PassengerFunc(object sender, EventArgs e)
    {
        if (!Game.IsControlJustPressed(GTA.Control.Enter))
            return;
        if (Function.Call<bool>(Hash.IS_PED_IN_ANY_VEHICLE, new InputArgument[2]
        {
      (InputArgument) (Entity) Game.Player.Character,
      (InputArgument) false
        }))
            return;
        if (Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, new InputArgument[1]
        {
      (InputArgument) (Entity) Game.Player.Character
        }))
        {
            Function.Call(Hash.CLEAR_PED_TASKS, new InputArgument[1]
            {
        (InputArgument) (Entity) Game.Player.Character
            });
        }
        else
        {
            Vehicle closestVehicle = this.GetClosestVehicle(10f);
            if ((Entity)closestVehicle != (Entity)null)
            {
                bool flag = Function.Call<bool>(Hash.IS_PED_RUNNING, new InputArgument[1]
                {
          (InputArgument) (Entity) Game.Player.Character
                });
                int closestVehicleSeat = this.GetClosestVehicleSeat(closestVehicle);
                if (closestVehicleSeat == -3 || closestVehicleSeat == -1)
                    return;
                if (flag)
                {
                    Function.Call(Hash.TASK_ENTER_VEHICLE, new InputArgument[7]
                    {
            (InputArgument) (Entity) Game.Player.Character,
            (InputArgument) (Entity) closestVehicle,
            (InputArgument) 9000,
            (InputArgument) closestVehicleSeat,
            (InputArgument) 2f,
            (InputArgument) 1,
            (InputArgument) 0
                    });
                    Script.Wait(4000);
                    if (!Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, new InputArgument[3]
                    {
            (InputArgument) (Entity) Game.Player.Character,
            (InputArgument) (Entity) closestVehicle,
            (InputArgument) false
                    }))
                        Function.Call(Hash.CLEAR_PED_TASKS, new InputArgument[1]
                        {
              (InputArgument) (Entity) Game.Player.Character
                        });
                }
                else
                {
                    Function.Call(Hash.TASK_ENTER_VEHICLE, new InputArgument[7]
                    {
            (InputArgument) (Entity) Game.Player.Character,
            (InputArgument) (Entity) closestVehicle,
            (InputArgument) 9000,
            (InputArgument) closestVehicleSeat,
            (InputArgument) 1f,
            (InputArgument) 1,
            (InputArgument) 0
                    });
                    Script.Wait(4000);
                    if (!Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, new InputArgument[3]
                    {
            (InputArgument) (Entity) Game.Player.Character,
            (InputArgument) (Entity) closestVehicle,
            (InputArgument) false
                    }))
                        Function.Call(Hash.CLEAR_PED_TASKS, new InputArgument[1]
                        {
              (InputArgument) (Entity) Game.Player.Character
                        });
                }
            }
        }
    }

    private Vehicle GetClosestVehicle(float dist)
    {
        Vehicle[] allVehicles = World.GetAllVehicles();
        float num1 = dist + 100f;
        Vehicle vehicle1 = (Vehicle)null;
        foreach (Vehicle vehicle2 in allVehicles)
        {
            float num2 = Vector3.Distance(Game.Player.Character.Position, vehicle2.Position);
            if ((double)num2 < (double)num1)
            {
                vehicle1 = vehicle2;
                num1 = num2;
            }
        }
        return (double)num1 <= (double)dist ? vehicle1 : (Vehicle)null;
    }
    private int GetClosestVehicleSeat(Vehicle v)
    {
        string[] strArray = new string[6]
        {
      "seat_dside_f",
      "seat_pside_f",
      "seat_dside_r",
      "seat_pside_r",
      "seat_dside_r1",
      "seat_pside_r1"
        };
        string str1 = "";
        float num1 = 100f;
        foreach (string str2 in strArray)
        {
            int num2 = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, new InputArgument[2]
            {
        (InputArgument) (Entity) v,
        (InputArgument) str2
            });
            if (num2 != -1)
            {
                float num3 = Vector3.Distance(Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, new InputArgument[2]
                {
          (InputArgument) (Entity) v,
          (InputArgument) num2
                }), Game.Player.Character.Position);
                if ((double)num3 < (double)num1)
                {
                    num1 = num3;
                    str1 = str2;
                }
            }
        }
        if ((double)num1 <= 10.0)
        {
            switch (str1)
            {
                case "seat_dside_f":
                    return -1;
                case "seat_pside_f":
                    return 0;
                case "seat_pside_r":
                    return 2;
                case "seat_dside_r":
                    return 1;
                case "seat_dside_r1":
                    return 3;
                case "seat_pside_r1":
                    return 4;
            }
        }
        return -3;
    }

    //Car Rolling Prevention Segment
    private void NoCarRolling(Object Sender, EventArgs e)
    {
        if (!Game.Player.Character.Exists() || !Game.Player.Character.IsInVehicle())
            return;
        Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
        if (Game.Player.Character.IsInFlyingVehicle || Game.Player.Character.IsOnBike)
            return;
        float num = Function.Call<float>(Hash.GET_ENTITY_ROLL, new InputArgument[1]
        {
      (InputArgument) (Entity) Game.Player.Character
        });
        if ((double)num > 90.9 || (double)num < -90.9)
            Function.Call(Hash.SET_VEHICLE_OUT_OF_CONTROL, new InputArgument[3]
            {
        (InputArgument) (Entity) currentVehicle,
        (InputArgument) false,
        (InputArgument) false
            });
    }
    //Door Animation Segment
    private void doorToggleAnim(Ped ped)
    {
        if (Function.Call<bool>(Hash.IS_VEHICLE_DOOR_FULLY_OPEN, vehicle, 0))
        {
            if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
            {
        (InputArgument) ped,
        (InputArgument) "anim@veh@std@ps@enter_exit",
        (InputArgument) "door_dside_open",
        (InputArgument) 3
            }))
                ped.Task.PlayAnimation(this.windowAnimDict, this.windowAnimName, this.windowAnimSpeed, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation);
            else if ((double)Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, new InputArgument[3]
            {
        (InputArgument) ped,
        (InputArgument) "anim@veh@std@ps@enter_exit",
        (InputArgument) "door_dside_open"
            }) > (double)this.windowAnimStopPoint)
            {
                Function.Call(Hash.STOP_ENTITY_ANIM, new InputArgument[4]
                {
          (InputArgument) ped,
          (InputArgument) "anim@veh@std@ps@enter_exit",
          (InputArgument) "door_dside_open",
          (InputArgument) 3f
                });

            }
        }
        else if ((!Function.Call<bool>(Hash.IS_VEHICLE_DOOR_FULLY_OPEN, vehicle, 0)))
        {
            if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                {
        (InputArgument) ped,
        (InputArgument) "anim@veh@std@ps@enter_exit",
        (InputArgument) "door_dside_opem",
        (InputArgument) 3
                }))
                ped.Task.PlayAnimation(this.windowAnimDict, this.windowAnimName, this.windowAnimSpeed, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation);
            else if ((double)Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, new InputArgument[3]
            {
        (InputArgument) ped,
        (InputArgument) "anim@veh@std@ps@enter_exit",
        (InputArgument) "door_dside_open"
            }) > (double)this.windowAnimStopPoint)
            {
                Function.Call(Hash.STOP_ENTITY_ANIM, new InputArgument[4]
                {
          (InputArgument) ped,
          (InputArgument) "anim@veh@std@ps@enter_exit",
          (InputArgument) "door_dside_open",
          (InputArgument) 3f
                });

            }





        }
    }



    // Window Animation Segment
    private void windowToggleAnim(Ped ped)
    {
        if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
        {
        (InputArgument) ped,
        (InputArgument) this.windowAnimDict,
        (InputArgument) this.windowAnimName,
        (InputArgument) 3
        }))
            ped.Task.PlayAnimation(this.windowAnimDict, this.windowAnimName, this.windowAnimSpeed, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation);
        else if ((double)Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, new InputArgument[3]
        {
        (InputArgument) ped,
        (InputArgument) this.windowAnimDict,
        (InputArgument) this.windowAnimName
        }) > (double)this.windowAnimStopPoint)
        {
            Function.Call(Hash.STOP_ENTITY_ANIM, new InputArgument[4]
            {
          (InputArgument) ped,
          (InputArgument) this.windowAnimName,
          (InputArgument) this.windowAnimDict,
          (InputArgument) 3f
            });

        }
    }

    private void windowToggleAnimPassengerSide(Ped ped)
    {
        if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
        {
        (InputArgument) ped,
        (InputArgument) this.windowAnimDict2,
        (InputArgument) this.windowAnimName,
        (InputArgument) 3
        }))
            ped.Task.PlayAnimation(this.windowAnimDict2, this.windowAnimName, this.windowAnimSpeed, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation);
        else if ((double)Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, new InputArgument[3]
        {
        (InputArgument) ped,
        (InputArgument) this.windowAnimDict,
        (InputArgument) this.windowAnimName
        }) > (double)this.windowAnimStopPoint)
        {
            Function.Call(Hash.STOP_ENTITY_ANIM, new InputArgument[4]
            {
          (InputArgument) ped,
          (InputArgument) this.windowAnimName,
          (InputArgument) this.windowAnimDict2,
          (InputArgument) 3f
            });

        }
    }
    private void WindowSoundFX()
    {
        if (File.Exists("scripts\\CarControlV\\window.wav"))
        {
            this.WavereaderDown = new WaveFileReader("scripts\\CarControlV\\window.wav");
            this.wavechanDown = new WaveChannel32((WaveStream)this.WavereaderDown);
            this.volumeDown = (float)this.gvd;
            this.DSODown = new DirectSoundOut();
            this.DSODown.Init((IWaveProvider)this.wavechanDown);
            this.wavechanDown.Volume = this.volumeDown / 100f;
            this.DSODown.Play();
            this.DSODown.Dispose();
        }
        else
            GTA.UI.Screen.ShowSubtitle("window.wav is not found!");
    }

    //Engine Animation Segment
    private void EngineShutOff(object sender, EventArgs e)
    {

        if (Game.IsEnabledControlJustReleased(VehicleExitControl))
        {
            if (Game.Player.Character.IsInVehicle())
            {

                if (Game.Player.Character.SeatIndex == VehicleSeat.Driver)
                {
                    Vehicle vehicle = Game.Player.Character.CurrentVehicle;


                    //backwards logic but it works lol
                    if ((IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE_OPENING_DOOR_EXITING) || (IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE))) && toggledEnginefromMenu)
                    {
                        vehicle.IsEngineRunning = true;

                    }
                    else if ((IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE_OPENING_DOOR_EXITING) || (IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE))) && !toggledEnginefromMenu)
                    {
                        vehicle.IsEngineRunning = false;
                    }

                    //vector position auto shutoff func
                }
            }

        }

    }

    private void EngineShutOff2(object sender, EventArgs e)
    {

        if (Game.IsEnabledControlJustReleased(VehicleExitControl))
        {
            if (Game.Player.Character.IsInVehicle())
            {

                if (Game.Player.Character.SeatIndex == VehicleSeat.Driver)
                {
                    Vehicle vehicle = Game.Player.Character.CurrentVehicle;


                    //backwards logic but it works lol
                    if ((IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE_OPENING_DOOR_EXITING) || (IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE))) && toggledEnginefromMenu)
                    {
                        vehicle.IsEngineRunning = true;

                    }
                    else if ((IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE_OPENING_DOOR_EXITING) || (IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE))) && !toggledEnginefromMenu)
                    {
                        vehicle.IsEngineRunning = false;
                    }

                    //vector position auto shutoff func
                }
            }

        }

    }



    private void EngineAnimation(object sender, EventArgs e)
    {
      
            int num;
        
        if (Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, new InputArgument[3]
                {

            (InputArgument) Game.Player.Character,
            (InputArgument) Game.Player.Character.CurrentVehicle,
            (InputArgument) false
                }))
                {
                 currentVehicle = Game.Player.Character.CurrentVehicle; 
            if (!toggledEnginefromMenu && !Game.Player.Character.CurrentVehicle.IsEngineRunning)
                {
                    num = (Entity)Game.Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == (Entity)Game.Player.Character ? 1 : 0;
                    goto label_5;
                }
            else if (toggledEnginefromMenu && Game.Player.Character.CurrentVehicle.IsEngineRunning)
            {

                //Game.Player.Character.CurrentVehicle.IsEngineRunning = false;
                //if (!Game.Player.Character.CurrentVehicle.IsEngineRunning)
                //toggledEnginefromMenu = false;
            }
                }
            
            num = 0;
        label_5:
            if (num != 0)
            {
            Game.DisableControlThisFrame(GTA.Control.VehicleExit);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)71, (InputArgument)true);
            Game.Player.Character.CurrentVehicle.IsEngineRunning = false;
            Game.Player.Character.CurrentVehicle.IsUndriveable = true;
        }
        if (!Game.Player.Character.IsSittingInVehicle() && ((Entity)Game.Player.Character.CurrentVehicle == (Entity)null || (Entity)Game.Player.Character.CurrentVehicle != (Entity)this.currentVehicle))
        {
            HasBooted = false;
            toggledEnginefromMenu = false;
            if (Game.Player.Character.CurrentVehicle != null)
            {
                //Game.Player.Character.CurrentVehicle.IsUndriveable = true;
                //Game.Player.Character.CurrentVehicle.IsEngineRunning = false;
//Function.Call(Hash.DISABLE_CONTROL_ACTION, (InputArgument)0, (InputArgument)71, (InputArgument)true);
            }

        }
        if (IsSubttaskActive(Game.Player.Character, Subtask.EXITING_VEHICLE))
        {

            toggledEnginefromMenu = false;
            
        }
        /*
        if (this.GameTimeRef2 < Game.GameTime)
        {
            this.GameTimeRef2 = Game.GameTime + 500;


            if (!this.config2.GetValue<bool>("Settings", "AUTO_START_ENGINE", false))
            {
                if (!Game.Player.Character.IsInVehicle())
                    this.VehicleTryingToEnter = Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_TRYING_TO_ENTER, new InputArgument[1]
                    {
              (InputArgument) Game.Player.Character
                    });
                if ((Entity)this.VehicleTryingToEnter != (Entity)null)
                {
                    this.engineStat = this.VehicleTryingToEnter.IsEngineRunning ? 1 : 0;
                    Script.Wait(100);
                    if ((Entity)this.VehicleTryingToEnter == (Entity)this.currentVehicle)
                        this.VehicleTryingToEnter = (Vehicle)null;
                }
            }
        }
        if (this.GameTimeRef < Game.GameTime)
        {
            this.GameTimeRef = Game.GameTime + 1000;
            if (Game.Player.Character.IsSittingInVehicle())
                this.driver = Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, new InputArgument[2]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle,
            (InputArgument) (-1)
                });
            if (Game.Player.Character.IsSittingInVehicle() && !this.getHashBool)
            {
                this.VehHash = Function.Call<uint>(Hash.GET_ENTITY_MODEL, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                });
                this.getHashBool = true;
            }
            if (!Game.Player.Character.IsInVehicle() || (Entity)Game.Player.Character.CurrentVehicle == (Entity)null)
                this.getHashBool = false;
            if ((Entity)this.currentVehicle != (Entity)null)
            {
                if ((Entity)this.driver == (Entity)Game.Player.Character)
                {
                    if ((Entity)Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_TRYING_TO_ENTER, new InputArgument[1]
                    {
              (InputArgument) Game.Player.Character
                    }) != (Entity)this.currentVehicle)
                        this.currentVehicle = this.prevVehicle;
                }
                else
                    Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, new InputArgument[1]
                    {
              (InputArgument) Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_TRYING_TO_ENTER, new InputArgument[1]
              {
                (InputArgument) false
              })
                    });
            }
        }*/
    }
    
    private void IgnitionKeyRemoveFX()
    {
        float num1 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 0
        });
        float num2 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 1
        });
        float num3 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 2
        });
        float num4 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 3
        });
        if (File.Exists("scripts\\CarControlV\\remove_key.wav"))
        {
            this.WavereaderDown2 = new WaveFileReader("scripts\\CarControlV\\remove_key.wav");
            this.wavechanDown2 = new WaveChannel32((WaveStream)this.WavereaderDown2);
            this.volumeDown2 = this.config2.GetValue<float>("Settings", "Global Volume Down", 30f);
            this.DSODown2 = new DirectSoundOut();
            if (this.config2.GetValue<bool>("Settings", "IMPROVED_SOUND_SYSTEM", true))
            {
                if (Game.Player.Character.IsSittingInVehicle() && Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) == 4)
                {
                    this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
                    this.wavechanDown2.Volume = (float)((double)this.volumeDown2 / 100.0 / 10.0);
                }
                if (Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) != 4 && Game.Player.Character.IsSittingInVehicle())
                {
                    int num5;
                    if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 0
                        }))
                        {
                            if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                            {
                  (InputArgument) Game.Player.Character.CurrentVehicle,
                  (InputArgument) 1
                            }) && (double)num1 <= 0.3 && (double)num2 <= 0.3 && (double)num3 <= 0.3)
                            {
                                num5 = (double)num4 > 0.3 ? 1 : 0;
                                goto label_10;
                            }
                        }
                    }
                    num5 = 1;
                label_10:
                    if (num5 != 0)
                    {
                        this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
                        this.wavechanDown2.Volume = (float)((double)this.volumeDown2 / 100.0 / 15.0);
                    }
                    int num6;
                    if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 0
                        }))
                        {
                            if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                            {
                  (InputArgument) Game.Player.Character.CurrentVehicle,
                  (InputArgument) 1
                            }) && (double)num1 < 0.1 && (double)num2 < 0.1 && (double)num3 < 0.1)
                            {
                                num6 = (double)num4 < 0.1 ? 1 : 0;
                                goto label_17;
                            }
                        }
                    }
                    num6 = 0;
                label_17:
                    if (num6 != 0)
                    {
                        this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
                        this.wavechanDown2.Volume = (float)((double)this.volumeDown2 / 100.0 / 20.0);
                    }
                }
            }
            else
            {
                this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
                this.wavechanDown2.Volume = this.volumeDown2 / 100f;
            }
            this.DSODown2.Play();
            this.DSODown2.Dispose();
        }
        else
            GTA.UI.Screen.ShowSubtitle("remove_key.wav is not found!");
    }
    private void IgnitionKeyInsertFX()
    {
        float num1 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 0
        });
        float num2 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 1
        });
        float num3 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 2
        });
        float num4 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 3
        });
        if (File.Exists("scripts\\CarControlV\\insert_key.wav"))
        {
            this.WavereaderDown2 = new WaveFileReader("scripts\\CarControlV\\insert_key.wav");
            this.wavechanDown2 = new WaveChannel32((WaveStream)this.WavereaderDown2);
            this.volumeDown2 = this.config2.GetValue<float>("Settings", "Global Volume Down", 30f);
            this.DSODown2 = new DirectSoundOut();
            this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
            if (this.config2.GetValue<bool>("Settings", "IMPROVED_SOUND_SYSTEM", true))
            {
                if (Game.Player.Character.IsSittingInVehicle() && Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) == 4)
                {
                    this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
                    this.wavechanDown2.Volume = (float)((double)this.volumeDown2 / 100.0 / 10.0);
                }
                if (Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) != 4 && Game.Player.Character.IsSittingInVehicle())
                {
                    int num5;
                    if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 0
                        }))
                        {
                            if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                            {
                  (InputArgument) Game.Player.Character.CurrentVehicle,
                  (InputArgument) 1
                            }) && (double)num1 <= 0.3 && (double)num2 <= 0.3 && (double)num3 <= 0.3)
                            {
                                num5 = (double)num4 > 0.3 ? 1 : 0;
                                goto label_10;
                            }
                        }
                    }
                    num5 = 1;
                label_10:
                    if (num5 != 0)
                    {
                        this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
                        this.wavechanDown2.Volume = (float)((double)this.volumeDown2 / 100.0 / 15.0);
                    }
                    int num6;
                    if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 0
                        }))
                        {
                            if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                            {
                  (InputArgument) Game.Player.Character.CurrentVehicle,
                  (InputArgument) 1
                            }) && (double)num1 < 0.1 && (double)num2 < 0.1 && (double)num3 < 0.1)
                            {
                                num6 = (double)num4 < 0.1 ? 1 : 0;
                                goto label_17;
                            }
                        }
                    }
                    num6 = 0;
                label_17:
                    if (num6 != 0)
                    {
                        this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
                        this.wavechanDown2.Volume = (float)((double)this.volumeDown2 / 100.0 / 20.0);
                    }
                }
            }
            else
            {
                this.DSODown2.Init((IWaveProvider)this.wavechanDown2);
                this.wavechanDown2.Volume = this.volumeDown2 / 100f;
            }
            this.DSODown2.Play();
            this.DSODown2.Dispose();
        }
        else
            GTA.UI.Screen.ShowSubtitle("insert_key.wav is not found!");
    }
    //Handbrake animation segment
    private void RegularHandbrake()
    {
        int num1;
        if (this.handOnHandbrake && Game.Player.Character.IsSittingInVehicle())
        {
            if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_CAR, new InputArgument[1]
            {
          (InputArgument) this.VehHash2
            }) && (Entity)Game.Player.Character.CurrentVehicle != (Entity)null && (Entity)this.driver2 == (Entity)Game.Player.Character)
            {
                if (!Function.Call<bool>(Hash.IS_PED_DOING_DRIVEBY, new InputArgument[1]
                {
            (InputArgument) this.driver2
                }))
                {
                    if (!Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, new InputArgument[1]
                    {
              (InputArgument) Game.Player
                    }))
                    {
                        num1 = (Entity)this.driver2 != (Entity)null ? 1 : 0;
                        goto label_6;
                    }
                }
            }
        }
        num1 = 0;
    label_6:
        if (num1 != 0)
        {
            int num2;
            if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
            {
          (InputArgument) this.driver2,
          (InputArgument) this.ANIMDICT,
          (InputArgument) this.ANIMNAME,
          (InputArgument) 3
            }))
                num2 = !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                {
            (InputArgument) this.driver2,
            (InputArgument) "veh@driveby@first_person@passenger_left_handed@1h",
            (InputArgument) "outro_90r",
            (InputArgument) 3
                }) ? 1 : 0;
            else
                num2 = 0;
            if (num2 != 0)
            {
                Function.Call(Hash.REQUEST_ANIM_DICT, new InputArgument[1]
                {
            (InputArgument) this.ANIMDICT
                });
                while (true)
                {
                    if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, new InputArgument[1]
                    {
              (InputArgument) this.ANIMDICT
                    }))
                        Script.Wait(10);
                    else
                        break;
                }
                this.driver2.Task.PlayAnimation(this.ANIMDICT, this.ANIMNAME, this.ANIMSPEED, -this.ANIMSPEED, -1, AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly, -1f);
            }
        }
        int num3;
        if (!this.handOnHandbrake && Game.Player.Character.IsSittingInVehicle())
        {
            if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_CAR, new InputArgument[1]
            {
          (InputArgument) this.VehHash2
            }) && (Entity)Game.Player.Character.CurrentVehicle != (Entity)null && (Entity)this.driver2 == (Entity)Game.Player.Character)
            {
                if (!Function.Call<bool>(Hash.IS_PED_DOING_DRIVEBY, new InputArgument[1]
                {
            (InputArgument) this.driver2
                }))
                {
                    if (!Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, new InputArgument[1]
                    {
              (InputArgument) Game.Player
                    }))
                    {
                        num3 = (Entity)this.driver2 != (Entity)null ? 1 : 0;
                        goto label_22;
                    }
                }
            }
        }
        num3 = 0;
    label_22:
        if (num3 == 0)
            return;
        if (!this.leaveHandOnBrake)
        {
            if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
            {
          (InputArgument) this.driver2,
          (InputArgument) this.ANIMDICT,
          (InputArgument) this.ANIMNAME,
          (InputArgument) 3
            }))
                this.driver2.Task.ClearAnimation(this.ANIMDICT, this.ANIMNAME);
        }
        if (!this.staticHandbrake)
            Function.Call(Hash.SET_VEHICLE_HANDBRAKE, new InputArgument[2]
                     {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) false
                     });
    }

    private void Handbrake(object sender, EventArgs e)
    {
        if (this.GameTimeRef5 < Game.GameTime)
        {
            this.GameTimeRef5 = Game.GameTime + 2000;
            if (Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, new InputArgument[1]
            {
          (InputArgument) Game.Player.Character
            }))
                this.VehicleTryingToEnter2 = Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_TRYING_TO_ENTER, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character
                });
            int num1;
            if ((Entity)this.brakedVehicle != (Entity)null)
                num1 = Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, new InputArgument[3]
                {
            (InputArgument) Game.Player.Character,
            (InputArgument) this.brakedVehicle,
            (InputArgument) true
                }) ? 1 : 0;
            else
                num1 = 0;
            if (num1 != 0)
            {
                this.staticHandbrake = true;
            }
            else
            {
                int num2;
                if ((Entity)this.brakedVehicle != (Entity)null)
                    num2 = !Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, new InputArgument[3]
                    {
              (InputArgument) Game.Player.Character,
              (InputArgument) this.brakedVehicle,
              (InputArgument) true
                    }) ? 1 : 0;
                else
                    num2 = 0;
                if (num2 != 0)
                    this.staticHandbrake = false;
            }
            if ((Entity)this.brakedVehicle == (Entity)null)
                this.staticHandbrake = false;
        }
        if ((Entity)this.brakedVehicle != (Entity)null)
            Function.Call(Hash.SET_VEHICLE_HANDBRAKE, new InputArgument[2]
                    {
              (InputArgument) brakedVehicle,
              (InputArgument) true
                    });
        if (this.handbrakeKeyPressed)
        {
            if (this.config3.GetValue<bool>("Settings", "HINT_TOGGLE", true))
            {
                string str1 = ((CarHud.Inputs)this.config3.GetValue<int>("Settings", "STATIC_HANDBRAKE_PRESS_BUTTON", 245)).ToString();
                string str2 = ((CarHud.Inputs)this.config3.GetValue<int>("Settings", "LEAVE_HAND_ON_HANDBRAKE", 72)).ToString();
                if (this.config3.GetValue<bool>("Settings", "HAND_ON_BRAKE_FUNCTION", false))
                    DisplayHelpTextThisFrame("~" + str1 + "~ handbrake ~n~~" + str2 + "~ leave hand on brake");
                else
                    DisplayHelpTextThisFrame("~" + str1 + "~ handbrake");
                Vehicle vehicle = Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_USING, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character
                });
                Function.Call(Hash.SET_VEHICLE_HANDBRAKE, new InputArgument[2]
                    {
              (InputArgument) vehicle,
              (InputArgument) false
                    });
                if ((Entity)this.brakedVehicle != (Entity)null && (Entity)vehicle != (Entity)this.brakedVehicle)
                    Function.Call(Hash.SET_VEHICLE_HANDBRAKE, new InputArgument[2] { (InputArgument)vehicle, (InputArgument)false });
            }
            if (this.config3.GetValue<bool>("Settings", "HAND_ON_BRAKE_FUNCTION", false))
            {
                if (Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, new InputArgument[2]
                {
            (InputArgument) 0,
            (InputArgument) this.config3.GetValue<int>("Settings", "LEAVE_HAND_ON_HANDBRAKE", 72)
                }))
                    this.leaveHandOnBrake = !this.leaveHandOnBrake;
            }
            if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2]
            {
          (InputArgument) 0,
          (InputArgument) this.config3.GetValue<int>("Settings", "STATIC_HANDBRAKE_PRESS_BUTTON", 245)
            }))
            {
                Vehicle vehicle = Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_USING, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character
                });
                if ((Entity)this.brakedVehicle == (Entity)null)
                    this.brakedVehicle = vehicle;
                else if ((Entity)this.brakedVehicle != (Entity)null)
                    this.brakedVehicle = !((Entity)vehicle == (Entity)this.brakedVehicle) ? vehicle : (Vehicle)null;
                int num;
                if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                {
            (InputArgument) this.driver2,
            (InputArgument) this.ANIMDICT,
            (InputArgument) this.ANIMNAME,
            (InputArgument) 3
                }))
                    num = !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                    {
              (InputArgument) this.driver2,
              (InputArgument) "veh@driveby@first_person@passenger_left_handed@1h",
              (InputArgument) "outro_90r",
              (InputArgument) 3
                    }) ? 1 : 0;
                else
                    num = 0;
                if (num != 0)
                {
                    Function.Call(Hash.REQUEST_ANIM_DICT, new InputArgument[1]
                    {
              (InputArgument) this.ANIMDICT
                    });
                    while (true)
                    {
                        if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, new InputArgument[1]
                        {
                (InputArgument) this.ANIMDICT
                        }))
                            Script.Wait(10);
                        else
                            break;
                    }
                    this.driver2.Task.PlayAnimation(this.ANIMDICT, this.ANIMNAME, this.ANIMSPEED, -this.ANIMSPEED, -1, AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly, -1f);
                    Script.Wait(390);
                    if (!this.staticHandbrake || !this.handOnHandbrake)
                        this.HandbrakeFX();
                    this.staticHandbrake = !this.staticHandbrake;
                }
                else
                    this.staticHandbrake = !this.staticHandbrake;
            }
        }
        if (this.GameTimeRef4 < Game.GameTime)
        {
            this.GameTimeRef4 = Game.GameTime + 1000;
            if (this.config3.GetValue<bool>("Settings", "HAND_ON_BRAKE_FUNCTION", false) && this.leaveHandOnBrake)
            {
                int num;
                if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                {
            (InputArgument) this.driver2,
            (InputArgument) this.ANIMDICT,
            (InputArgument) this.ANIMNAME,
            (InputArgument) 3
                }))
                    num = !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                    {
              (InputArgument) this.driver2,
              (InputArgument) "veh@driveby@first_person@passenger_left_handed@1h",
              (InputArgument) "outro_90r",
              (InputArgument) 3
                    }) ? 1 : 0;
                else
                    num = 0;
                if (num != 0)
                {
                    Function.Call(Hash.REQUEST_ANIM_DICT, new InputArgument[1]
                    {
              (InputArgument) this.ANIMDICT
                    });
                    while (true)
                    {
                        if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, new InputArgument[1]
                        {
                (InputArgument) this.ANIMDICT
                        }))
                            Script.Wait(10);
                        else
                            break;
                    }
                    this.driver2.Task.PlayAnimation(this.ANIMDICT, this.ANIMNAME, this.ANIMSPEED, -this.ANIMSPEED, -1, AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly, -1f);
                }
            }
            if (Game.Player.Character.IsSittingInVehicle())
                this.driver2 = Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, new InputArgument[2]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle,
            (InputArgument)(-1)
                });
            if (!Game.Player.Character.IsInVehicle() || (Entity)Game.Player.Character.CurrentVehicle == (Entity)null)
            {
                this.handOnHandbrake = false;
                this.leaveHandOnBrake = false;
            }
            int num3;
            if (!Game.Player.Character.IsSittingInVehicle())
            {
                if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                {
            (InputArgument) this.driver2,
            (InputArgument) this.ANIMDICT,
            (InputArgument) this.ANIMNAME,
            (InputArgument) 3
                }))
                {
                    num3 = Function.Call<uint>(Hash.GET_SELECTED_PED_WEAPON, new InputArgument[1]
                    {
              (InputArgument) Game.Player.Character
                    }) != 883325847U ? 1 : 0;
                    goto label_67;
                }
            }
            num3 = 0;
        label_67:
            if (num3 != 0)
                Function.Call(Hash.STOP_ANIM_TASK, new InputArgument[4]
                {
            (InputArgument) this.driver2,
            (InputArgument) this.ANIMDICT,
            (InputArgument) this.ANIMNAME,
            (InputArgument) 1f
                });
            int num4;
            if (!Game.Player.Character.IsSittingInVehicle())
                num4 = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                {
            (InputArgument) this.driver2,
            (InputArgument) "veh@driveby@first_person@passenger_left_handed@1h",
            (InputArgument) "outro_90r",
            (InputArgument) 3
                }) ? 1 : 0;
            else
                num4 = 0;
            if (num4 != 0)
                Function.Call(Hash.STOP_ANIM_TASK, new InputArgument[4]
                {
            (InputArgument) this.driver2,
            (InputArgument) "veh@driveby@first_person@passenger_left_handed@1h",
            (InputArgument) "outro_90r",
            (InputArgument) 1f
                });
            int num5;
            if (Game.Player.Character.IsSittingInVehicle())
            {
                if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_CAR, new InputArgument[1]
                {
            (InputArgument) this.VehHash2
                }) && (Entity)Game.Player.Character.CurrentVehicle != (Entity)null && (Entity)this.driver2 == (Entity)Game.Player.Character)
                {
                    if (!Function.Call<bool>(Hash.IS_PED_DOING_DRIVEBY, new InputArgument[1]
                    {
              (InputArgument) this.driver2
                    }))
                    {
                        if (!Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, new InputArgument[1]
                        {
                (InputArgument) Game.Player
                        }) && (Entity)this.driver2 != (Entity)null)
                        {
                            num5 = this.brakeOn ? 1 : 0;
                            goto label_80;
                        }
                    }
                }
            }
            num5 = 0;
        label_80:
            if (num5 != 0)
            {
                int num6;
                if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                {
            (InputArgument) this.driver2,
            (InputArgument) this.ANIMDICT,
            (InputArgument) this.ANIMNAME,
            (InputArgument) 3
                }))
                    num6 = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, new InputArgument[4]
                    {
              (InputArgument) this.driver2,
              (InputArgument) "veh@driveby@first_person@passenger_left_handed@1h",
              (InputArgument) "outro_90r",
              (InputArgument) 3
                    }) ? 1 : 0;
                else
                    num6 = 1;
                if (num6 != 0)
                    Function.Call(Hash.SET_VEHICLE_HANDBRAKE, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) true
                    });
            }
            if ((Entity)Game.Player.Character.CurrentVehicle != (Entity)null && (Entity)this.driver2 == (Entity)Game.Player.Character)
            {
                int num7;
                if (this.config3.GetValue<bool>("SETTINGS", "ICON_DRAW", true))
                {
                    if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_CAR, new InputArgument[1]
                    {
              (InputArgument) this.VehHash2
                    }) && !Function.Call<bool>(Hash.IS_RADAR_HIDDEN, Array.Empty<InputArgument>()) && !Function.Call<bool>(Hash.IS_HUD_HIDDEN, Array.Empty<InputArgument>()) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING, Array.Empty<InputArgument>()))
                    {
                        num7 = !GTA.UI.Screen.IsFadedOut ? 1 : 0;
                        goto label_92;
                    }
                }
                num7 = 0;
            label_92:
                if (num7 != 0)
                {
                    int x = (int)this.config3.GetValue<float>("Settings", "x", 1250f);
                    int y = (int)this.config3.GetValue<float>("Settings", "y", 361f);
                    int width = (int)this.config3.GetValue<float>("Settings", "Width", 29f);
                    int height = (int)this.config3.GetValue<float>("Settings", "Height", 29f);
                    if (File.Exists("scripts\\CarControlV\\handbrakeOn.png") && File.Exists("scripts\\CarControlV\\handbrakeOff.png"))
                    {
                        if (this.handOnHandbrake && !this.staticHandbrake)
                            new CustomSprite(".\\Scripts\\CarControlV\\handbrakeOn.png", new Size(width, height), new Point(x, y));
                        else if (!this.handOnHandbrake && this.staticHandbrake)
                            new CustomSprite(".\\Scripts\\CarControlV\\handbrakeOn.png", new Size(width, height), new Point(x, y));
                        else if (!this.handOnHandbrake && !this.staticHandbrake)
                            new CustomSprite(".\\Scripts\\CarControlV\\handbrakeOff.png", new Size(width, height), new Point(x, y));
                    }
                    else
                        GTA.UI.Screen.ShowSubtitle("handbrakeOff.png or handbrakeOn was not found!");
                }
            }
            if (Game.Player.Character.IsSittingInVehicle() && !this.getHashBool2)
            {
                this.VehHash2 = Function.Call<uint>(Hash.GET_ENTITY_MODEL, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                });
                this.getHashBool2 = true;
            }
            if (!Game.Player.Character.IsInVehicle() || (Entity)Game.Player.Character.CurrentVehicle == (Entity)null)
                this.getHashBool2 = false;
        }
        int num8;
        if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2]
        {
        (InputArgument) 0,
        (InputArgument) 76
        }) && Game.Player.Character.IsSittingInVehicle())
        {
            if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_CAR, new InputArgument[1]
            {
          (InputArgument) this.VehHash2
            }) && (Entity)Game.Player.Character.CurrentVehicle != (Entity)null && (Entity)this.driver2 == (Entity)Game.Player.Character)
            {
                if (!Function.Call<bool>(Hash.IS_PED_DOING_DRIVEBY, new InputArgument[1]
                {
            (InputArgument) this.driver2
                }))
                {
                    if (!Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, new InputArgument[1]
                    {
              (InputArgument) Game.Player
                    }) && (Entity)this.driver2 != (Entity)null && (Entity)Game.Player.Character.CurrentVehicle != (Entity)null && !this.handbrakeKeyPressed)
                    {
                        num8 = !Function.Call<bool>(Hash.IS_PED_RUNNING_MOBILE_PHONE_TASK, new InputArgument[1]
                        {
                (InputArgument) this.driver2
                        }) ? 1 : 0;
                        goto label_114;
                    }
                }
            }
        }
        num8 = 0;
    label_114:
        if (num8 != 0)
        {
            if (Function.Call<bool>(Hash.IS_PED_RUNNING_MOBILE_PHONE_TASK, new InputArgument[1]
            {
          (InputArgument) this.driver2
            }))
                this.driver2.Task.ClearAll();
            this.handbrakeKeyPressed = true;
            this.handOnHandbrake = true;
            this.brakeOn = true;
            float num9 = Function.Call<float>(Hash.GET_ENTITY_ANIM_TOTAL_TIME, new InputArgument[3]
            {
          (InputArgument) this.driver2,
          (InputArgument) this.ANIMDICT,
          (InputArgument) this.ANIMNAME
            });
            float num10 = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, new InputArgument[3]
            {
          (InputArgument) this.driver2,
          (InputArgument) this.ANIMDICT,
          (InputArgument) this.ANIMNAME
            });
            float num11 = Function.Call<float>(Hash.GET_ENTITY_ANIM_TOTAL_TIME, new InputArgument[3]
            {
          (InputArgument) this.driver2,
          (InputArgument) "veh@driveby@first_person@passenger_left_handed@1h",
          (InputArgument) "outro_90r"
            });
            float num12 = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, new InputArgument[3]
            {
          (InputArgument) this.driver2,
          (InputArgument) "veh@driveby@first_person@passenger_left_handed@1h",
          (InputArgument) "outro_90r"
            });
            if ((double)num10 == (double)num9)
                this.HandbrakeFX();
            else if ((double)num12 == (double)num11)
                this.HandbrakeFX();
            this.RegularHandbrake();
        }
        int num13;
        if (!Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, new InputArgument[2]
        {
        (InputArgument) 0,
        (InputArgument) 76
        }))
            num13 = Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2]
            {
          (InputArgument) 0,
          (InputArgument) 76
            }) ? 0 : (this.handbrakeKeyPressed ? 1 : 0);
        else
            num13 = 1;
        if (num13 == 0)
            return;
        int num14;
        if (Game.Player.Character.IsSittingInVehicle())
        {
            if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_CAR, new InputArgument[1]
            {
          (InputArgument) this.VehHash2
            }) && (Entity)Game.Player.Character.CurrentVehicle != (Entity)null && (Entity)this.driver2 == (Entity)Game.Player.Character)
            {
                if (!Function.Call<bool>(Hash.IS_PED_DOING_DRIVEBY, new InputArgument[1]
                {
            (InputArgument) this.driver2
                }))
                {
                    if (!Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, new InputArgument[1]
                    {
              (InputArgument) Game.Player
                    }))
                    {
                        num14 = (Entity)this.driver2 != (Entity)null ? 1 : 0;
                        goto label_133;
                    }
                }
            }
        }
        num14 = 0;
    label_133:
        if (num14 != 0)
        {
            this.brakeOn = false;
            this.handbrakeKeyPressed = false;
            this.handOnHandbrake = false;
            this.RegularHandbrake();
            this.HandbrakeReleaseFX();
        }

    }
    private void HandbrakeFX()
    {
        if (!((Entity)Game.Player.Character.CurrentVehicle != (Entity)null))
            return;
        float num1 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 0
        });
        float num2 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 1
        });
        float num3 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 2
        });
        float num4 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 3
        });
        if (File.Exists("scripts\\CarControlV\\handbrake.wav"))
        {
            this.WavereaderDown3 = new WaveFileReader("scripts\\CarControlV\\handbrake.wav");
            this.wavechanDown3 = new WaveChannel32((WaveStream)this.WavereaderDown3);
            this.volumeDown3 = this.config3.GetValue<float>("Settings", "Global Volume Down", 30f);
            this.DSODown3 = new DirectSoundOut();
            this.DSODown3.Init((IWaveProvider)this.wavechanDown3);
            if (Game.Player.Character.IsSittingInVehicle() && Function.Call<int>(Hash.GET_FOLLOW_PED_CAM_VIEW_MODE, Array.Empty<InputArgument>()) == 4)
            {
                this.DSODown3.Init((IWaveProvider)this.wavechanDown3);
                this.wavechanDown3.Volume = (float)((double)this.volumeDown3 / 100.0 / 2.0);
            }
            if (Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) != 4 && Game.Player.Character.IsSittingInVehicle())
            {
                int num5;
                if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                }))
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) 0
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 1
                        }) && (double)num1 <= 0.3 && (double)num2 <= 0.3 && (double)num3 <= 0.3)
                        {
                            num5 = (double)num4 > 0.3 ? 1 : 0;
                            goto label_10;
                        }
                    }
                }
                num5 = 1;
            label_10:
                if (num5 != 0)
                {
                    this.DSODown3.Init((IWaveProvider)this.wavechanDown3);
                    this.wavechanDown3.Volume = (float)((double)this.volumeDown3 / 100.0 / 5.0);
                }
                int num6;
                if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                }))
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) 0
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 1
                        }) && (double)num1 < 0.1 && (double)num2 < 0.1 && (double)num3 < 0.1)
                        {
                            num6 = (double)num4 < 0.1 ? 1 : 0;
                            goto label_17;
                        }
                    }
                }
                num6 = 0;
            label_17:
                if (num6 != 0)
                {
                    this.DSODown3.Init((IWaveProvider)this.wavechanDown3);
                    this.wavechanDown3.Volume = (float)((double)this.volumeDown3 / 100.0 / 10.0);
                }
            }
            this.DSODown3.Play();
            this.DSODown3.Dispose();
        }
        else
            GTA.UI.Screen.ShowSubtitle("handbrake.wav is not found!");
    }
    private void HandbrakeReleaseFX()
    {
        if (!((Entity)Game.Player.Character.CurrentVehicle != (Entity)null))
            return;
        float num1 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 0
        });
        float num2 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 1
        });
        float num3 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 2
        });
        float num4 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 3
        });
        if (File.Exists("scripts\\CarControlV\\handbrakeRelease.wav"))
        {
            this.WavereaderDown3 = new WaveFileReader("scripts\\CarControlV\\handbrakeRelease.wav");
            this.wavechanDown3 = new WaveChannel32((WaveStream)this.WavereaderDown3);
            this.volumeDown3 = this.config3.GetValue<float>("Settings", "Global Volume Down", 30f);
            this.DSODown3 = new DirectSoundOut();
            this.DSODown3.Init((IWaveProvider)this.wavechanDown3);
            if (Game.Player.Character.IsSittingInVehicle() && Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) == 4)
            {
                this.DSODown3.Init((IWaveProvider)this.wavechanDown3);
                this.wavechanDown3.Volume = this.volumeDown / 100f;
            }
            if (Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) != 4 && Game.Player.Character.IsSittingInVehicle())
            {
                int num5;
                if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                }))
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) 0
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 1
                        }) && (double)num1 <= 0.3 && (double)num2 <= 0.3 && (double)num3 <= 0.3)
                        {
                            num5 = (double)num4 > 0.3 ? 1 : 0;
                            goto label_10;
                        }
                    }
                }
                num5 = 1;
            label_10:
                if (num5 != 0)
                {
                    this.DSODown3.Init((IWaveProvider)this.wavechanDown3);
                    this.wavechanDown3.Volume = (float)((double)this.volumeDown3 / 100.0 / 5.0);
                }
                int num6;
                if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                }))
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) 0
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 1
                        }) && (double)num1 < 0.1 && (double)num2 < 0.1 && (double)num3 < 0.1)
                        {
                            num6 = (double)num4 < 0.1 ? 1 : 0;
                            goto label_17;
                        }
                    }
                }
                num6 = 0;
            label_17:
                if (num6 != 0)
                {
                    this.DSODown3.Init((IWaveProvider)this.wavechanDown3);
                    this.wavechanDown3.Volume = (float)((double)this.volumeDown3 / 100.0 / 10.0);
                }
            }
            this.DSODown3.Play();
            this.DSODown3.Dispose();
        }
        else
            GTA.UI.Screen.ShowSubtitle("handbrakeRelease.wav is not found!");
    }

    // Headlight and indicator animation
    private void headlightsAnim()
    {
        if ((Entity)Game.Player.Character.CurrentVehicle != (Entity)null && Game.Player.Character.CurrentVehicle.Model.IsCar)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, new InputArgument[1]
            {
          (InputArgument) this.AnimDictHeadlights
            });
            while (true)
            {
                if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, new InputArgument[1]
                {
            (InputArgument) this.AnimDictHeadlights
                }))
                    Script.Wait(10);
                else
                    break;
            }
            Function.Call(Hash.TASK_PLAY_ANIM, new InputArgument[11]
            {
          (InputArgument) Game.Player.Character,
          (InputArgument) this.AnimDictHeadlights,
          (InputArgument) this.AnimNameHeadlights,
          (InputArgument) 8.5f,
          (InputArgument)(-8.1f),
          (InputArgument)(-1),
          (InputArgument) 48,
          (InputArgument) 0.0f,
          (InputArgument) 0,
          (InputArgument) 0,
          (InputArgument) 0
            });
        }
        else
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, new InputArgument[1]
            {
          (InputArgument) this.AnimDictHazzard
            });
            while (true)
            {
                if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, new InputArgument[1]
                {
            (InputArgument) this.AnimDictHazzard
                }))
                    Script.Wait(10);
                else
                    break;
            }
            Function.Call(Hash.TASK_PLAY_ANIM, new InputArgument[11]
            {
          (InputArgument) Game.Player.Character,
          (InputArgument) this.AnimDictHazzard,
          (InputArgument) this.AnimNameHazzard,
          (InputArgument) 8.5f,
          (InputArgument)(-8.1f),
          (InputArgument)(-1),
          (InputArgument) 48,
          (InputArgument) 0.0f,
          (InputArgument) 0,
          (InputArgument) 0,
          (InputArgument) 0
            });
        }
    }

    private void indicatorAnim()
    {
        Function.Call(Hash.REQUEST_ANIM_DICT, new InputArgument[1]
        {
        (InputArgument) this.AnimDictBlinkers
        });
        while (true)
        {
            if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, new InputArgument[1]
            {
          (InputArgument) this.AnimDictBlinkers
            }))
                Script.Wait(10);
            else
                break;
        }
        Function.Call(Hash.TASK_PLAY_ANIM, new InputArgument[11]
        {
        (InputArgument) Game.Player.Character,
        (InputArgument) this.AnimDictBlinkers,
        (InputArgument) this.AnimNameBlinkers,
        (InputArgument) 1.5f,
        (InputArgument)(-4.1f),
        (InputArgument)(-1),
        (InputArgument) 48,
        (InputArgument) 0.3f,
        (InputArgument) 0,
        (InputArgument) 0,
        (InputArgument) 0
        });
        Script.Wait(this.BlinkersAnimStopAfterSeconds);
        Function.Call(Hash.STOP_ENTITY_ANIM, new InputArgument[4]
        {
        (InputArgument) Game.Player.Character,
        (InputArgument) this.AnimNameBlinkers,
        (InputArgument) this.AnimDictBlinkers,
        (InputArgument) 3f
        });
    }

    private void IndicatorTickSoundEffects()
    {
        if (!((Entity)Game.Player.Character.CurrentVehicle != (Entity)null) || !Game.Player.Character.CurrentVehicle.IsEngineRunning)
            return;
        float num1 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 0
        });
        float num2 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 1
        });
        float num3 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 2
        });
        float num4 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 3
        });
        if (File.Exists("scripts\\CarControlV\\indicatorTick.wav"))
        {
            this.WavereaderDown4 = new WaveFileReader("scripts\\CarControlV\\indicatorTick.wav");
            this.wavechanDown4 = new WaveChannel32((WaveStream)this.WavereaderDown4);
            this.volumeDown4 = this.config4.GetValue<float>("Settings", "Global Volume Down", 100f);
            this.DSODown4 = new DirectSoundOut();
            this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
            this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 7.0);
            if (Game.Player.Character.IsSittingInVehicle() && Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) == 4)
            {
                this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
                this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 7.0);
            }
            if (Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) != 4 && Game.Player.Character.IsSittingInVehicle())
            {
                int num5;
                if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                }))
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) 0
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 1
                        }) && (double)num1 <= 0.3 && (double)num2 <= 0.3 && (double)num3 <= 0.3)
                        {
                            num5 = (double)num4 > 0.3 ? 1 : 0;
                            goto label_10;
                        }
                    }
                }
                num5 = 1;
            label_10:
                if (num5 != 0)
                {
                    this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
                    this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 15.0);
                }
                int num6;
                if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                }))
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) 0
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 1
                        }) && (double)num1 < 0.1 && (double)num2 < 0.1 && (double)num3 < 0.1)
                        {
                            num6 = (double)num4 < 0.1 ? 1 : 0;
                            goto label_17;
                        }
                    }
                }
                num6 = 0;
            label_17:
                if (num6 != 0)
                {
                    this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
                    this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 50.0);
                }
            }
            this.DSODown4.Play();
            this.DSODown4.Dispose();
        }
        else
            GTA.UI.Screen.ShowSubtitle("indicatorTick.wav is not found!");
    }
    private void IndicatorOffSoundEffects()
    {
        if (!((Entity)Game.Player.Character.CurrentVehicle != (Entity)null))
            return;
        float num1 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 0
        });
        float num2 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 1
        });
        float num3 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 2
        });
        float num4 = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 3
        });
        if (File.Exists("scripts\\CarControlV\\indicatorOff.wav"))
        {
            this.WavereaderDown4 = new WaveFileReader("scripts\\CarControlV\\indicatorOff.wav");
            this.wavechanDown4 = new WaveChannel32((WaveStream)this.WavereaderDown4);
            this.volumeDown4 = this.config4.GetValue<float>("Settings", "Global Volume Down", 30f);
            this.DSODown4 = new DirectSoundOut();
            this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
            this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 3.0);
            if (Game.Player.Character.IsSittingInVehicle() && Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) == 4)
            {
                this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
                this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 3.0);
            }
            if (Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) != 4 && Game.Player.Character.IsSittingInVehicle())
            {
                int num5;
                if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                }))
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) 0
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 1
                        }) && (double)num1 <= 0.3 && (double)num2 <= 0.3 && (double)num3 <= 0.3)
                        {
                            num5 = (double)num4 > 0.3 ? 1 : 0;
                            goto label_10;
                        }
                    }
                }
                num5 = 1;
            label_10:
                if (num5 != 0)
                {
                    this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
                    this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 15.0);
                }
                int num6;
                if (Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, new InputArgument[1]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle
                }))
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                    {
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) 0
                    }))
                    {
                        if (Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, new InputArgument[2]
                        {
                (InputArgument) Game.Player.Character.CurrentVehicle,
                (InputArgument) 1
                        }) && (double)num1 < 0.1 && (double)num2 < 0.1 && (double)num3 < 0.1)
                        {
                            num6 = (double)num4 < 0.1 ? 1 : 0;
                            goto label_17;
                        }
                    }
                }
                num6 = 0;
            label_17:
                if (num6 != 0)
                {
                    this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
                    this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 50.0);
                }
            }
            this.DSODown4.Play();
            this.DSODown4.Dispose();
        }
        else
            GTA.UI.Screen.ShowSubtitle("indicatorOff.wav is not found!");
    }
    private void IndicatorSoundEffects()
    {
        if (!((Entity)Game.Player.Character.CurrentVehicle != (Entity)null))
            return;
        Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 0
        });
        Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 1
        });
        Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 2
        });
        Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, new InputArgument[2]
        {
        (InputArgument) Game.Player.Character.CurrentVehicle.Handle,
        (InputArgument) 3
        });
        if (File.Exists("scripts\\CarControlV\\Indicator.wav"))
        {
            this.WavereaderDown4 = new WaveFileReader("scripts\\CarControlV\\Indicator.wav");
            this.wavechanDown4 = new WaveChannel32((WaveStream)this.WavereaderDown4);
            this.volumeDown4 = this.config4.GetValue<float>("Settings", "Global Volume Down", 30f);
            this.DSODown4 = new DirectSoundOut();
            this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
            this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 3.0);
            if (Game.Player.Character.IsSittingInVehicle() && Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) == 4)
            {
                this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
                this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 3.0);
            }
            if (Function.Call<int>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE, Array.Empty<InputArgument>()) != 4 && Game.Player.Character.IsSittingInVehicle())
            {
                this.DSODown4.Init((IWaveProvider)this.wavechanDown4);
                this.wavechanDown4.Volume = (float)((double)this.volumeDown4 / 100.0 / 25.0);
            }
            this.DSODown4.Play();
            this.DSODown4.Dispose();
        }
        else
            GTA.UI.Screen.ShowSubtitle("indicator.wav is not found!");
    }

    private void Indicators(object sender, EventArgs e)
    {
        Model model;

        if (this.autoTurnOffLoop < Game.GameTime)
        {
            this.autoTurnOffLoop = Game.GameTime + 500;
            if (Game.Player.Character.IsSittingInVehicle())
            {
                model = Game.Player.Character.CurrentVehicle.Model;
                int num;
                if (!model.IsCar)
                {
                    model = Game.Player.Character.CurrentVehicle.Model;
                    if (!model.IsBike)
                    {
                        model = Game.Player.Character.CurrentVehicle.Model;
                        num = model.IsQuadBike ? 1 : 0;
                        goto label_75;
                    }
                }
                num = 1;
            label_75:
                if (num != 0)
                {
                    if (this.rightIndicatorOn)
                    {
                        if ((double)Game.Player.Character.CurrentVehicle.SteeringScale <= -0.75)
                            this.indicatorRightWheelBack = true;
                        else if (this.indicatorRightWheelBack)
                        {
                            ++this.r;
                            if ((double)this.r > 1.0)
                            {
                                this.r = 0.0f;
                                this.indicatorSignal = false;
                                this.indicatorRight = false;
                                this.rightIndicatorOn = false;
                                this.indicatorStat.SetValue<bool>("BLINKERS", "RIGHT", false);
                                this.indicatorStat.Save();
                                Game.Player.Character.CurrentVehicle.IsRightIndicatorLightOn = false;
                                this.IndicatorOffSoundEffects();
                                this.indicatorRightWheelBack = false;
                            }
                        }
                    }
                    if (this.leftIndicatorOn)
                    {
                        if ((double)Game.Player.Character.CurrentVehicle.SteeringScale >= 0.75)
                            this.indicatorLeftWheelBack = true;
                        else if (this.indicatorLeftWheelBack)
                        {
                            ++this.l;
                            if ((double)this.l > 1.0)
                            {
                                this.l = 0.0f;
                                this.indicatorSignal = false;
                                this.indicatorLeft = false;
                                this.leftIndicatorOn = false;
                                Game.Player.Character.CurrentVehicle.IsLeftIndicatorLightOn = false;
                                this.IndicatorOffSoundEffects();
                                this.indicatorLeftWheelBack = false;
                                this.indicatorStat.SetValue<bool>("BLINKERS", "LEFT", false);
                                this.indicatorStat.Save();
                            }
                        }
                    }
                }
            }
        }
        if (this.GameTimeRef6 >= Game.GameTime)
            return;
        this.GameTimeRef6 = Game.GameTime + 1000;
        if (!this.leftIndicatorOn)
            this.l = 0.0f;
        if (!this.rightIndicatorOn)
            this.r = 0.0f;
        if (Game.Player.Character.IsSittingInVehicle())
        {
            model = Game.Player.Character.CurrentVehicle.Model;
            int num4;
            if (!model.IsCar)
            {
                model = Game.Player.Character.CurrentVehicle.Model;
                if (!model.IsBike)
                {
                    model = Game.Player.Character.CurrentVehicle.Model;
                    num4 = model.IsQuadBike ? 1 : 0;
                    goto label_107;
                }
            }
            num4 = 1;
        label_107:
            if (num4 != 0)
            {
                this.driver3 = Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, new InputArgument[2]
                {
            (InputArgument) Game.Player.Character.CurrentVehicle,
            (InputArgument) (-1)
                });
                if (!this.enterVehicleAgain)
                {
                    if ((Entity)Game.Player.Character.LastVehicle != (Entity)null)
                    {
                        if ((Entity)Game.Player.Character.LastVehicle == (Entity)Game.Player.Character.CurrentVehicle)
                        {
                            this.indicatorLeft = this.indicatorStat.GetValue<bool>("BLINKERS", "LEFT", false);
                            this.indicatorRight = this.indicatorStat.GetValue<bool>("BLINKERS", "RIGHT", false);
                            this.hazardLights = this.indicatorStat.GetValue<bool>("BLINKERS", "HAZARD", false);
                            this.indicatorSignal = this.indicatorLeft || this.indicatorRight || this.hazardLights;
                            this.enterVehicleAgain = true;
                        }
                        else
                        {
                            this.indicatorStat.SetValue<bool>("BLINKERS", "LEFT", false);
                            this.indicatorStat.Save();
                            this.indicatorStat.SetValue<bool>("BLINKERS", "RIGHT", false);
                            this.indicatorStat.Save();
                            this.indicatorStat.SetValue<bool>("BLINKERS", "HAZARD", false);
                            this.indicatorStat.Save();
                            Game.Player.Character.CurrentVehicle.IsRightIndicatorLightOn = false;
                            Game.Player.Character.CurrentVehicle.IsLeftIndicatorLightOn = false;
                            this.indicatorLeft = false;
                            this.indicatorRight = false;
                            this.indicatorSignal = false;
                            this.enterVehicleAgain = true;
                        }
                    }
                    else
                    {
                        if (this.indicatorRight)
                        {
                            this.indicatorStat.SetValue<bool>("BLINKERS", "RIGHT", true);
                            this.indicatorStat.Save();
                            Game.Player.Character.CurrentVehicle.IsRightIndicatorLightOn = true;
                        }
                        else
                        {
                            this.indicatorStat.SetValue<bool>("BLINKERS", "RIGHT", false);
                            this.indicatorStat.Save();
                            Game.Player.Character.CurrentVehicle.IsRightIndicatorLightOn = false;
                        }
                        if (this.indicatorLeft)
                        {
                            this.indicatorStat.SetValue<bool>("BLINKERS", "LEFT", true);
                            this.indicatorStat.Save();
                            Game.Player.Character.CurrentVehicle.IsLeftIndicatorLightOn = true;
                        }
                        else
                        {
                            this.indicatorStat.SetValue<bool>("BLINKERS", "LEFT", false);
                            this.indicatorStat.Save();
                            Game.Player.Character.CurrentVehicle.IsLeftIndicatorLightOn = false;
                        }
                        if (this.hazardLights)
                        {
                            this.indicatorStat.SetValue<bool>("BLINKERS", "HAZARD", true);
                            this.indicatorStat.Save();
                            Game.Player.Character.CurrentVehicle.IsLeftIndicatorLightOn = true;
                            Game.Player.Character.CurrentVehicle.IsLeftIndicatorLightOn = true;
                        }
                        else
                        {
                            this.indicatorStat.SetValue<bool>("BLINKERS", "HAZARD", false);
                            this.indicatorStat.Save();
                            if (!this.indicatorRight && !this.indicatorLeft)
                            {
                                Game.Player.Character.CurrentVehicle.IsLeftIndicatorLightOn = false;
                                Game.Player.Character.CurrentVehicle.IsLeftIndicatorLightOn = false;
                            }
                        }
                        this.indicatorSignal = this.indicatorLeft || this.indicatorRight || this.hazardLights;
                        this.enterVehicleAgain = true;
                    }
                }
                int num5;
                if (this.drawIcon)
                {
                    if (Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, new InputArgument[3]
                    {
              (InputArgument) Game.Player.Character,
              (InputArgument) Game.Player.Character.CurrentVehicle,
              (InputArgument) false
                    }) && (Entity)Game.Player.Character.CurrentVehicle != (Entity)null && !Function.Call<bool>(Hash.IS_RADAR_HIDDEN, Array.Empty<InputArgument>()) && !Function.Call<bool>(Hash.IS_HUD_HIDDEN, Array.Empty<InputArgument>()) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING, Array.Empty<InputArgument>()))
                    {
                        num5 = !GTA.UI.Screen.IsFadedOut ? 1 : 0;
                        goto label_130;
                    }
                }
                num5 = 0;
            label_130:
                if (num5 != 0)
                {
                    ++this.i2;
                    if ((double)this.i2 == (double)this.TickRate)
                    {
                        this.i2 = 0;
                        if (File.Exists("scripts\\CarControlV\\blank.png"))
                            new CustomSprite(".\\Scripts\\CarControlV\\blank.png", new Size(this.Width, this.Height), new Point(this.x, this.y));
                        else
                            GTA.UI.Screen.ShowSubtitle("blank.png was not found");
                        if (Game.Player.Character.CurrentVehicle.IsEngineRunning)
                        {
                            if (this.indicatorLeft)
                            {
                                if (File.Exists("scripts\\CarControlV\\indicatorLeft.png"))
                                    new CustomSprite(".\\Scripts\\CarControlV\\indicatorLeft.png", new Size(this.Width, this.Height), new Point(this.x, this.y));
                                else
                                    GTA.UI.Screen.ShowSubtitle("indicatorLeft.png was not found");
                            }
                            if (this.indicatorRight)
                            {
                                if (File.Exists("scripts\\CarControlV\\indicatorRight.png"))
                                    new CustomSprite(".\\Scripts\\CarControlV\\indicatorRight.png", new Size(this.Width, this.Height), new Point(this.x, this.y));
                                else
                                    GTA.UI.Screen.ShowSubtitle("indicatorRight.png was not found");
                            }
                            if (this.hazardLights)
                            {
                                if (File.Exists("scripts\\CarControlV\\hazardLights.png"))
                                    new CustomSprite(".\\Scripts\\CarControlV\\hazardLights.png", new Size(this.Width, this.Height), new Point(this.x, this.y));
                                else
                                    GTA.UI.Screen.ShowSubtitle("hazardLights.png was not found");
                            }
                        }
                    }
                }
            }
        }
        else
            this.enterVehicleAgain = false;
        if (this.PlaySounds)
        {
            int num;
            if (this.indicatorSignal)
            {
                if (Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, new InputArgument[3]
                {
            (InputArgument) Game.Player.Character,
            (InputArgument) Game.Player.Character.CurrentVehicle,
            (InputArgument) false
                }))
                {
                    num = (Entity)Game.Player.Character.CurrentVehicle != (Entity)null ? 1 : 0;
                    goto label_162;
                }
            }
            num = 0;
        label_162:
            if (num != 0)
            {
                ++this.i2;
                if ((double)this.i2 == (double)this.TickRate)
                {
                    this.i2 = 0;
                    this.IndicatorTickSoundEffects();
                }
            }
        }
    }

    private void IndicatorSoundControl()
    {
        if (this.PlaySounds)
        {
            int num;
            if (this.indicatorSignal)
            {
                if (Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, new InputArgument[3]
                {
            (InputArgument) Game.Player.Character,
            (InputArgument) Game.Player.Character.CurrentVehicle,
            (InputArgument) false
                }))
                {
                    num = (Entity)Game.Player.Character.CurrentVehicle != (Entity)null ? 1 : 0;
                    goto label_162;
                }
            }
            num = 0;
        label_162:
            if (num != 0)
            {
                ++this.i2;
                if ((double)this.i2 == (double)this.TickRate)
                {
                    this.i2 = 0;
                    this.IndicatorTickSoundEffects();
                }
            }
        }
    }

    //Seatbelt segment:

    private void Seatbelt(object sender, EventArgs e)
    {

        if (this.ControlsDisabled)
            Game.DisableAllControlsThisFrame();
        if (Game.GameTime > this.DisableControlsTime)
        {
            this.ControlsDisabled = false;
            Function.Call(Hash.TRIGGER_SCREENBLUR_FADE_OUT, (InputArgument)500f);
        }
        if (!this.CanWeUse2((Entity)Game.Player.Character.CurrentVehicle))
        {

            if (!Game.Player.Character.CanFlyThroughWindscreen)
                Game.Player.Character.CanFlyThroughWindscreen = true;
        }
        if (Game.GameTime <= this.GameTimeRef3 + 100)
            return;
        if (this.CanWeUse2((Entity)Game.Player.Character.CurrentVehicle))
        {
            Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
            float num1 = Math.Abs(Game.Player.Character.CurrentVehicle.Velocity.Length() - this.OldSpeed);
            int num2 = 3;
            Model model = currentVehicle.Model;
            if (!model.IsBike)
            {
                model = currentVehicle.Model;
                if (!model.IsBicycle)
                    goto label_18;
            }
            num2 = 5;
        label_18:
            if ((double)Math.Abs(num1) > (double)num2 && (Game.Player.Character.CurrentVehicle.HasCollided))
            {
                int num3 = (int)num1;
                if (Game.Player.Character.GetConfigFlag(32))
                {
                    if (!Game.Player.IsInvincible)
                    {
                        if (this.UseArmorEffects)
                        {

                            if (Game.Player.Character.Armor > 0)
                                GTA.UI.Screen.StartEffect(GTA.UI.ScreenEffect.SwitchShortMichaelIn, (int)500f, false);
                            //
                            else
                                GTA.UI.Screen.StartEffect(GTA.UI.ScreenEffect.SwitchShortTrevorIn, (int)500f, false);
                            //
                        }
                        else
                            GTA.UI.Screen.StartEffect(GTA.UI.ScreenEffect.SwitchShortTrevorIn, (int)500f, false);                  
                    }
                    if (currentVehicle.IsUpsideDown)
                    {
                        num3 *= 3;
                        if (!Game.Player.IsInvincible)
                            Game.Player.Character.ApplyDamage(Math.Abs(num3));
                    }
                    else if (!Game.Player.IsInvincible)
                    {
                        num3 *= 2;
                        Game.Player.Character.ApplyDamage(Math.Abs(num3));
                    }
                    if (num3 > 10 && !Game.Player.Character.IsWearingHelmet)
                    {
                        this.ControlsDisabled = true;
                        this.DisableControlsTime = Game.GameTime + num3 * 1000 / 10;
                        Function.Call(Hash.TRIGGER_SCREENBLUR_FADE_IN, (InputArgument)1);
                    }
                }
                else if (num3 > 30 && !Game.Player.Character.IsWearingHelmet)
                {
                    num3 *= 2;
                    Game.Player.Character.ApplyDamage(Math.Abs(num3));
                    this.ControlsDisabled = true;
                    this.DisableControlsTime = Game.GameTime + 1000;
                    Function.Call(Hash.TRIGGER_SCREENBLUR_FADE_IN, (InputArgument)1);
                }
            }
            this.OldSpeed = Game.Player.Character.CurrentVehicle.Speed;
        }
        else if ((double)this.OldSpeed > 0.0)
            this.OldSpeed = 0.0f;
        this.GameTimeRef3 = Game.GameTime;
    }


    private void HandleManualSeatbelt()
    {
        if (!this.CanWeUse2((Entity)Game.Player.Character.CurrentVehicle))
            return;

            Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
            if (currentVehicle.Model.IsBike || currentVehicle.Model.IsBicycle)
                return;

            if (Game.Player.Character.CanFlyThroughWindscreen)
            {

                Game.Player.Character.CanFlyThroughWindscreen = false;
                Game.Player.Character.Task.ClearAll();
                Game.Player.Character.Task.PlayAnimation("skydive@parachute@", "chute_off", 3.5f, 2f, -1, AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary, -1f);
                PlaySeatbeltON();
                Wait(1000);
                Function.Call(Hash.STOP_ENTITY_ANIM, (InputArgument)(Entity)Game.Player.Character, (InputArgument)"chute_off", (InputArgument)"skydive@parachute@", (InputArgument)3);


            }
            else if (!Game.Player.Character.CanFlyThroughWindscreen)
            {

                Game.Player.Character.CanFlyThroughWindscreen = true;
                Game.Player.Character.Task.ClearAll();
                Game.Player.Character.Task.PlayAnimation("skydive@parachute@", "chute_off", 1.3f, 3f, -1, AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary, -1f);
                PlaySeatbeltOFF();
                Wait(1000);
                Function.Call(Hash.STOP_ENTITY_ANIM, (InputArgument)(Entity)Game.Player.Character, (InputArgument)"chute_off", (InputArgument)"skydive@parachute@", (InputArgument)3);

            }
        
    }

    private void PlaySeatbeltON()
    {
        if (!File.Exists("./scripts/CarControlV/seatbelton.mp3"))
        {
            GTA.UI.Screen.ShowSubtitle("~b~SeatbeltV: ~w~a Seatbelt Sound could not be played More information below");
            GTA.UI.Screen.ShowSubtitle("~b~SeatbeltV: ~r~Error: ~w~You are missing the  'on.mp3'  file, please Consider Reinstalling SeatbeltV~w~, If you need any further help please message me at ~y~GTA5Mods.com");
        }
        if (!File.Exists("./scripts/CarControlV/seatbelton.mp3"))
            return;
        this.reader = new NAudio.Wave.Mp3FileReader("./scripts/CarControlV/seatbelton.mp3");
        this.streaming = new NAudio.Wave.WaveChannel32((WaveStream)this.reader);
        this.output = new DirectSoundOut();
        this.output.Init((NAudio.Wave.IWaveProvider)this.streaming);
        this.streaming.Volume = 1f;
        this.output.Play();
    }

    private void PlaySeatbeltOFF()
    {
        if (!File.Exists("./scripts/CarControlV/seatbeltoff.mp3"))
        {
            GTA.UI.Screen.ShowSubtitle("~b~SeatbeltV: ~w~a Seatbelt Sound could not be played More information below");
            GTA.UI.Screen.ShowSubtitle("~b~SeatbeltV: ~r~Error: ~w~You are missing the 'off.mp3'  file, please Consider Reinstalling SeatbeltV~w~, If you need any further help please message me at ~y~GTA5Mods.com");
        }
        if (!File.Exists("./scripts/CarControlV/seatbeltoff.mp3"))
            return;
        this.reader = new NAudio.Wave.Mp3FileReader("./scripts/CarControlV/seatbeltoff.mp3");
        this.streaming = new NAudio.Wave.WaveChannel32((WaveStream)this.reader);
        this.output = new NAudio.Wave.DirectSoundOut();
        this.output.Init((NAudio.Wave.IWaveProvider)this.streaming);
        this.streaming.Volume = 1f;
        this.output.Play();
    }



    private void WarnPlayer(string script_name, string title, string message)
    {
        Function.Call(Hash.SET_WARNING_MESSAGE, (InputArgument)"STRING");
        Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (InputArgument)message);
        Function.Call(Hash.SET_WARNING_MESSAGE, (InputArgument)"CHAR_SOCIAL_CLUB", (InputArgument)"CHAR_SOCIAL_CLUB", (InputArgument)true, (InputArgument)0, (InputArgument)title, (InputArgument)("~b~" + script_name));

    }

    private bool CanWeUse2(Entity entity) => entity != (Entity)null && entity.Exists();

    private void LoadSettings()
    {
        if (File.Exists("scripts\\\\seatbelt.ini"))
        {
            ScriptSettings scriptSettings = ScriptSettings.Load("scripts\\seatbelt.ini");
            this.UseManualSeatbelt = scriptSettings.GetValue<bool>("SETTINGS", "UseManualSeatbelt", true);
            this.UseAutoSeatbelt = scriptSettings.GetValue<bool>("SETTINGS", "UseAutoSeatbelt", true);
            this.UseArmorEffects = scriptSettings.GetValue<bool>("SETTINGS", "UseArmorEffects", true);
        }
        else
            WarnPlayer(this.ScriptName + " " + this.ScriptVer, "CONFIG NOT FOUND", "A configuration file for ~y~" + this.ScriptName + "~w~ hasn't been found.");
    }

    //Misc Functions and Enums

    public enum Subtask
    {
        AIMED_SHOOTING_ON_FOOT = 4,
        GETTING_UP = 16,
        MOVING_ON_FOOT_NO_COMBAT = 35,
        MOVING_ON_FOOT_COMBAT = 38,
        USING_LADDER = 47,
        CLIMBING = 50,
        GETTING_OFF_SOMETHING = 51,
        SWAPPING_WEAPON = 56,
        REMOVING_HELMET = 92,
        DEAD = 97,
        MELEE_COMBAT = 130,
        HITTING_MELEE = 130,
        SITTING_IN_VEHICLE = 150,
        DRIVING_WANDERING = 151,
        EXITING_VEHICLE = 152,

        ENTERING_VEHICLE_GENERAL = 160,
        ENTERING_VEHICLE_BREAKING_WINDOW = 161,
        ENTERING_VEHICLE_OPENING_DOOR = 162,
        ENTERING_VEHICLE_ENTERING = 163,
        ENTERING_VEHICLE_CLOSING_DOOR = 164,

        EXITING_VEHICLE_OPENING_DOOR_EXITING = 167,
        EXITING_VEHICLE_CLOSING_DOOR = 168,
        DRIVING_GOING_TO_DESTINATION_OR_ESCORTING = 169,
        USING_MOUNTED_WEAPON = 199,
        AIMING_THROWABLE = 289,
        AIMING_GUN = 290,
        AIMING_PREVENTED_BY_OBSTACLE = 299,
        IN_COVER_GENERAL = 287,
        IN_COVER_FULLY_IN_COVER = 288,

        RELOADING = 298,

        RUNNING_TO_COVER = 300,
        IN_COVER_TRANSITION_TO_AIMING_FROM_COVER = 302,
        IN_COVER_TRANSITION_FROM_AIMING_FROM_COVER = 303,
        IN_COVER_BLIND_FIRE = 304,

        PARACHUTING = 334,
        PUTTING_OFF_PARACHUTE = 336,

        JUMPING_OR_CLIMBING_GENERAL = 420,
        JUMPING_AIR = 421,
        JUMPING_FINISHING_JUMP = 422,
    }
    private enum Inputs
    {
        INPUT_NEXT_CAMERA,
        INPUT_LOOK_LR,
        INPUT_LOOK_UD,
        INPUT_LOOK_UP_ONLY,
        INPUT_LOOK_DOWN_ONLY,
        INPUT_LOOK_LEFT_ONLY,
        INPUT_LOOK_RIGHT_ONLY,
        INPUT_CINEMATIC_SLOWMO,
        INPUT_SCRIPTED_FLY_UD,
        INPUT_SCRIPTED_FLY_LR,
        INPUT_SCRIPTED_FLY_ZUP,
        INPUT_SCRIPTED_FLY_ZDOWN,
        INPUT_WEAPON_WHEEL_UD,
        INPUT_WEAPON_WHEEL_LR,
        INPUT_WEAPON_WHEEL_NEXT,
        INPUT_WEAPON_WHEEL_PREV,
        INPUT_SELECT_NEXT_WEAPON,
        INPUT_SELECT_PREV_WEAPON,
        INPUT_SKIP_CUTSCENE,
        INPUT_CHARACTER_WHEEL,
        INPUT_MULTIPLAYER_INFO,
        INPUT_SPRINT,
        INPUT_JUMP,
        INPUT_ENTER,
        INPUT_ATTACK,
        INPUT_AIM,
        INPUT_LOOK_BEHIND,
        INPUT_PHONE,
        INPUT_SPECIAL_ABILITY,
        INPUT_SPECIAL_ABILITY_SECONDARY,
        INPUT_MOVE_LR,
        INPUT_MOVE_UD,
        INPUT_MOVE_UP_ONLY,
        INPUT_MOVE_DOWN_ONLY,
        INPUT_MOVE_LEFT_ONLY,
        INPUT_MOVE_RIGHT_ONLY,
        INPUT_DUCK,
        INPUT_SELECT_WEAPON,
        INPUT_PICKUP,
        INPUT_SNIPER_ZOOM,
        INPUT_SNIPER_ZOOM_IN_ONLY,
        INPUT_SNIPER_ZOOM_OUT_ONLY,
        INPUT_SNIPER_ZOOM_IN_SECONDARY,
        INPUT_SNIPER_ZOOM_OUT_SECONDARY,
        INPUT_COVER,
        INPUT_RELOAD,
        INPUT_TALK,
        INPUT_DETONATE,
        INPUT_HUD_SPECIAL,
        INPUT_ARREST,
        INPUT_ACCURATE_AIM,
        INPUT_CONTEXT,
        INPUT_CONTEXT_SECONDARY,
        INPUT_WEAPON_SPECIAL,
        INPUT_WEAPON_SPECIAL_TWO,
        INPUT_DIVE,
        INPUT_DROP_WEAPON,
        INPUT_DROP_AMMO,
        INPUT_THROW_GRENADE,
        INPUT_VEH_MOVE_LR,
        INPUT_VEH_MOVE_UD,
        INPUT_VEH_MOVE_UP_ONLY,
        INPUT_VEH_MOVE_DOWN_ONLY,
        INPUT_VEH_MOVE_LEFT_ONLY,
        INPUT_VEH_MOVE_RIGHT_ONLY,
        INPUT_VEH_SPECIAL,
        INPUT_VEH_GUN_LR,
        INPUT_VEH_GUN_UD,
        INPUT_VEH_AIM,
        INPUT_VEH_ATTACK,
        INPUT_VEH_ATTACK2,
        INPUT_VEH_ACCELERATE,
        INPUT_VEH_BRAKE,
        INPUT_VEH_DUCK,
        INPUT_VEH_HEADLIGHT,
        INPUT_VEH_EXIT,
        INPUT_VEH_HANDBRAKE,
        INPUT_VEH_HOTWIRE_LEFT,
        INPUT_VEH_HOTWIRE_RIGHT,
        INPUT_VEH_LOOK_BEHIND,
        INPUT_VEH_CIN_CAM,
        INPUT_VEH_NEXT_RADIO,
        INPUT_VEH_PREV_RADIO,
        INPUT_VEH_NEXT_RADIO_TRACK,
        INPUT_VEH_PREV_RADIO_TRACK,
        INPUT_VEH_RADIO_WHEEL,
        INPUT_VEH_HORN,
        INPUT_VEH_FLY_THROTTLE_UP,
        INPUT_VEH_FLY_THROTTLE_DOWN,
        INPUT_VEH_FLY_YAW_LEFT,
        INPUT_VEH_FLY_YAW_RIGHT,
        INPUT_VEH_PASSENGER_AIM,
        INPUT_VEH_PASSENGER_ATTACK,
        INPUT_VEH_SPECIAL_ABILITY_FRANKLIN,
        INPUT_VEH_STUNT_UD,
        INPUT_VEH_CINEMATIC_UD,
        INPUT_VEH_CINEMATIC_UP_ONLY,
        INPUT_VEH_CINEMATIC_DOWN_ONLY,
        INPUT_VEH_CINEMATIC_LR,
        INPUT_VEH_SELECT_NEXT_WEAPON,
        INPUT_VEH_SELECT_PREV_WEAPON,
        INPUT_VEH_ROOF,
        INPUT_VEH_JUMP,
        INPUT_VEH_GRAPPLING_HOOK,
        INPUT_VEH_SHUFFLE,
        INPUT_VEH_DROP_PROJECTILE,
        INPUT_VEH_MOUSE_CONTROL_OVERRIDE,
        INPUT_VEH_FLY_ROLL_LR,
        INPUT_VEH_FLY_ROLL_LEFT_ONLY,
        INPUT_VEH_FLY_ROLL_RIGHT_ONLY,
        INPUT_VEH_FLY_PITCH_UD,
        INPUT_VEH_FLY_PITCH_UP_ONLY,
        INPUT_VEH_FLY_PITCH_DOWN_ONLY,
        INPUT_VEH_FLY_UNDERCARRIAGE,
        INPUT_VEH_FLY_ATTACK,
        INPUT_VEH_FLY_SELECT_NEXT_WEAPON,
        INPUT_VEH_FLY_SELECT_PREV_WEAPON,
        INPUT_VEH_FLY_SELECT_TARGET_LEFT,
        INPUT_VEH_FLY_SELECT_TARGET_RIGHT,
        INPUT_VEH_FLY_VERTICAL_FLIGHT_MODE,
        INPUT_VEH_FLY_DUCK,
        INPUT_VEH_FLY_ATTACK_CAMERA,
        INPUT_VEH_FLY_MOUSE_CONTROL_OVERRIDE,
        INPUT_VEH_SUB_TURN_LR,
        INPUT_VEH_SUB_TURN_LEFT_ONLY,
        INPUT_VEH_SUB_TURN_RIGHT_ONLY,
        INPUT_VEH_SUB_PITCH_UD,
        INPUT_VEH_SUB_PITCH_UP_ONLY,
        INPUT_VEH_SUB_PITCH_DOWN_ONLY,
        INPUT_VEH_SUB_THROTTLE_UP,
        INPUT_VEH_SUB_THROTTLE_DOWN,
        INPUT_VEH_SUB_ASCEND,
        INPUT_VEH_SUB_DESCEND,
        INPUT_VEH_SUB_TURN_HARD_LEFT,
        INPUT_VEH_SUB_TURN_HARD_RIGHT,
        INPUT_VEH_SUB_MOUSE_CONTROL_OVERRIDE,
        INPUT_VEH_PUSHBIKE_PEDAL,
        INPUT_VEH_PUSHBIKE_SPRINT,
        INPUT_VEH_PUSHBIKE_FRONT_BRAKE,
        INPUT_VEH_PUSHBIKE_REAR_BRAKE,
        INPUT_MELEE_ATTACK_LIGHT,
        INPUT_MELEE_ATTACK_HEAVY,
        INPUT_MELEE_ATTACK_ALTERNATE,
        INPUT_MELEE_BLOCK,
        INPUT_PARACHUTE_DEPLOY,
        INPUT_PARACHUTE_DETACH,
        INPUT_PARACHUTE_TURN_LR,
        INPUT_PARACHUTE_TURN_LEFT_ONLY,
        INPUT_PARACHUTE_TURN_RIGHT_ONLY,
        INPUT_PARACHUTE_PITCH_UD,
        INPUT_PARACHUTE_PITCH_UP_ONLY,
        INPUT_PARACHUTE_PITCH_DOWN_ONLY,
        INPUT_PARACHUTE_BRAKE_LEFT,
        INPUT_PARACHUTE_BRAKE_RIGHT,
        INPUT_PARACHUTE_SMOKE,
        INPUT_PARACHUTE_PRECISION_LANDING,
        INPUT_MAP,
        INPUT_SELECT_WEAPON_UNARMED,
        INPUT_SELECT_WEAPON_MELEE,
        INPUT_SELECT_WEAPON_HANDGUN,
        INPUT_SELECT_WEAPON_SHOTGUN,
        INPUT_SELECT_WEAPON_SMG,
        INPUT_SELECT_WEAPON_AUTO_RIFLE,
        INPUT_SELECT_WEAPON_SNIPER,
        INPUT_SELECT_WEAPON_HEAVY,
        INPUT_SELECT_WEAPON_SPECIAL,
        INPUT_SELECT_CHARACTER_MICHAEL,
        INPUT_SELECT_CHARACTER_FRANKLIN,
        INPUT_SELECT_CHARACTER_TREVOR,
        INPUT_SELECT_CHARACTER_MULTIPLAYER,
        INPUT_SAVE_REPLAY_CLIP,
        INPUT_SPECIAL_ABILITY_PC,
        INPUT_CELLPHONE_UP,
        INPUT_CELLPHONE_DOWN,
        INPUT_CELLPHONE_LEFT,
        INPUT_CELLPHONE_RIGHT,
        INPUT_CELLPHONE_SELECT,
        INPUT_CELLPHONE_CANCEL,
        INPUT_CELLPHONE_OPTION,
        INPUT_CELLPHONE_EXTRA_OPTION,
        INPUT_CELLPHONE_SCROLL_FORWARD,
        INPUT_CELLPHONE_SCROLL_BACKWARD,
        INPUT_CELLPHONE_CAMERA_FOCUS_LOCK,
        INPUT_CELLPHONE_CAMERA_GRID,
        INPUT_CELLPHONE_CAMERA_SELFIE,
        INPUT_CELLPHONE_CAMERA_DOF,
        INPUT_CELLPHONE_CAMERA_EXPRESSION,
        INPUT_FRONTEND_DOWN,
        INPUT_FRONTEND_UP,
        INPUT_FRONTEND_LEFT,
        INPUT_FRONTEND_RIGHT,
        INPUT_FRONTEND_RDOWN,
        INPUT_FRONTEND_RUP,
        INPUT_FRONTEND_RLEFT,
        INPUT_FRONTEND_RRIGHT,
        INPUT_FRONTEND_AXIS_X,
        INPUT_FRONTEND_AXIS_Y,
        INPUT_FRONTEND_RIGHT_AXIS_X,
        INPUT_FRONTEND_RIGHT_AXIS_Y,
        INPUT_FRONTEND_PAUSE,
        INPUT_FRONTEND_PAUSE_ALTERNATE,
        INPUT_FRONTEND_ACCEPT,
        INPUT_FRONTEND_CANCEL,
        INPUT_FRONTEND_X,
        INPUT_FRONTEND_Y,
        INPUT_FRONTEND_LB,
        INPUT_FRONTEND_RB,
        INPUT_FRONTEND_LT,
        INPUT_FRONTEND_RT,
        INPUT_FRONTEND_LS,
        INPUT_FRONTEND_RS,
        INPUT_FRONTEND_LEADERBOARD,
        INPUT_FRONTEND_SOCIAL_CLUB,
        INPUT_FRONTEND_SOCIAL_CLUB_SECONDARY,
        INPUT_FRONTEND_DELETE,
        INPUT_FRONTEND_ENDSCREEN_ACCEPT,
        INPUT_FRONTEND_ENDSCREEN_EXPAND,
        INPUT_FRONTEND_SELECT,
        INPUT_SCRIPT_LEFT_AXIS_X,
        INPUT_SCRIPT_LEFT_AXIS_Y,
        INPUT_SCRIPT_RIGHT_AXIS_X,
        INPUT_SCRIPT_RIGHT_AXIS_Y,
        INPUT_SCRIPT_RUP,
        INPUT_SCRIPT_RDOWN,
        INPUT_SCRIPT_RLEFT,
        INPUT_SCRIPT_RRIGHT,
        INPUT_SCRIPT_LB,
        INPUT_SCRIPT_RB,
        INPUT_SCRIPT_LT,
        INPUT_SCRIPT_RT,
        INPUT_SCRIPT_LS,
        INPUT_SCRIPT_RS,
        INPUT_SCRIPT_PAD_UP,
        INPUT_SCRIPT_PAD_DOWN,
        INPUT_SCRIPT_PAD_LEFT,
        INPUT_SCRIPT_PAD_RIGHT,
        INPUT_SCRIPT_SELECT,
        INPUT_CURSOR_ACCEPT,
        INPUT_CURSOR_CANCEL,
        INPUT_CURSOR_X,
        INPUT_CURSOR_Y,
        INPUT_CURSOR_SCROLL_UP,
        INPUT_CURSOR_SCROLL_DOWN,
        INPUT_ENTER_CHEAT_CODE,
        INPUT_INTERACTION_MENU,
        INPUT_MP_TEXT_CHAT_ALL,
        INPUT_MP_TEXT_CHAT_TEAM,
        INPUT_MP_TEXT_CHAT_FRIENDS,
        INPUT_MP_TEXT_CHAT_CREW,
        INPUT_PUSH_TO_TALK,
        INPUT_CREATOR_LS,
        INPUT_CREATOR_RS,
        INPUT_CREATOR_LT,
        INPUT_CREATOR_RT,
        INPUT_CREATOR_MENU_TOGGLE,
        INPUT_CREATOR_ACCEPT,
        INPUT_CREATOR_DELETE,
        INPUT_ATTACK2,
        INPUT_RAPPEL_JUMP,
        INPUT_RAPPEL_LONG_JUMP,
        INPUT_RAPPEL_SMASH_WINDOW,
        INPUT_PREV_WEAPON,
        INPUT_NEXT_WEAPON,
        INPUT_MELEE_ATTACK1,
        INPUT_MELEE_ATTACK2,
        INPUT_WHISTLE,
        INPUT_MOVE_LEFT,
        INPUT_MOVE_RIGHT,
        INPUT_MOVE_UP,
        INPUT_MOVE_DOWN,
        INPUT_LOOK_LEFT,
        INPUT_LOOK_RIGHT,
        INPUT_LOOK_UP,
        INPUT_LOOK_DOWN,
        INPUT_SNIPER_ZOOM_IN,
        INPUT_SNIPER_ZOOM_OUT,
        INPUT_SNIPER_ZOOM_IN_ALTERNATE,
        INPUT_SNIPER_ZOOM_OUT_ALTERNATE,
        INPUT_VEH_MOVE_LEFT,
        INPUT_VEH_MOVE_RIGHT,
        INPUT_VEH_MOVE_UP,
        INPUT_VEH_MOVE_DOWN,
        INPUT_VEH_GUN_LEFT,
        INPUT_VEH_GUN_RIGHT,
        INPUT_VEH_GUN_UP,
        INPUT_VEH_GUN_DOWN,
        INPUT_VEH_LOOK_LEFT,
        INPUT_VEH_LOOK_RIGHT,
        INPUT_REPLAY_START_STOP_RECORDING,
        INPUT_REPLAY_START_STOP_RECORDING_SECONDARY,
        INPUT_SCALED_LOOK_LR,
        INPUT_SCALED_LOOK_UD,
        INPUT_SCALED_LOOK_UP_ONLY,
        INPUT_SCALED_LOOK_DOWN_ONLY,
        INPUT_SCALED_LOOK_LEFT_ONLY,
        INPUT_SCALED_LOOK_RIGHT_ONLY,
        INPUT_REPLAY_MARKER_DELETE,
        INPUT_REPLAY_CLIP_DELETE,
        INPUT_REPLAY_PAUSE,
        INPUT_REPLAY_REWIND,
        INPUT_REPLAY_FFWD,
        INPUT_REPLAY_NEWMARKER,
        INPUT_REPLAY_RECORD,
        INPUT_REPLAY_SCREENSHOT,
        INPUT_REPLAY_HIDEHUD,
        INPUT_REPLAY_STARTPOINT,
        INPUT_REPLAY_ENDPOINT,
        INPUT_REPLAY_ADVANCE,
        INPUT_REPLAY_BACK,
        INPUT_REPLAY_TOOLS,
        INPUT_REPLAY_RESTART,
        INPUT_REPLAY_SHOWHOTKEY,
        INPUT_REPLAY_CYCLEMARKERLEFT,
        INPUT_REPLAY_CYCLEMARKERRIGHT,
        INPUT_REPLAY_FOVINCREASE,
        INPUT_REPLAY_FOVDECREASE,
        INPUT_REPLAY_CAMERAUP,
        INPUT_REPLAY_CAMERADOWN,
        INPUT_REPLAY_SAVE,
        INPUT_REPLAY_TOGGLETIME,
        INPUT_REPLAY_TOGGLETIPS,
        INPUT_REPLAY_PREVIEW,
        INPUT_REPLAY_TOGGLE_TIMELINE,
        INPUT_REPLAY_TIMELINE_PICKUP_CLIP,
        INPUT_REPLAY_TIMELINE_DUPLICATE_CLIP,
        INPUT_REPLAY_TIMELINE_PLACE_CLIP,
        INPUT_REPLAY_CTRL,
        INPUT_REPLAY_TIMELINE_SAVE,
        INPUT_REPLAY_PREVIEW_AUDIO,
        INPUT_VEH_DRIVE_LOOK,
        INPUT_VEH_DRIVE_LOOK2,
        INPUT_VEH_FLY_ATTACK2,
        INPUT_RADIO_WHEEL_UD,
        INPUT_RADIO_WHEEL_LR,
        INPUT_VEH_SLOWMO_UD,
        INPUT_VEH_SLOWMO_UP_ONLY,
        INPUT_VEH_SLOWMO_DOWN_ONLY,
        INPUT_MAP_POI,
    }

    private void DisplayHelpTextThisFrame(string text)
    {
        Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, new InputArgument[1]
        {
          (InputArgument) "STRING"
        });
        Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1]
        {
          (InputArgument) text
        });
        Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, new InputArgument[4]
        {
          (InputArgument) 0,
          (InputArgument) 0,
          (InputArgument) 1,
          (InputArgument) (-1)
        });
    }

    public Boolean IsSubttaskActive(Ped ped, Subtask task)
    {
        return Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, ped, (int)task);
    }
}
