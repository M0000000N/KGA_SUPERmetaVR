using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : SingletonBehaviour<RewardManager>
{
    [SerializeField] private RewardPopup rewardPopup;

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
            Item newItem = new Item();
            newItem.ItemID = rewardItemID;
            ItemManager.Instance.Inventory.AcquireItem(newItem, 1);
            rewardPopup.SetPopupUI(rewardItemID);
            return true;
        }
        return false;
    }
}
