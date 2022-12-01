using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public enum ControlMode
{
    Keyboard,
    Gamepad
}

public class GameSystem : MonoBehaviour
{
    // Singleton instance
    private static GameSystem instance;

    private ControlMode control;

    // Properties
    public static GameSystem Inst
    {
        get { return instance; }
    }
    public ControlMode Control
    {
        get { return control; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InputUser.onChange += ControlsChanged;
    }

    private void ControlsChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change != InputUserChange.ControlSchemeChanged)
        {
            return;
        }

        if (user.controlScheme.Value.name == "Gamepad")
        {
            control = ControlMode.Gamepad;
        }
        else
        {
            control = ControlMode.Keyboard;
        }
    }
}
