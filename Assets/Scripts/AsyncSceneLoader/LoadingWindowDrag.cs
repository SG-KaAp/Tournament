using UnityEngine;
using UnityEngine.EventSystems;
public class LoadingWindowDrag : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
