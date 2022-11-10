using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionOutline : MonoBehaviour
{
    [SerializeField] XRRayInteractor RightRayInteractor;

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
            outline.enabled = false;
        }
        else
        {
            // �Ÿ� ���ϱ� 
            if (Vector3.Distance(targetObject.transform.position, transform.position) >= maxDistance)
            {
                Outline outline = targetObject.GetComponent<Outline>();
                outline.enabled = false;
            }
            else
            {
                Debug.Log("�Ÿ� ���");
            }
        }
    }
    public GameObject InteractCharacter()
    {
        return targetObject;
    }
}
