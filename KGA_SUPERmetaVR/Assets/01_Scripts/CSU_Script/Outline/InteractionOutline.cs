using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionOutline : MonoBehaviour
{
    [SerializeField] XRRayInteractor leftRayInteractor;
    [SerializeField] XRRayInteractor RightRayInteractor;

    private RaycastHit leftRayHit;
    private RaycastHit RightRayHit;

    [SerializeField]
    private int maxDistance = 3;

    GameObject targetObject;

    private void Update()
    {
        RayCastHit();
    }

    public void RayCastHit()
    {
        if (RightRayInteractor.TryGetCurrent3DRaycastHit(out RightRayHit, out maxDistance))
        {
            targetObject = RightRayHit.transform.gameObject;
            Outline outline = targetObject.GetComponent<Outline>();

            if (RightRayHit.transform.tag == "InteractionOutlineObject")
            {
                if (outline != null)
                {
                    outline.enabled = true;
                }
            }
        }
        else if (leftRayInteractor.TryGetCurrent3DRaycastHit(out leftRayHit))
        {
            targetObject = leftRayHit.transform.gameObject;
            {
                Outline outline = targetObject.GetComponent<Outline>();
                if (leftRayHit.transform.tag == "InteractionOutlineObject")
                {

                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                }
            }
        }
        else
        {
            // 거리 구하기 
            if (Vector3.Distance(targetObject.transform.position, transform.position) >= maxDistance)
            {
                Outline outline = targetObject.GetComponent<Outline>();
                outline.enabled = false;
            }
            else
            {
                Debug.Log("거리 벗어남");
            }
        }
    }
    public GameObject InteractCharacter()
    {
        return targetObject;
    }
}
