using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine("Test");
        }
    }

    IEnumerator TestCor()
    {
        int min = 0;
        yield return new WaitForSeconds(600);
        min += 10;
        UnityEngine.Debug.Log($"{min}분 지났습니다");
        if(CloverColonyDataBase.Instance.CheckCooltime(1))
        {
        }
    }
}
