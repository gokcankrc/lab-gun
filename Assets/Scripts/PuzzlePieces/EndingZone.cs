using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class EndingZone : MonoBehaviour
{
	[SerializeField]int cutsceneTriggered;
	private void OnTriggerEnter2D(Collider2D other)
    {
          DialogueManager.StartDialogueScene(cutsceneTriggered);
    }

}
