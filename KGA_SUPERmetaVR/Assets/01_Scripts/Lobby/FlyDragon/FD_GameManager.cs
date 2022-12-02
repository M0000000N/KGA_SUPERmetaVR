using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FD_GameManager : OnlyOneSceneSingleton<FD_GameManager>
{
    [SerializeField] private Transform[] area;
    [SerializeField] private FD_Dragon[] star;
    private PhotonView photonView;

    void Awake()
    {
        photonView = PhotonView.Get(this);
        Initialize();

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    [PunRPC]
    public void Initialize()
    {
        for (int i = 0; i < star.Length; i++)
        {
            SpawnObject(star[i].transform);
        }
    }

    public void SpawnObject(Transform _target)
    {
        _target.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        FD_Area spawnArea;
        Vector3 spawnPosition;

        do
        {
            int randomArea = Random.Range(0, area.Length);
            spawnArea = area[randomArea].GetComponent<FD_Area>();

            float randomX = Random.Range(-1, 2);
            float randomY = Random.Range(-1, 2);

            spawnPosition = new Vector3(randomX, -0.2f, randomY);
        }
        while (CheckSpawnArea(spawnArea, spawnPosition) == false);

        spawnArea.SpawnPosition.Add(spawnPosition);

        int randomRotation = Random.Range(0, 360);


        _target.position = spawnArea.transform.position - spawnPosition;
        _target.rotation = Quaternion.Euler(0, randomRotation, 0);
        _target.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _target.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        _target.gameObject.SetActive(true);
    }

    public bool CheckSpawnArea(FD_Area _spawnArea, Vector3 _position)
    {
        for (int i = 0; i < _spawnArea.SpawnPosition.Count; i++)
        {
            if (_spawnArea.SpawnPosition[i] == _position)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator RespawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(600f); // QA CODE : 7200
            photonView.RPC("Initialize", RpcTarget.AllViaServer);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Star"))
        {
            other.gameObject.GetComponentInParent<FD_Dragon>().IsStartFadedout = true;
        }
    }
}
