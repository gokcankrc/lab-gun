using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu]
[Serializable]
public class Dialogue:ScriptableObject
{
	[SerializeField]DialogueLine[] lines;
	int lineMarker = 0;
	public DialogueLine ReturnNextLine ()
	{
		
		Debug.Log(""+lineMarker+" vs "+lines.Length);
		if (lineMarker>=lines.Length)
		{
			return null;
		}
		lineMarker++;
		return lines[lineMarker-1];
		
	}
	public DialogueLine ReturnPreviousLine ()
	{
		if (lineMarker==0)
		{
			return lines[0];
		}
		lineMarker--;
		return lines[lineMarker-1];
		
	}
	public DialogueLine ReturnFirstLine ()
	{
		//if LineMarker is 0 when NextLine is processed, the next line will be the first.
		lineMarker = 1;
		return lines[0];
		
	}
}