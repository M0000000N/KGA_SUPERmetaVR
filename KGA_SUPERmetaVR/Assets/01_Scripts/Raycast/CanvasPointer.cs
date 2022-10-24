using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CanvasPointer : MonoBehaviour 
{
    [SerializeField]
    private float lineLength = 3.0f;

    public EventSystem eventSystem = null;
    public StandaloneInputModule inputModule = null;

    private LineRenderer lineRenderer = null;  

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    private Vector3 GetEnd()
    {
        float distance = GetCanvasDistance();
        Vector3 endPosition = CalculateEnd(lineLength);

        if(distance != 0.0f)
        {
            endPosition = CalculateEnd(distance);
        }

        return endPosition;
    }
    private float GetCanvasDistance()
    {
        //Get Data
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = inputModule.inputOverride.mousePosition;

        //Raycast Using data
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        //Get Closet
        RaycastResult closetResult = FindFisrtRaycast(results);
        float distance = closetResult.distance;

        //Clamp 
        distance = Mathf.Clamp(distance, 0.0f, lineLength);
        return distance; 
    }
    private RaycastResult FindFisrtRaycast(List<RaycastResult> results)
    {
       foreach(RaycastResult result in results)
        {
            if(!result.gameObject)
            {
                continue;
            }
            return result; 
        }
        return new RaycastResult(); 
    }    
    private Vector3 CalculateEnd(float _lenght)
    {
        return transform.position + (transform.forward * _lenght); 
    }
}
