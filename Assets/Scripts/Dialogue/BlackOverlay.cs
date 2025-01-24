using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOverlay : MonoBehaviour
{
    float currentFade,maxTime;
    bool fadingOut = false;
    bool fadingIn = false;
    Image selfImage;
    void Start()
    {
        selfImage = transform.GetComponent<Image>();
        if (selfImage == null)
        {
            print ("The overlay was unable to get its own Image class");
            print ("Quite sad tbh");
        }
    }
    void Update()
    {
        if (fadingOut)
        {
            currentFade += Time.deltaTime;
            float perc = currentFade/maxTime;
            if (perc>=1f)
            {
                perc = 1f;
                fadingOut =false;
                currentFade = 0f;
            }
            selfImage.color = new Color(0f,0f,0f,perc);
        }
        else if(fadingIn){
            currentFade += Time.deltaTime;
            float perc = currentFade/maxTime;
            if (perc>=1f)
            {
                perc = 1f;
                fadingIn =false;
                currentFade = 0f;
            }
            selfImage.color = new Color(0f,0f,0f,1f-perc);
            
        }
    }
    public void FadeOut(float fadeTime)
    {
        fadingOut = true;
        currentFade = 0f;
        maxTime = fadeTime;
    }
    public void FadeIn(float fadeTime)
    {
        fadingIn = true;
        currentFade = 0f;
        maxTime = fadeTime;
    }
}
