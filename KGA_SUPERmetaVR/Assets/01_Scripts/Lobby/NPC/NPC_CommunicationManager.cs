using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class NPC_CommunicationManager : MonoBehaviourPun
{
    [SerializeField] private int npcID;
    [SerializeField] private int startSheetID;

    private int sheetID;
    public int SheetID { get { return sheetID; } set { sheetID = value; } }

    [SerializeField] private int number;
    public int Number { get { return number; } set { number = value; } }


    [SerializeField] private GameObject handshake;
    [SerializeField] private GameObject communication;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button[] button;

    [SerializeField] private Animator characterAnimator;


    PlayerData playerData;

    [SerializeField] private Animator comunicationAnimationController;
    public Animator ComunicationAnimationController { get { return comunicationAnimationController; } set { comunicationAnimationController = value; } }

    private bool isComunicationAnimationEnd;

    private void Start()
    {
        // id 설정해주기
        playerData = GameManager.Instance.PlayerData;

        Button button = handshake.GetComponentInChildren<Button>();
        button.onClick.AddListener(OnPressHand);

        handshake.SetActive(false);
        communication.SetActive(false);

        Initialize();
    }

    public void Initialize()
    {
        number = 1;
        SheetID = startSheetID;
        isComunicationAnimationEnd = true;
    }

    // Collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            // 대화 중
            Initialize();
            handshake.SetActive(true);
            communication.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            // 대화 끝
            Initialize();
            handshake.SetActive(false);
            communication.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            OnPressHand();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            button[0].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            button[1].onClick.Invoke();
        }
    }

    // 시작
    public void OnPressHand()
    {
        Initialize();
        SetDialogue();
        handshake.SetActive(false);
        communication.SetActive(true);
    }

    // 대화
    public void SetDialogue()
    {
        comunicationAnimationController.SetBool(Animator.StringToHash("NextDialogue"), false);
        isComunicationAnimationEnd = true;
        NPCDialogueData npcDialogueData = StaticData.GetNPCDialogueData(SheetID, number);

        if (npcDialogueData == null) return;

        if(npcDialogueData.Animation != string.Empty)
        {
            characterAnimator.SetTrigger(Animator.StringToHash(npcDialogueData.Animation));
        }

        if (npcDialogueData.Soudeffect != string.Empty)
        {
            SoundManager.Instance.PlaySE(npcDialogueData.Soudeffect);
        }

        dialogueText.text = npcDialogueData.Talk;

        button[0].onClick.RemoveAllListeners();
        button[1].onClick.RemoveAllListeners();

        // TODO : 시스템화하면 좋겠지만 시간상 하드코딩을 진행
        if (npcID == 20300 && sheetID == 24000)
        {
            startSheetID = 24001;
        }
        else if (npcID == 20600 && sheetID == 27003)
        {
            startSheetID = 27005;
        }

        // 버튼1
        if (npcDialogueData.SELECT1.Equals(string.Empty))
        {
            NPCDialogueData nextNPCDialogueData = StaticData.GetNPCDialogueData(SheetID, number + 1);
            string text = string.Empty;
            if (nextNPCDialogueData != null)
            {
                text = "다음";
                button[0].onClick.AddListener(() => { NextNumber(npcDialogueData); });
            }
            else
            {
                text = "확인";



                button[0].onClick.AddListener(() => { EndCommunication(npcDialogueData); });
            }
            button[0].GetComponentInChildren<TextMeshProUGUI>().text = text;


            button[0].gameObject.SetActive(true);
        }
        else
        {
            button[0].GetComponentInChildren<TextMeshProUGUI>().text = npcDialogueData.SELECT1;
            button[0].onClick.AddListener(() => { CheckCondition(1, npcDialogueData); });
            button[0].gameObject.SetActive(true);
        }

        // 버튼2
        if (npcDialogueData.SELECT2.Equals(string.Empty))
        {
            button[1].gameObject.SetActive(false);
        }
        else
        {
            button[1].GetComponentInChildren<TextMeshProUGUI>().text = npcDialogueData.SELECT2;
            button[1].onClick.AddListener(() => { CheckCondition(2, npcDialogueData); });
            button[1].gameObject.SetActive(true);
        }
    }

    public void EndCommunication(NPCDialogueData _npcDialogueData)
    {
        if (isComunicationAnimationEnd == false) return;
        isComunicationAnimationEnd = false;

        GetItem(_npcDialogueData);
        
        if(npcID == 20600 && sheetID == 27004)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (npcID == 20001)
        {
            if(sheetID == 21000 && number == 4)
            {
                LoginManager.Instance.SetUICanvas(LoginManager.Instance.CreateNickName);
            }
            else if(sheetID == 21001 && number == 5)
            {
                LobbyManager.Instance.JoinOrCreateRoom(null, true);
            }
        }
        else if (npcID == 20401 && sheetID == 25002 && number == 4)
        {
            FFF_GameManager.Instance.StartDance();
            communication.SetActive(false);
        }
        else
        {
            communication.SetActive(false);
        }
    }

    public void NextNumber(NPCDialogueData _npcDialogueData)
    {
        if (isComunicationAnimationEnd == false) return;
        isComunicationAnimationEnd = false;

        number++;
        GetItem(_npcDialogueData);
        comunicationAnimationController.SetBool((int)Animator.StringToHash("NextDialogue"), true);
    }

    public void CheckCondition(int _type, NPCDialogueData _npcDialogueData) // _npcDialogueData.Select2condition2, _npcDialogueData.Condition2quantity)
    {
        int checkID = 0;
        int checkCount = 0;
        switch (_type)
        {
            case 1:
                checkID = _npcDialogueData.Select1condition1;
                checkCount = _npcDialogueData.Condition1quantity;
                break;
            default:
                checkID = _npcDialogueData.Select2condition2;
                checkCount = _npcDialogueData.Condition2quantity;
                break;
        }

        if (isComunicationAnimationEnd == false) return;
        isComunicationAnimationEnd = false;

        List<int> itemID = new List<int>();
        List<int> itemCount = new List<int>();
        int totalCount = 0;

        if (checkID > 0)
        {
            for (int i = 0; i < playerData.ItemSlotData.ItemData.Length; i++)
            {
                if (playerData.ItemSlotData.ItemData[i].ID == checkID)
                {
                    for (int j = 0; j < itemCount.Count; j++)
                    {
                        totalCount += itemCount[j];
                    }

                    if (playerData.ItemSlotData.ItemData[i].Count + totalCount >= checkCount)
                    {
                        for (int k = 0; k < itemID.Count; k++)
                        {
                            totalCount -= itemCount[k];
                        }

                        switch (_type)
                        {
                            case 1:
                                SheetID = StaticData.GetNPCDialogueData(SheetID, number).Condition1nexttalkid;
                                break;

                            default:
                                SheetID = StaticData.GetNPCDialogueData(SheetID, number).Condition2nexttalkid;
                                break;
                        }

                        number = 1;
                        comunicationAnimationController.SetBool((int)Animator.StringToHash("NextDialogue"), true);

                        GetItem(_npcDialogueData);
                        comunicationAnimationController.SetBool((int)Animator.StringToHash("NextDialogue"), true);
                        return;
                    }
                    else
                    {
                        itemID.Add(i);
                        itemID.Add(playerData.ItemSlotData.ItemData[i].Count);
                    }
                }
            }

            //switch (_type)
            //{
            //    case 1:
            //        SheetID = StaticData.GetNPCDialogueData(SheetID, number).Nexttalkid1;
            //        break;

            //    default:
            //        SheetID = StaticData.GetNPCDialogueData(SheetID, number).Nexttalkid2;
            //        break;
            //}

            //number = 1;
            //comunicationAnimationController.SetBool((int)Animator.StringToHash("NextDialogue"), true);
        }

        switch (_type)
        {
            case 1:
                SheetID = StaticData.GetNPCDialogueData(SheetID, number).Nexttalkid1;
                break;

            default:
                SheetID = StaticData.GetNPCDialogueData(SheetID, number).Nexttalkid2;
                break;
        }

        number = 1;

        GetItem(_npcDialogueData);
        comunicationAnimationController.SetBool((int)Animator.StringToHash("NextDialogue"), true);
    }

    public void GetItem(NPCDialogueData _npcDialogueData)
    {
        int getID = _npcDialogueData.Getitemid;
        int getCount = _npcDialogueData.Getitemea;

        int outID = _npcDialogueData.Outitemid;
        int outCount = _npcDialogueData.Outitemea;

        List<int> itemID = new List<int>();
        List<int> itemCount = new List<int>();
        int totalCount = 0;

        if (getID > 0 && outID > 0)
        {
            for (int i = 0; i < playerData.ItemSlotData.ItemData.Length; i++)
            {
                if (playerData.ItemSlotData.ItemData[i].ID == outID)
                {
                    for (int j = 0; j < itemCount.Count; j++)
                    {
                        totalCount += itemCount[j];
                    }

                    if (playerData.ItemSlotData.ItemData[i].Count > outCount) // 5 + 3 >= 7
                    {
                        // 남은 totalCount 수량만큼 파괴
                        playerData.ItemSlotData.ItemData[i].Count -= outCount;

                        // 새로운 아이템 획득
                        Item newItem = new Item();
                        newItem.ItemID = getID;
                        ItemManager.Instance.Inventory.AcquireItem(newItem, getCount);

                        number = 1;
                    }
                    else if (playerData.ItemSlotData.ItemData[i].Count == outCount)
                    {
                        playerData.ItemSlotData.ItemData[i].ID = 0;
                        playerData.ItemSlotData.ItemData[i].Count = 0;

                        // 새로운 아이템 획득
                        Item newItem = new Item();
                        newItem.ItemID = getID;
                        ItemManager.Instance.Inventory.AcquireItem(newItem, getCount);

                        number = 1;
                    }
                    else if (playerData.ItemSlotData.ItemData[i].Count + totalCount >= outCount) // 5 + 3 >= 7
                    {
                        for (int k = 0; k < itemID.Count; k++)
                        {
                            playerData.ItemSlotData.ItemData[itemID[k]].ID = 0;
                            playerData.ItemSlotData.ItemData[itemID[k]].Count = 0;
                            totalCount -= itemCount[k];
                        }

                        // 남은 totalCount 수량만큼 파괴
                        playerData.ItemSlotData.ItemData[i].Count -= totalCount;

                        // 새로운 아이템 획득
                        Item newItem = new Item();
                        newItem.ItemID = getID;
                        ItemManager.Instance.Inventory.AcquireItem(newItem, getCount);

                        number = 1;
                    }
                    else
                    {
                        itemID.Add(i);
                        itemCount.Add(playerData.ItemSlotData.ItemData[i].Count);
                    }
                }
            }
        }
    }
}
