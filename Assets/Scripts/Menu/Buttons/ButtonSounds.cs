using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;
public class ButtonSounds : MonoBehaviour, IPointerEnterHandler,  IPointerDownHandler
{
    [SerializeField] private EventReference PointerEnterSoundEvent;
    [SerializeField] private EventReference PointerClickSoundEvent;
    public void OnPointerEnter(PointerEventData eventData)
    {
        RuntimeManager.PlayOneShot(PointerEnterSoundEvent);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        RuntimeManager.PlayOneShot(PointerClickSoundEvent);
    }
}
