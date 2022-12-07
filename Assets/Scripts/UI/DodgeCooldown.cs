using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeCooldown : MonoBehaviour
{
	public Image ImageCooldown;
	private bool isCooldown;
	public float cooldownTime = 2.0f;
	private float timer = 0.0f;

	// Start is called before the first frame update
	void Start()
	{
		ImageCooldown.fillAmount = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			UseAbility();
		}
		if (isCooldown)
		{
			ApplyCooldown();
		}
	}

	private void ApplyCooldown()
	{
		timer = timer - Time.deltaTime;

		if (timer < 0.0f)
		{
			isCooldown = false;
			ImageCooldown.fillAmount = 0.0f;
		} else
		{
			ImageCooldown.fillAmount = timer / cooldownTime;
		}
	}

	private void UseAbility()
	{
		if (!isCooldown)
		{
			isCooldown = true;
			timer = cooldownTime;
		}
	}
}
