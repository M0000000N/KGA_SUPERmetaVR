using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : SingletonBehaviour<RewardManager>
{
    [SerializeField] private RewardPopup rewardPopup;
    [SerializeField] private FD_RewardMessage FD_RewardMessage;
    private Item newItem;
    public Item NewItem { get { return newItem; } set { newItem = value; } }

    private void Awake()
    {
        NewItem = new Item();
    }
    public int ChooseItem()
    {
        int rewardItemProbability = Random.Range(0, 100000);
         for (int i = 0; i < StaticData.RewardSheetData.Length; i++)
        {
            int probability = StaticData.GetRewardData(i).Probability;
            if (rewardItemProbability <= probability)
            {
                return StaticData.GetRewardData(i).Itemid;
            }
            rewardItemProbability -= probability;
        }
        return 0;
    }

    public bool GetItem()
    {
        int rewardItemID = ChooseItem();
        if(rewardItemID > 0)
        {
            rewardPopup.gameObject.SetActive(true);
            NewItem.ItemID = rewardItemID;
            ItemManager.Instance.Inventory.AcquireItem(NewItem, 1);
            rewardPopup.SetPopupUI(rewardItemID);
            return true;
        }
        return false;
    }

    public void OpenRewardMessage()
    {
        FD_RewardMessage.gameObject.SetActive(true);
        FD_RewardMessage.OpenUI();
    }
}
