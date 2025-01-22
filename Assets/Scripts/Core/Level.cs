using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int index;
    public List<ILevelObject> levelObjects = new();

    private void OnEnable()
    {
        GameManager.I.AddLevel(index, this);
    }

    private void Start()
    {
        ILevelObject[] foundObjects = GetComponentsInChildren<ILevelObject>();
        foreach (ILevelObject levelIndexed in foundObjects)
        {
            levelIndexed.LevelIndex = index;
        }

        levelObjects.AddRange(foundObjects);
    }

    private void OnDisable()
    {
        GameManager.I.RemoveLevel(index);
    }

    public void TriggerAlarm(int triggeredLevelIndex)
    {
        // don't alarm if this is a later level than the alarmed level.
        if (this.index > triggeredLevelIndex) return;
        foreach (var levelObject in levelObjects)
        {
            levelObject.Alarm();
        }
    }
}