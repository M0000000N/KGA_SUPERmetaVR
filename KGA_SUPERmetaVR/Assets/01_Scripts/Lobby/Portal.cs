using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform moveTransform;
    [SerializeField] private GameObject LobbyPlayerCamera;

    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnPressButton);
    }

    private void Start()
    {
        button.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            button.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            button.gameObject.SetActive(false);
        }
    }

    public void OnPressButton()
    {
        LobbyPlayerCamera.transform.position = moveTransform.position;
    }
}
