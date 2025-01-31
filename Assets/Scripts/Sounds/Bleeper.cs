using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ky;
public class Bleeper : Singleton<Bleeper>
{
    [SerializeField]AudioSource [] bleeps;
	[SerializeField]float startsFrom;
	void Start ()
	{
		foreach (AudioSource audio in bleeps)
		{
			audio.volume = MusicManager.soundsVolume;
		}
		
	}
	public void PlayRandomBleep ()
	{
		int random = UnityEngine.Random.Range(0,bleeps.Length);
		bleeps[random].Play();
		//bleeps[UnityEngine.Random.Range(0,bleeps.Length)].time = startsFrom;
		bleeps[random].time = startsFrom;
		
		//bleeps[random].pitch = UnityEngine.Random.Range(0.97f,1.03f);
	}
}
