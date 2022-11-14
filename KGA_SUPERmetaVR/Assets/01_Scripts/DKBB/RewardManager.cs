using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : SingletonBehaviour<RewardManager>
{
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

    public bool GetItme()
    {
        int rewardItemID = ChooseItem();
        if(rewardItemID > 0)
        {
            // æ∆¿Ã≈€ »πµÊ
            return true;
        }
        return false;
    }
}
