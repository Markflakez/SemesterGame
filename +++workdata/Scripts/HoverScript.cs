using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetObject; // The object to be activated/deactivated
    private Manager manager;

    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!manager.isPaused && !manager.inventoryMain.activeSelf)
        {
            targetObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetObject.SetActive(false);
    }
}