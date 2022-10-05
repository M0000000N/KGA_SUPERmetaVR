using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    // static이기 때문에 타입으로 접근이 가능하다.
    //인스턴스는 클래스에서 접근 가능한 변수,,?데이터...?클래스에서 사용하는 변수...??데이터?
    // 클래스와 인스턴스의 관계를 알아야하는데, 클래스는 설계도 인스턴스는 그 설계도로 만들어낸 실체.
    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
            return;
        }
        _instance = GetComponent<T>();
        //씬이 전환되어도 파괴가 되면 안된다.
        DontDestroyOnLoad(gameObject);
    }
}
