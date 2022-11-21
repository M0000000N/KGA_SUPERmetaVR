using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LeftTwoBoneIKConstraint : MonoBehaviour
{
    private TwoBoneIKConstraint leftTwoBoneIKConstraint;
    private SkinnedMeshRenderer[] LobbyPlayerModelings;

    private void Awake()
    {
        leftTwoBoneIKConstraint = GetComponent<TwoBoneIKConstraint>();
        LobbyPlayerModelings = transform.root.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var children in LobbyPlayerModelings)
        {
            children.gameObject.SetActive(false);
        }
        LobbyPlayerModelings[0].gameObject.SetActive(true);
        leftTwoBoneIKConstraint.data.root = LobbyPlayerModelings[0].gameObject.transform.parent.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm");
        leftTwoBoneIKConstraint.data.mid = LobbyPlayerModelings[0].gameObject.transform.parent.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm");
        leftTwoBoneIKConstraint.data.tip = LobbyPlayerModelings[0].gameObject.transform.parent.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand");

    }
}
