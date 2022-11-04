using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Oculus.Platform;

public class VoiceroomManager : SingletonBehaviour<VoiceroomManager>
{
    // PhotonVoiceNetwork photonVoiceNetwork;

    public Recorder recorder;
 //   private bool[] voiceroomNumber = new bool[256];

    //public void AcceptButton()
    //{
    //    for (int i = 1; i < 256; ++i)
    //    {
    //        if (voiceroomNumber[i] == false) // 방이 없을 때 
    //        {
    //            // byte[] remove = new byte[] { 0 }; // 기본그룹(전체음성) 삭제           
    //            byte[] add = new byte[] { (byte)i };
    //            photonVoiceNetwork.Client.OpChangeGroups(null, add);
    //            VoiceroomManager.Instance.recorder.InterestGroup = (byte)i;
    //            Debug.Log(i);
    //        }
    //        else { continue; }
    //    }

    //}
    //public void FinishButton()
    //{
    //    // connectandjoinroom 

    //    photonVoiceNetwork.AutoLeaveAndDisconnect = true;
    //    // 레코더 초기화
    //    // photonVoiceNetwork.InitRecorder(VoiceroomManager.Instance.recorder);
    //}

    //public void RefuseButton()
    //{
    //    // 1:1 생성된 방을 없애고
    //    // 모두가 속해있는 그룹으로 돌아간다
    //    for (int i = 1; i < 256; ++i)
    //    {
    //        if (voiceroomNumber[i]) // 방이 있을 때
    //        {
    //            byte[] remove = new byte[] { (byte)i };
    //            byte[] add = new byte[] { 0 };
    //            photonVoiceNetwork.Client.OpChangeGroups(remove, add);
    //            VoiceroomManager.Instance.recorder.InterestGroup = 0;
    //            Debug.Log("모두 들리나");
    //            voiceroomNumber[i] = false;
    //        }
    //    }
    //}
}
    