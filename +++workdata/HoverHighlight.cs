using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Manager manager;
    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = manager.uiFontColorDisabled;
    }
}
