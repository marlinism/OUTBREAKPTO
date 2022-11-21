using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum UIEffect
{
    ScreenCrack,
    BlackScreen
}

public class UIEffectsController : MonoBehaviour
{
    [SerializeField]
    private GameObject screenCrackEffect;

    [SerializeField]
    private GameObject blackScreenEffect;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(screenCrackEffect);
        Assert.IsNotNull(blackScreenEffect);
    }

    public void DisableAllEffects()
    {
        screenCrackEffect.SetActive(false);
        blackScreenEffect.SetActive(false);
    }

    public void EnableEffect(UIEffect effect)
    {
        switch (effect)
        {
            case UIEffect.ScreenCrack:
                screenCrackEffect.SetActive(true);
                return;

            case UIEffect.BlackScreen:
                blackScreenEffect.SetActive(true);
                return;
        }
    }

    public void DisableEffect(UIEffect effect)
    {
        switch (effect)
        {
            case UIEffect.ScreenCrack:
                screenCrackEffect.SetActive(false);
                return;

            case UIEffect.BlackScreen:
                blackScreenEffect.SetActive(false);
                return;
        }
    }
}
