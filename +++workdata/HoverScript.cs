using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetObject; // The object to be activated/deactivated

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetObject.SetActive(false);
    }
}