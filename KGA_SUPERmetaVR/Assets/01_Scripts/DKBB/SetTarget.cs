using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTarget : MonoBehaviour
{
    public void SetTransform(Transform _transform)
    {
        gameObject.transform.position = _transform.position;
        gameObject.transform.rotation = _transform.rotation;
    }
}
