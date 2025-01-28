using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeInItem : MonoBehaviour
{
	[SerializeField]Color initial,final;
	[SerializeField]float timeToFade;
	[SerializeField]bool gradual,looping;
	bool reversed;
	float timePassed;
	SpriteRenderer img;
    void Start()
    {
        img = gameObject.GetComponent<SpriteRenderer>();
		img.color = initial;
    }
	public void Setup(float time)
	{
		timeToFade = time;
	}
    void Update()
    {
		bool selfDestruct = false;
		if (reversed)
		{
			timePassed -= Time.deltaTime;
			if (timePassed <= 0){
				timePassed = 0;
				if (looping){
					reversed = false;
				}
				else 
				{
					selfDestruct = true;
				}
			}
			
		}
		else 
		{
			timePassed += Time.deltaTime;
			if (timePassed >= timeToFade){
				timePassed = timeToFade;
				if (looping){
					reversed = true;
				}
				else 
				{
					selfDestruct = true;
				}
			}
		}
		if (gradual)
		{
			img.color = (initial*(1-(timePassed/timeToFade)))+(final*(timePassed/timeToFade));
		}
		else 
		{
			if ((reversed||selfDestruct) && timePassed == timeToFade)
			{
				img.color = final;
			}
			else if (!reversed && timePassed == 0){
				img.color = initial;
			}
		}
		if (selfDestruct)
		{
			//Destroy(gameObject.GetComponent<FadeInItem>());
			Destroy(this);
		}
    }
	
}
