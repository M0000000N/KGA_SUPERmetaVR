using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ItemEquip : MonoBehaviour
{
    [SerializeField]
    private Button itemEquipButton;
    [SerializeField]
    private Button itemEquipCancelButton;
    [SerializeField]
    private TextMeshProUGUI equipDiscriptionText;

    private PhotonView photonView;

    public void EquipItemButton(int _slotNumber)
    {
        Debug.Log($" ���Գѹ�~~~~~@~!@#!@#$#${_slotNumber}");
        gameObject.SetActive(true);
        string itemName = StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Name;
        equipDiscriptionText.text = "'" + itemName + "'��(��) �����Ͻðڽ��ϱ�?";
        itemEquipButton.onClick.AddListener(() => { EquipItem(_slotNumber); });
        itemEquipCancelButton.onClick.AddListener(() => { CloseEquipUseUI(); });
    }
    


    private void EquipItem(int _slotNumber)
    {
        int IDnumber = GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID;
        PlayerCustum playerCustum = GameManager.Instance.Player.GetComponentInChildren<PlayerCustum>();
        GameManager.Instance.PlayerData.Customize = IDnumber;
        // �ٲ� Ŀ���͸���¡ ������ �ø��� �ڵ�
        ItemManager.Instance.IsEquipItem = true;
        GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Equip = (int)ITEMSTATE.EQUIP;
        playerCustum.ChangeCustum(IDnumber);
        itemEquipButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
        ItemManager.Instance.CloseItemInfoUI();
    }

    public void CloseEquipUseUI()
    {
        itemEquipButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
