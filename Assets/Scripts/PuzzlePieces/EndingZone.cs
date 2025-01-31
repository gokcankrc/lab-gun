using Sirenix.OdinInspector;
using UnityEngine;

public class EndingZone : MonoBehaviour
{
    [SerializeField] int cutsceneTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        End();
    }

    [Button]
    private void End()
    {
        DialogueManager.StartDialogueScene(cutsceneTriggered);
    }
}