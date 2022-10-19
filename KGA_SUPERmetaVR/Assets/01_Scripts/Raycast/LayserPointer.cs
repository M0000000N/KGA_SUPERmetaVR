using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; 

// ���ݴ���� ��, ������������ ������ ��ī�� ���(1�� ���� ����̶�� �����ϰ�)
public class LayserPointer : MonoBehaviour
{
    [SerializeField]
    private float lineLength = 1.5f;

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
        lineRenderer.SetPosition(1, CalculatedEnd());
    }

    public Vector3 CalculatedEnd()
    {
        RaycastHit hit = CreateFowardRaycast();
        Vector3 endPosition = DefaultEnd(lineLength); 

        if(hit.collider)
        {
            endPosition = hit.point;
        }
        return endPosition; 
    }

    public RaycastHit CreateFowardRaycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, lineLength);
        return hit;
    }

    private Vector3 DefaultEnd(float _lenght)
    {
        return transform.position + (transform.forward * _lenght);
    }

 }
