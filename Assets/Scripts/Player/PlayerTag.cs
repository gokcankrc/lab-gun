using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class PlayerTag
{
	public string tagName;
	public int value;
	public ResetCondition resetCondition;
	public bool exactValueForCondition;

	public PlayerTag (string name, int initialValue)
	{
		tagName = name;
		value = initialValue;
	}
	public PlayerTag (PlayerTag clone)
	{
		tagName = clone.tagName;
		value = clone.value;
		resetCondition = clone.resetCondition;
	}
	public static void AddToList (List<PlayerTag> list, PlayerTag toBeAdded)
	{
		bool addAsNew = true;
		foreach (PlayerTag pt in list)
		{
			if (pt.tagName == toBeAdded.tagName)
			{
				addAsNew = false;
				pt.value += toBeAdded.value;
				pt.resetCondition = toBeAdded.resetCondition;
			}
		}
		if (addAsNew)
		{
			list.Add(new PlayerTag(toBeAdded));
		}
	}
	public static void SubtractFromList (List<PlayerTag> list, PlayerTag toBeSubtracted)
	{
		foreach (PlayerTag pt in list)
		{
			if (pt.tagName == toBeSubtracted.tagName)
			{
				pt.value -= toBeSubtracted.value;
			}
		}
	}
	public static void SetValue(List<PlayerTag> list, PlayerTag toBeSet)
	{
		bool addAsNew = true;
		foreach (PlayerTag pt in list)
		{
			if (pt.tagName == toBeSet.tagName)
			{
				addAsNew = false;
				pt.value = toBeSet.value;
				pt.resetCondition = toBeSet.resetCondition;
			}
		}
		if (addAsNew)
		{
			list.Add(new PlayerTag(toBeSet));
		}
	}
	public static void ResetValue(List<PlayerTag> list, string toBeReset)
	{
		foreach (PlayerTag pt in list)
		{
			if (pt.tagName == toBeReset)
			{
				list.Remove(pt);
				return;
			}
		}
	}
	public static void IncreaseValue(List<PlayerTag> list, string toBeAdded)
	{
		bool addAsNew = true;
		foreach (PlayerTag pt in list)
		{
			if (pt.tagName == toBeAdded)
			{
				addAsNew = false;
				pt.value ++;
			}
		}
		if (addAsNew)
		{
			list.Add(new PlayerTag(toBeAdded,1));
		}
	}
	public static bool DecreaseValue(List<PlayerTag> list, string toBeDecreased)
	{
		foreach (PlayerTag pt in list)
		{
			if (pt.tagName == toBeDecreased)
			{
				
				pt.value --;
				if (pt.value == 0)
				{
					PlayerTag.ResetValue(list,toBeDecreased);
				}
				return false;
			}
		}
		return false;
	}
	public static bool HasTagValue (List<PlayerTag> list,string name, int value, bool exactValue = false)
	{
		foreach (PlayerTag pt in list)
		{
			if (pt.tagName == name)
			{
				if(exactValue)
				{
					return pt.value==value;
				}
				return pt.value>=value;
			}
		}
		return false;
	}
	public static bool HasTagValue (List<PlayerTag> list,PlayerTag comparison)
	{
		foreach (PlayerTag pt in list)
		{
			if (pt.tagName == comparison.tagName)
			{
				if (comparison.exactValueForCondition)
				{
					return pt.value==comparison.value;
				}
				return pt.value>=comparison.value;
			}
		}
		return false;
	}
	
	public bool ProcessResetCondition(Tag.ResetsOn eventId, float value)
	{
		if ((resetCondition.condition & eventId) == Tag.ResetsOn.OnTimePassed)
		{
			resetCondition.value-=value;
			if (resetCondition.value <= 0)
			{
				return true;
			}
		}
		else if((resetCondition.condition & eventId) == Tag.ResetsOn.OnPlayerCollide)
		{
			return true;
		}
		
		if ((resetCondition.condition & eventId) == Tag.ResetsOn.OnSpeedReached)
		{
			if (resetCondition.value <= value)
			{
				return true;
			}
		}
		if ((resetCondition.condition & eventId) == Tag.ResetsOn.OnSpeedNotReached)
		{
			if (resetCondition.value >= value)
			{
				return true;
			}
		}
		
		return false;
	}
	
}
[Serializable]
public struct ResetCondition
{
	public float value;
	public Tag.ResetsOn condition;
}
[Serializable]
public struct SpecialActions
{
	public AdditionalActions.Action action;
	public AdditionalActions.Trigger trigger;
	public float value;
}
namespace AdditionalActions
{
    public enum Action
    {
        None,
		Heal,
		Accellerate,
		Damage
    }
    public enum Trigger
    {
        None,
		PlayerEnters
    }
}