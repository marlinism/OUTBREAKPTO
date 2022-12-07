using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    private CameraController cc;

    private ControlMode control;

    // Properties
    public static GameSystem Inst
    {
        get { return instance; }
    }
    public CameraController CameraControl
    {
        get { return cc; }
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

        Assert.IsNotNull(cc);

        InputUser.onChange += ControlsChanged;
    }

    public void DeathReload()
    {
        StartCoroutine(DeathReloadCoroutine());
    }

    IEnumerator DeathReloadCoroutine()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield break;
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
