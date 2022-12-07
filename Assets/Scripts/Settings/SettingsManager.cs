using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
  public Slider VolumeSlider;
  public Toggle TutorialToggle;
  public Toggle MusicToggle;
  public GameObject musicControl;
  void Start()
  {
    TutorialToggle.isOn = StateManager.tutorialState;
    VolumeSlider.value = StateManager.voulumeLevel;
    MusicToggle.isOn = StateManager.musicState; 
	}
  public void ToggleTutorials(bool IsOn)
  {
    StateManager.tutorialState = IsOn;
  }

  public void UpdateVolume (float volume)
  {
    StateManager.voulumeLevel = volume;
  }

  public void ToggleMusic(bool isOn)
  {
    StateManager.musicState = isOn;
		if (!StateManager.musicState)
		{
			musicControl.SetActive(false);
		}
		else
		{
			musicControl.SetActive(true);
		}
	}
}
