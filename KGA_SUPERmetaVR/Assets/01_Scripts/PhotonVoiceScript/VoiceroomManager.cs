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
    //        if (voiceroomNumber[i] == false) // ���� ���� �� 
    //        {
    //            // byte[] remove = new byte[] { 0 }; // �⺻�׷�(��ü����) ����           
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
    //    // ���ڴ� �ʱ�ȭ
    //    // photonVoiceNetwork.InitRecorder(VoiceroomManager.Instance.recorder);
    //}

    //public void RefuseButton()
    //{
    //    // 1:1 ������ ���� ���ְ�
    //    // ��ΰ� �����ִ� �׷����� ���ư���
    //    for (int i = 1; i < 256; ++i)
    //    {
    //        if (voiceroomNumber[i]) // ���� ���� ��
    //        {
    //            byte[] remove = new byte[] { (byte)i };
    //            byte[] add = new byte[] { 0 };
    //            photonVoiceNetwork.Client.OpChangeGroups(remove, add);
    //            VoiceroomManager.Instance.recorder.InterestGroup = 0;
    //            Debug.Log("��� �鸮��");
    //            voiceroomNumber[i] = false;
    //        }
    //    }
    //}
}
    