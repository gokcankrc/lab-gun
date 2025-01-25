using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AnimatedCharacter : MonoBehaviour
{
	SpriteRenderer img;
	
	Sprite [] current;
	int currentStep;
	float timeToNextStep, timePerStep;
	[SerializeField]Animation.AnimationSet [] sets;
	bool looping;
	Animation.AnimationId currentId;
	Animation.Direction currentDirection = Animation.Direction.down;
    void Start()
    {
		
        img = gameObject.GetComponent<SpriteRenderer>();
		if (img is null){
			print ("Missing a SpriteRenderer for "+this);
		}
		StartAnimation(Animation.AnimationId.idle,Animation.Direction.none,true);
    }
	
    void Update()
    {
        timeToNextStep -=Time.deltaTime;
		if (timeToNextStep <= 0){
			GetNextStep();
		}
		//StartAnimation();
    }
	public void SetAnimationSpeed(float newVal)
	{
		timeToNextStep /= timePerStep;
		timePerStep = newVal;
		timeToNextStep *= timePerStep;
	}
	public void Turn(Animation.Direction newDir)
	{
		
		print ("turning "+newDir);
		currentDirection = newDir;
		if (newDir == Animation.Direction.none)
		{
			return;
		}
		current = GetAnimationFor(currentId).ReturnSet(currentDirection);
		if (currentStep >=current.Count())
		{
			currentStep = 0;
		}
		//GetNextStep();
		//print ("I did not explode");
	}
	Animation.AnimationSet GetAnimationFor(Animation.AnimationId id)
	{
		
		//in case we want to randomize and set multiple animations
		/*
		Animation.AnimationSet result; 
		List<int> possibleResults = new List<int>();
		int counter = 0;
		*/
		foreach (Animation.AnimationSet anim in sets)
		{
			
			if (anim.id == id)
			{
				//possibleResults.Add(counter);
				return anim;
			}
			//counter++;
		}
		/*
		if (counter > 0)
		{
			return sets[possibleResults[Unity.Random.Range(0,possibleResults.Count())]];
		}
		*/
		//default value
		return sets[0];
	}
	public void StartAnimation ( Animation.AnimationId newId = Animation.AnimationId.idle, Animation.Direction dir = Animation.Direction.none, bool canInterrupt = false)
	{
		if (!canInterrupt && !looping && currentStep<current.Count()){
			
			return;
		}
		if (dir != Animation.Direction.none && dir != currentDirection)
		{
			Turn(dir);
		}
		if (currentId == newId  && looping == true)
		{
			//print ("discarded change: same animation ("+currentId+" vs "+newId+")");
			return;
		}
		currentDirection = dir;
		currentId = newId;
		SetAnimation(GetAnimationFor(newId));
		currentStep = -1;
		
		GetNextStep();
	}
	void SetAnimation(Animation.AnimationSet anim)
	{
		//print ("Starting animation "+currentId);
		current = anim.ReturnSet(currentDirection);
		timePerStep = anim.timePerFrame;
		looping= anim.looped;
	}
	void GetNextStep ()
	{
		currentStep++;
		if (currentStep>=current.Count()){
			if (looping){
				currentStep = -1;
			}
			else {
				StartAnimation();
			}
		}
		else 
		{
			img.sprite = current[currentStep];
			timeToNextStep = timePerStep;	
		}
	}
	public static Animation.Direction VectorToDirection(Vector2 vector)
	{
		if (vector.magnitude == 0)
		{
			return Animation.Direction.none;
		}
		if (Math.Abs(vector.x)>Math.Abs(vector.y))
		{
			if (vector.x>0)
			{
				return Animation.Direction.right;
			}
			else
			{
				return Animation.Direction.left;
			}
		}
		else 
		{
			if (vector.y>0)
			{
				return Animation.Direction.up;
			}
			else
			{
				return Animation.Direction.down;
			}
		}
		
	}
}
namespace Animation
{
	
	public enum AnimationId
	{
		idle,
		attack,
		walk,
		run,
		die,
		cower,
		rewindingTime
	}
	public enum Direction
	{
		none,
		left,
		right,
		up,
		down
	}
	[Serializable]
	public struct AnimationSet 
	{
		public float timePerFrame;
		public AnimationId id;
		public bool looped;
		public Sprite[] spriteListDown;// doubles as default
		public bool directional;
		public Sprite[] spriteListUp;
		public Sprite[] spriteListLeft;
		public Sprite[] spriteListRight;
		
		

		public Sprite[] ReturnSet(Direction dir)
		{
			Debug.Log("incoming "+dir);
			if (directional)
			{		
				switch ((int)dir)
				{
					case (int)Direction.left:
					{
						Debug.Log("returning left");
						return spriteListLeft;
					}
					case (int)Direction.right:
					{
						Debug.Log("returning left");
						return spriteListRight;
					}
					case (int)Direction.up:
					{
						Debug.Log("returning left");
						return spriteListUp;
					}
					case (int)Direction.down:
					{
						Debug.Log("returning left");
						return spriteListDown;
					}
					default:
					{
						return spriteListDown;
					}
				}
			}
			else 
			{
				return spriteListDown;
			}
		}
		
	}
}