using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
  public Slider VolumeSlider;
  public Toggle TutorialToggle;
  void Start()
  {
    TutorialToggle.isOn = StateManager.tutorialState;
    VolumeSlider.value = StateManager.voulumeLevel;
	}
  public void ToggleTutorials(bool IsOn)
  {
    StateManager.tutorialState = IsOn;
  }

  public void UpdateVolume (float volume)
  {
    StateManager.voulumeLevel = volume;
  }
}
