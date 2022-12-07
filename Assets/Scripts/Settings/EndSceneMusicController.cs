using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneMusicController : MonoBehaviour
{
	public GameObject bgMusic;
	// Start is called before the first frame update
	void Start()
	{
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
