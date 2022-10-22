using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LayserPointer2 : MonoBehaviour
{
    private LineRenderer layser;
    private RaycastHit Collided_object;
    private GameObject currentObject;

    [SerializeField]
    private float rayDistance = 10f;

    [SerializeField]
    private GameObject peekaboo;

    private void Start()
    {

        peekaboo.SetActive(false);
    }

    private void Update()
    {
        layser.SetPosition(0, transform.position);

        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green, 0.5f);

        if (Physics.Raycast(transform.position, transform.forward, out Collided_object, rayDistance))
        {
            layser.SetPosition(1, Collided_object.point);

            if (Collided_object.collider.gameObject.CompareTag("Player") || Collided_object.collider.gameObject.CompareTag("Enemy"))
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)))
                {
                    StartCoroutine("FadeOutPeekaboo");
                }
            }
        }
    }

    public IEnumerator FadeOutPeekaboo()
    {
        Debug.Log("코루틴함수2");
        peekaboo.SetActive(true);

        for (float f = 1f; f > 0; f -= 0.02f)
        {
            Color c = peekaboo.GetComponent<Image>().color;
            c.a = f;
            peekaboo.GetComponent<Image>().color = c;
            yield return null;
        }
        yield return new WaitForSeconds(1);
    }

}
