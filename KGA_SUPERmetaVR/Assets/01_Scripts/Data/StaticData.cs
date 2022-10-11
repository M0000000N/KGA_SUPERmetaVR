using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : SingletonBehaviour<StaticData>
{
    // ---------------------------------------------------------------------------------------
    [SerializeField] TestDB testData;
    public static TestDBData[] TestData { get { return Instance.testData.dataArray; } }

    public static TestDBData GetTestData(int _id)
    {
        for (int i = 0; i < TestData.Length; i++)
        {
            if (TestData[i].Id == _id)
            {
                return TestData[i];
            }
        }
        return null;
    }
    // ---------------------------------------------------------------------------------------

    [SerializeField] NoticePopup noticePopup;
    public static NoticePopupData[] NoticePopupData { get { return Instance.noticePopup.dataArray; } }

    public static NoticePopupData GetNotificationData(int _id)
    {
        for (int i = 0; i < NoticePopupData.Length; i++)
        {
            if (NoticePopupData[i].Id == _id)
            {
                return NoticePopupData[i];
            }
        }
        return null;
    }
}
