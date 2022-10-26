using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOOCHARACTERBEHAVIOURDATA
{
    FRONT_TO_LEFT_MIN_TIME = 1,
    FRONT_TO_LEFT_MAX_TIME,
    LEFT_TO_FRONT_MIN_TIME,
    LEFT_TO_FRONT_MAX_TIME,
    FRONT_TO_RIGHT_MIN_TIME,
    FRONT_TO_RIGHT_MAX_TIME,
    RIGHT_TO_FRONT_MIN_TIME,
    RIGHT_TO_FRONT_MAX_TIME,
    MIN_SEEINGTIME_FRONT,
    MAX_SEEINGTIME_FRONT,
    MIN_SEEINGTIME_LEFT,
    MAX_SEEINGTIME_LEFT,
    MIN_SEEINGTIME_RIGHT,
    MAX_SEEINGTIME_RIGHT,
    TURN_AROUND_TIME,
}

public class StaticData : SingletonBehaviour<StaticData>
{
    //// ---------------------------------------------------------------------------------------
    //[SerializeField] private TestDB testData;
    //public static TestDBData[] TestData { get { return Instance.testData.dataArray; } }

    //public static TestDBData GetTestData(int _id)
    //{
    //    for (int i = 0; i < TestData.Length; i++)
    //    {
    //        if (TestData[i].Id == _id)
    //        {
    //            return TestData[i];
    //        }
    //    }
    //    return null;
    //}
    //// ---------------------------------------------------------------------------------------

    // ============================================== PEEKABOO ==============================================
    
    [SerializeField] private NoticePopup noticePopup;
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

    [SerializeField] private PeekabooCustomizing peekabooCustiomizing;
    public static PeekabooCustomizingData[] PeekabooCustiomizingData { get { return Instance.peekabooCustiomizing.dataArray; } }

    public static PeekabooCustomizingData GetPeekabooCustomizingData(int _id)
    {
        for (int i = 0; i < PeekabooCustiomizingData.Length; i++)
        {
            if (PeekabooCustiomizingData[i].ID == _id)
            {
                return PeekabooCustiomizingData[i];
            }
        }
        return null;
    }

    [SerializeField] private PeekabooCharacterBehaviour peekabooCharacterBehaviour;
    public static PeekabooCharacterBehaviourData[] PeekabooCharacterBehaviourData { get { return Instance.peekabooCharacterBehaviour.dataArray; } }

    public static PeekabooCharacterBehaviourData GetPeekabooCharacterBehaviourData(int _id)
    {
        for (int i = 0; i < PeekabooCharacterBehaviourData.Length; i++)
        {
            if (PeekabooCharacterBehaviourData[i].ID == _id)
            {
                return PeekabooCharacterBehaviourData[i];
            }
        }
        return null;
    }

    // ============================================== PEEKABOO ==============================================

    // ================================================ LOBBY ================================================

    [SerializeField] private SpeechBubbleSheet SpeechBubbleSheet;
    public static SpeechBubbleSheetData[] SpeechBubbleSheetData { get { return Instance.SpeechBubbleSheet.dataArray; } }

    public static SpeechBubbleSheetData GetSpeechBubbleSheet(int _id)
    {
        for (int i = 0; i < SpeechBubbleSheetData.Length; i++)
        {
            if (SpeechBubbleSheetData[i].ID == _id)
            {
                return SpeechBubbleSheetData[i];
            }
        }
        return null;
    }

    // ================================================ LOBBY ================================================


}