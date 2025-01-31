using UnityEngine;
using UnityEngine.EventSystems;

public class AudioOnHoverAndClick : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] AudioSource hoverSound, clickSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverSound.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickSound.Play();
    }
}