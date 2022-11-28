using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFF_ButtonList : MonoBehaviour
{
    [SerializeField] FFF_Button[] button;
    [SerializeField] Sprite[] imagePool;
    public Sprite[] ImagePool { get { return imagePool; } set { imagePool = value; } }
    private List<Vector3> spawnPositionList = new List<Vector3>();
    private int round;

    private void Start()
    {
        for (int i = 1; i < button.Length; i++)
        {
            button[i].gameObject.SetActive(false);
        }
        round = 0;
    }

    private void Update()
    {
        if (FFF_GameManager.Instance.flow == 2)
        {
            int doneCount = FFF_GameManager.Instance.DoneCount;
            if (doneCount > 0 && doneCount % 2 == 0)
            {
                SetNextButtonList(round, false);
                round++;
                SetNextButtonList(round, true);
            }
        }
    }

    public void SetNextButtonList(int _round, bool _isActive)
    {
        button[_round].gameObject.SetActive(_isActive);
        button[_round + 1].gameObject.SetActive(_isActive);
    }

    //public void Initialize()
    //{
    //    for (int i = 0; i < button.Length; i++)
    //    {
    //        SpawnObject(button[i].transform);
    //    }
    //}
    //public void SpawnObject(Transform _target)
    //{
    //    Vector3 spawnPosition;
    //    do
    //    {
    //        float randomX = Random.Range(-1f, 0.5f);
    //        float randomY = Random.Range(-0.2f, 0.7f);

    //        spawnPosition = new Vector3(randomX, randomY, 0);
    //    }
    //    while (CheckSpawnArea(spawnPosition) == false);

    //    spawnPositionList.Add(spawnPosition);

    //    _target.position = spawnPosition;
    //}

    //public bool CheckSpawnArea(Vector3 _position)
    //{
    //    for (int i = 0; i < spawnPositionList.Count; i++)
    //    {

    //        if (spawnPositionList[i] == _position)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
}
