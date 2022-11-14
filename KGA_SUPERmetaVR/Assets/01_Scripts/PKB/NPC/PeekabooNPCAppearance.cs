using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCAppearance : MonoBehaviour
{
    [SerializeField]
    private Dictionary<int,GameObject> NPCappearance;
    [SerializeField]
    private List<GameObject> NPCappearanceList;
    private void Start()
    {
        NPCappearanceList = new List<GameObject>();
        NPCappearance = new Dictionary<int, GameObject>();
    }

    void Update()
    {
        
    }

}
