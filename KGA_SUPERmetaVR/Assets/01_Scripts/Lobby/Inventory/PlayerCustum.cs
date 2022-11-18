using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCustum : MonoBehaviour
{
    private Animator playerAnimator;
    private BoneRenderer playerBonerednerer;
    private LobbyPlayerFSM lobbyPlayerFSM;
    private GameObject playerCharacter;
    private SkinnedMeshRenderer playerSkinnedMeshRenderer;
    private MultiParentConstraint multiParentConstraint;
    private TwoBoneIKConstraint twoBoneIKConstraint;
    private void Start()
    {
        
    }
    private void ChangeCustum(string _custum)
    {
        GameObject prefab = Resources.Load<GameObject>("PlayerCustum/" + _custum);

        playerAnimator.avatar = prefab.GetComponent<Animator>().avatar;
        GameObject rig = prefab.GetComponentInChildren<Lig>().gameObject;
        Transform[] allChildren = rig.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChildren.Length; ++i)
        {
            playerBonerednerer.transforms[i] = allChildren[i];
        }
        playerSkinnedMeshRenderer = prefab.GetComponentInChildren<SkinnedMeshRenderer>();
        playerSkinnedMeshRenderer.rootBone = rig.transform;

        lobbyPlayerFSM.myRenderer = playerSkinnedMeshRenderer;
        lobbyPlayerFSM.MyOpaqueMaterial = playerSkinnedMeshRenderer.material;
        lobbyPlayerFSM.MyTransparentMaterial = playerSkinnedMeshRenderer.material;
        //multiParentConstraint.data.constrainedObject = 









    }
}
