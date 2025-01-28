using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ky;
public class MusicManager : Singleton<MusicManager>
{
    [SerializeField]AudioSource basic, combat;
    [SerializeField] float fadeTimeForCombat;
    static float musicVolume = 0.4f;
    static bool muteMusic;
    bool combatStarted;
    float timePassed, combatTimePassed;
    static float trackTime;
    void Start()
    {
        basic.time = trackTime;
        combat.time = trackTime;
        basic.volume = musicVolume;
        combat.volume = 0;
        // test values
        //muteMusic = true;
        //combatStarted = true;
        // test values end
        if (muteMusic)
        {
            basic.mute = true;
            combat.mute = true;
        }
    }
    void Update ()
    {
        if (combatStarted&&combatTimePassed<fadeTimeForCombat)
        {
            combatTimePassed+= Time.deltaTime;
            if (combatTimePassed>= fadeTimeForCombat)
            {
                combatTimePassed = fadeTimeForCombat;
            }
            combat.volume = basic.volume*(combatTimePassed / fadeTimeForCombat);
        }
        else if (!combatStarted&&combatTimePassed>0)
        {
            combatTimePassed-= Time.deltaTime;
            if (combatTimePassed<= 0)
            {
                combatTimePassed = 0;
            }
            combat.volume = basic.volume*(combatTimePassed / fadeTimeForCombat);
        }
    }
    public void GameStateChanged (GameState newState)
    {
        switch ((int)newState)
        {
            case (int)GameState.InPuzzle :
            {
                combatStarted = false;
                break;
            }
            case (int)GameState.InBreakout :
            {
                combatStarted = true;
                break;
            }
        }

    }
    public void Resetting ()
    {
        trackTime = basic.time;
    }
}
