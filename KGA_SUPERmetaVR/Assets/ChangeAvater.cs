using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvater : MonoBehaviour
{
    private SkinnedMeshRenderer[] LobbyPlayerModelings;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        LobbyPlayerModelings = transform.root.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var children in LobbyPlayerModelings)
        {
            children.gameObject.SetActive(false);
        }
        LobbyPlayerModelings[0].gameObject.SetActive(true);
        animator.avatar = Resources.Load<Avatar>("Avatars/Frog");
    }
}
