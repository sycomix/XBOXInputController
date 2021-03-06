﻿// These codes are licensed under CC0.
// http://creativecommons.org/publicdomain/zero/1.0/deed.ja

using System.Collections;
using UnityEngine;

internal class XBOXInputController : InputController
{
#if WINDOWS_UWP
    private Gamepad controller;
#endif
    // Common
    private const string INPUT_HORIZONTAL = "Horizontal";
    private const string INPUT_VERTICAL = "Vertical";

    private const string INPUT_ACTION0 = "Action0";
    private const string INPUT_ACTION1 = "Action1";
    private const string INPUT_ACTION2 = "Action2";
    private const string INPUT_ACTION3 = "Action3";

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    // OSX
    private const string INPUT_HORIZONTAL_R = "Horizontal2";
    private const string INPUT_VERTICAL_R = "Vertical2";

    private const string INPUT_TRIGGER_L = "LeftTrigger";
    private const string INPUT_TRIGGER_R = "RightTrigger";
    
    private const string INPUT_DPAD_H = "DPADH";
    private const string INPUT_DPAD_V = "DPADV";

    private const string INPUT_LB = "LB_Mac";
    private const string INPUT_RB = "RB_Mac";
    private const string INPUT_VIEW = "View_Mac";
    private const string INPUT_MENU = "Menu_Mac";
   private const string INPUT_LSTICK_CLICK = "LStickClick";
    private const string INPUT_RSTICK_CLICK = "RStickClick";
#elif UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
    // Linux
    private const string INPUT_HORIZONTAL_R = "Horizontal2";
    private const string INPUT_VERTICAL_R = "Vertical2";

    private const string INPUT_TRIGGER_L = "LeftTrigger";
    private const string INPUT_TRIGGER_R = "RightTrigger";

    private const string INPUT_DPAD_H = "DPADH";
    private const string INPUT_DPAD_V = "DPADV";

    private const string INPUT_LB = "LB_Linux";
    private const string INPUT_RB = "RB_Linux";
    private const string INPUT_VIEW = "View_Linux";
    private const string INPUT_MENU = "Menu_Linux";
    private const string INPUT_LSTICK_CLICK = "LStickClick";
    private const string INPUT_RSTICK_CLICK = "RStickClick";
#else
    // Win
    private const string INPUT_HORIZONTAL_R = "Horizontal2";
    private const string INPUT_VERTICAL_R = "Vertical2";

    private const string INPUT_TRIGGER_L = "LeftTrigger";
    private const string INPUT_TRIGGER_R = "RightTrigger";

    private const string INPUT_DPAD_H = "DPADH";
    private const string INPUT_DPAD_V = "DPADV";

    private const string INPUT_LB = "LB";
    private const string INPUT_RB = "RB";
    private const string INPUT_VIEW = "View";
    private const string INPUT_MENU = "Menu";
    private const string INPUT_LSTICK_CLICK = "LStickClick";
    private const string INPUT_RSTICK_CLICK = "RStickClick";
#endif

    /// <summary>
    /// Property
    /// </summary>
    private bool  isReady;
    private float triggerL;
    private float triggerR;
    private float analogL_H;
    private float analogL_V;
    private float analogR_H;
    private float analogR_V;

    public float TriggerL { get => triggerL; }
    public float TriggerR { get => triggerR; }
    public float AnalogL_H { get => analogL_H; }
    public float AnalogL_V { get => analogL_V; }
    public float AnalogR_H { get => analogR_H; }
    public float AnalogR_V { get => analogR_V; }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private IEnumerator Start()
    {
        yield return StartCoroutine(InitializeManager());
        yield break;
    }

    /// <summary>
    /// Startから呼ぶ初期化
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitializeManager()
    {
        Debug.Log("InitializeManager()");

#if WINDOWS_UWP
        // Gamepadを探す
        if　(Gamepad.Gamepads.Count > 0) 
        {
            Debug.Log("Gamepad found.");
          　//  controller = Gamepad.Gamepads.First();
        } else
        {
            Debug.Log("Gamepad not found.");
        }
        // ゲームパッド追加時イベント処理を追加
        Gamepad.GamepadAdded += Gamepad_GamepadAdded;
#endif

        isReady = true;
        Debug.Log("isReady.");

        yield break;
    }

    /// <summary>
    /// パッド状態を取得
    /// </summary>
    /// <returns>入力状態Bit</returns>
    public override uint Poll()
    {
        uint pad = 0;

        ///////////////////////////////////////////////////////////////////
        // Test Code begin
        ///////////////////////////////////////////////////////////////////
        // 接続されているコントローラの名前を調べる
        var controllerNames = Input.GetJoystickNames();

        if (controllerNames.Length == 0)
        {
            // Steamコントローラの場合も０になる？
            Debug.Log("***Steam Controller?***");
        }
        else
        {
            // 一台もコントローラが接続されていなければエラー
            if (controllerNames[0] == "")
            {
                // ゲームコントローラは接続されていない
//                Debug.Log(controllerNames[0]);
            }
//            Debug.Log(controllerNames[0]);
/*
            if (controllerNames[0].Equals("Controller (XBOX 360 For Windows)"))
            {
                Debug.Log("***XBOX360***");

            }
            else
            if (controllerNames[0].Equals("Controller (Xbox One For Windows)"))
            {
                Debug.Log("***XBOXONE***");

            }
            else
            if (controllerNames[0].Equals("JC-PS101U"))
            {
                Debug.Log("***PlayStation1***");

            }
 */
        }
        ///////////////////////////////////////////////////////////////////
        // Test Code end
        ///////////////////////////////////////////////////////////////////

        // Analog Stick
        float vx = Input.GetAxis(INPUT_HORIZONTAL);
        float vy = Input.GetAxis(INPUT_VERTICAL);
        // Dead Zone
        if (Mathf.Abs(vx) < PAD_DEAD)
        {
            vx = 0;
        }
        if (Mathf.Abs(vy) < PAD_DEAD)
        {
            vy = 0;
        }
        analogL_H = vx;
        analogL_V = vy;

        // Right Analog Stick
        float rvx = Input.GetAxis(INPUT_HORIZONTAL_R);
        float rvy = Input.GetAxis(INPUT_VERTICAL_R);
        // Dead Zone
        if (Mathf.Abs(rvx) < PAD_DEAD)
        {
            rvx = 0;
        }
        if (Mathf.Abs(rvy) < PAD_DEAD)
        {
            rvy = 0;
        }
        analogR_H = rvx;
        analogR_V = rvy;

        // DPad
        float dvx = Input.GetAxis(INPUT_DPAD_H);
        float dvy = Input.GetAxis(INPUT_DPAD_V);
        // Dead Zone
        if (Mathf.Abs(dvx) < PAD_DEAD)
        {
            dvx = 0;
        }
        if (Mathf.Abs(dvy) < PAD_DEAD)
        {
            dvy = 0;
        }

        // Left Trigger
        triggerL = Input.GetAxis(INPUT_TRIGGER_L);
        // Right Trigger
        triggerR = Input.GetAxis(INPUT_TRIGGER_R);

        // Analog Left check
        if (vx < 0f)
        {
//            Debug.Log("ANALOG_LEFT");
            pad |= PAD_LEFT;
        }
        else if (vx > 0f)
        {
//            Debug.Log("ANALOG_RIGHT");
            pad |= PAD_RIGHT;
        }
        // Vertical reverse
        if (vy > 0f)
        {
//            Debug.Log("ANALOG_UP");
            pad |= PAD_UP;
        }
        else if (vy < 0f)
        {
//            Debug.Log("ANALOG_DOWN");
            pad |= PAD_DOWN;
        }

        // Analog Right check
        if (rvx < 0f)
        {
            Debug.Log("RIGHT_ANALOG_LEFT");
        }
        else if (rvx > 0f)
        {
            Debug.Log("RIGHT_ANALOG_RIGHT");
        }
        if (rvy < 0f)
        {
            Debug.Log("RIGHT_ANALOG_UP");
        }
        else if (rvy > 0f)
        {
            Debug.Log("RIGHT_ANALOG_DOWN");
        }


        // Trigger check
//        Debug.Log("tl=" + triggerL + " tr=" + triggerR);

        // DPAD check
        if (dvx < 0f)
        {
            pad |= PAD_LEFT;
        }
        else if (dvx > 0f)
        {
            pad |= PAD_RIGHT;
        }

        // 上下逆
        if (dvy > 0f)
        {
            pad |= PAD_UP;
        }
        else if (dvy < 0f)
        {
            pad |= PAD_DOWN;
        }

        // Buttons check
        if (Input.GetButton(INPUT_ACTION0))
        {
//            Debug.Log("A");
            pad |= PAD_BUTTON_A;
        }
        if (Input.GetButton(INPUT_ACTION1))
        {
//            Debug.Log("B");
            pad |= PAD_BUTTON_B;
        }
        if (Input.GetButton(INPUT_ACTION2))
        {
//            Debug.Log("X");
            pad |= PAD_BUTTON_X;
        }
        if (Input.GetButton(INPUT_ACTION3))
        {
//            Debug.Log("Y");
            pad |= PAD_BUTTON_Y;
        }
        if (Input.GetButton(INPUT_LB))
        {
//            Debug.Log("LB");
            pad |= PAD_BUTTON_LB;
        }
        if (Input.GetButton(INPUT_RB))
        {
//            Debug.Log("RB");
            pad |= PAD_BUTTON_RB;
        }
        if (Input.GetButton(INPUT_LSTICK_CLICK))
        {
            Debug.Log("LSTICK_CLICK");
        }
        if (Input.GetButton(INPUT_RSTICK_CLICK))
        {
            Debug.Log("RSTICK_CLICK");
        }
        // Start
        if (Input.GetButton(INPUT_MENU))
        {
//            Debug.Log("MENU");
            pad |= PAD_BUTTON_MENU;
        }
        // Back
        if (Input.GetButton(INPUT_VIEW))
        {
//            Debug.Log("VIEW");
            pad |= PAD_BUTTON_VIEW;
        }

        // ボタンが押された一瞬の状態を記録
        uint tmppad = pad;
        pad_trg = ~pad_bak & tmppad;
        pad_bak = tmppad;

        return pad;
    }


#if WINDOWS_UWP
    /// <summary>
    // ゲームパッド追加時のイベント処理
    /// </summary>
    private void Gamepad_GamepadAdded(object sender, Gamepad e)
    {
        controller = e;
        Debug.Log("Gamepad added");
    }
#endif
}
