using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ky;
public class OptionsMenu : Singleton<OptionsMenu>
{
	bool active = true;
	void Start()
	{
		Toggle();
	}
    public void Toggle ()
	{
		active = !active;
		gameObject.SetActive(active);
	}
	public void ReturnToMain ()
	{
		SceneManager.LoadScene("Main Menu");
	}
}
