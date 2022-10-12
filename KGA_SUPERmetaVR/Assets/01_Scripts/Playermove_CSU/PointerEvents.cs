using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color enterColor = Color.white;
    [SerializeField]
    private Color downColor = Color.white;
    [SerializeField]
    private UnityEvent OnClick = new UnityEvent();

    private MeshRenderer meshRenderer = null;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
    }

    public void OnPointerEnter(PointerEventData enventData)
    {
        meshRenderer.material.color = enterColor;
        Debug.Log("1");
    }

    public void OnPointerExit(PointerEventData enventData)
    {
        meshRenderer.material.color = normalColor;
        Debug.Log("2");
    }
    public void OnPointerDown(PointerEventData enventData)
    {
        meshRenderer.material.color = downColor;
        Debug.Log("3");
    }

    public void OnPointerUp(PointerEventData enventData)
    {
        meshRenderer.material.color = enterColor;
        Debug.Log("4");
    }

    public void OnPointerClick(PointerEventData enventData)
    {
        OnClick.Invoke();
        Debug.Log("5"); 
    }
}
