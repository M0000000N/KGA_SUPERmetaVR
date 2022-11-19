using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class CloverInfo : MonoBehaviourPun
{
    [SerializeField] private bool isFourLeaf;
    public bool IsFourLeaf { get { return isFourLeaf; } set { isFourLeaf = value; } }

    [SerializeField] private int area;
    public int Area { get { return area; } set { area = value; } }

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material fadeMaterial;
    private float fadeoutTime = 2f;
    private bool isStartFadedout = false;

    public bool IsStartFadedout
    {
        get
        {
            return isStartFadedout;
        }
        set
        {
            isStartFadedout = value;
            if (isStartFadedout && gameObject.activeSelf) photonView.RPC("StartFadeOut", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void StartFadeOut()
    {
        StartCoroutine(FadeoutRespawnCoroutine());
    }

    public IEnumerator FadeoutRespawnCoroutine()
    {
        gameObject.GetComponent<XRGrabInteractable>().enabled = false;

        MeshRenderer myRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        myRenderer.material = fadeMaterial;
        Color myColor = myRenderer.material.color;

        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= 0.1f / fadeoutTime;
            myRenderer.material.color = myColor;
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
        myRenderer.material = defaultMaterial;

        yield return new WaitForSeconds(0.1f);
        CloverSpawnManager.Instance.ReSpawnClover(gameObject.transform, Area);
        gameObject.GetComponent<XRGrabInteractable>().enabled = true;
        isStartFadedout = false;
    }
}
