using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneController : MonoBehaviour
{
	public GameObject helpMarker;
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
	}
}
