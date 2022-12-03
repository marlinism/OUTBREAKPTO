using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
  Resolution[] resolutions;
  public TMP_Dropdown resolutionDropdown;
  public TMP_Dropdown graphics;

  void Start()
  {
    int currGraphicsLevel = QualitySettings.GetQualityLevel();
    graphics.value = currGraphicsLevel;
    graphics.RefreshShownValue();
    resolutions = Screen.resolutions;
    resolutionDropdown.ClearOptions();

    List<string> optionsList = new List<string>();
    int currResIndex = 0;

    for(int i = 0; i < resolutions.Length; i++)
    {
      string option = resolutions[i].width + " x " + resolutions[i].height;
      optionsList.Add(option);

      if(resolutions[i].width == Screen.currentResolution.width &&
        resolutions[i].height == Screen.currentResolution.height)
      {
        currResIndex = i;
      }
    }

    resolutionDropdown.AddOptions(optionsList);
    resolutionDropdown.value = currResIndex;
    resolutionDropdown.RefreshShownValue();
  }
  public void SetQuality (int qualityIndex)
  {
    QualitySettings.SetQualityLevel (qualityIndex);
  }

  public void SetFullScreen (bool isFullScreen)
  {
    Screen.fullScreen = isFullScreen;
  }

  public void SetResolution (int resolutionIndex)
  {
    Resolution resolution = resolutions[resolutionIndex];
    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
  }
}
