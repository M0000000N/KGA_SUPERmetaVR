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

    // static�̱� ������ Ÿ������ ������ �����ϴ�.
    //�ν��Ͻ��� Ŭ�������� ���� ������ ����,,?������...?Ŭ�������� ����ϴ� ����...??������?
    // Ŭ������ �ν��Ͻ��� ���踦 �˾ƾ��ϴµ�, Ŭ������ ���赵 �ν��Ͻ��� �� ���赵�� ���� ��ü.
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
        //���� ��ȯ�Ǿ �ı��� �Ǹ� �ȵȴ�.
        DontDestroyOnLoad(gameObject);
    }
}
