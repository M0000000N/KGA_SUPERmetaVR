using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ConnectingConstrainedObject : MonoBehaviour
{
    private MultiParentConstraint multiParentConstraint;
    private SkinnedMeshRenderer[] LobbyPlayerModelings;

    private void Awake()
    {
        multiParentConstraint = GetComponent<MultiParentConstraint>();
        LobbyPlayerModelings = transform.root.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var children in LobbyPlayerModelings)
        {
            children.gameObject.SetActive(false);
        }
        LobbyPlayerModelings[0].gameObject.SetActive(true);
        multiParentConstraint.data.constrainedObject = LobbyPlayerModelings[0].gameObject.transform.parent.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head");

    }
}
