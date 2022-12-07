using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneController : MonoBehaviour
{
	public GameObject helpMarker;
	public GameObject bgMusic;
	void Start()
	{
		if (!StateManager.tutorialState)
		{
			helpMarker.SetActive(false);
		}
		else
		{
			helpMarker.SetActive(true);
		}

		if (!StateManager.musicState)
		{
			bgMusic.SetActive(false);
		}
		else
		{
			bgMusic.SetActive(true);
		}
	}
}
