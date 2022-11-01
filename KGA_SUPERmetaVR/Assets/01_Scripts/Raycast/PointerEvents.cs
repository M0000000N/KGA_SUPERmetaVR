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
    }

    public void OnPointerExit(PointerEventData enventData)
    {
        meshRenderer.material.color = normalColor;
    }
    public void OnPointerDown(PointerEventData enventData)
    {
        meshRenderer.material.color = downColor;
    }

    public void OnPointerUp(PointerEventData enventData)
    {
        meshRenderer.material.color = enterColor;
    }

    public void OnPointerClick(PointerEventData enventData)
    {
        OnClick.Invoke();      
    }
 
 }
