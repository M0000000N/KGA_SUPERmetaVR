using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RightTwoBoneIKConstraint : MonoBehaviour
{
    private TwoBoneIKConstraint rightTwoBoneIKConstraint;
    private SkinnedMeshRenderer[] LobbyPlayerModelings;

    private void Awake()
    {
        rightTwoBoneIKConstraint = GetComponent<TwoBoneIKConstraint>();
        LobbyPlayerModelings = transform.root.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var children in LobbyPlayerModelings)
        {
            children.gameObject.SetActive(false);
        }
        LobbyPlayerModelings[0].gameObject.SetActive(true);
        rightTwoBoneIKConstraint.data.root = LobbyPlayerModelings[0].gameObject.transform.parent.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm");
        rightTwoBoneIKConstraint.data.mid = LobbyPlayerModelings[0].gameObject.transform.parent.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm");
        rightTwoBoneIKConstraint.data.tip = LobbyPlayerModelings[0].gameObject.transform.parent.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");

    }
}
