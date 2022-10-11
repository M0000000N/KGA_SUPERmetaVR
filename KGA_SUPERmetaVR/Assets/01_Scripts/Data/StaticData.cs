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

    [SerializeField] Notification notification;
    public static NotificationData[] NotificationData { get { return Instance.notification.dataArray; } }

    public static NotificationData GetNotificationData(int _id)
    {
        for (int i = 0; i < NotificationData.Length; i++)
        {
            if (NotificationData[i].Id == _id)
            {
                return NotificationData[i];
            }
        }
        return null;
    }
}
