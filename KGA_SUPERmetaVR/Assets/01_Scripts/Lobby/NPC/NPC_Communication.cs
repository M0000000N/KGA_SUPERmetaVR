using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC_Communication : MonoBehaviour
{
    [SerializeField] private int npcID;

    [SerializeField] private int startSheetID;

    private int sheetID;
    public int SheetID { get { return sheetID; } set { sheetID = value; } }

    [SerializeField] private int number;

    [SerializeField] private GameObject handshake;
    [SerializeField] private GameObject communication;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button[] button;

    PlayerData playerData;

    [SerializeField] private Animator comunicationAnimationController;
    private bool isComunicationAnimationEnd;

    private void Start()
    {
        // id 설정해주기
        playerData = GameManager.Instance.PlayerData;
        comunicationAnimationController = transform.GetComponent<Animator>();

        handshake.SetActive(false);
        communication.SetActive(false);

        Initialize();
        SetDialogue();
    }

    public void Initialize()
    {
        number = 1;
        SheetID = startSheetID;
        isComunicationAnimationEnd = true;
        SetDialogue();
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

    // 시작
    public void OnPressHand()
    {
        Initialize();
        handshake.SetActive(false);
        communication.SetActive(true);
    }

    // 대화
    public void SetDialogue()
    {
        comunicationAnimationController.SetBool((int)Animator.StringToHash("NextDialogue"), false);
        isComunicationAnimationEnd = true;
        NPCDialogueData npcDialogueData = StaticData.GetNPCDialogueData(SheetID, number);

        if (npcDialogueData == null) return;

        dialogueText.text = npcDialogueData.Talk;

        button[0].onClick.RemoveAllListeners();
        button[1].onClick.RemoveAllListeners();

        // 버튼1
        if (npcDialogueData.SELECT1.Equals(string.Empty))
        {
            NPCDialogueData nextNPCDialogueData = StaticData.GetNPCDialogueData(SheetID, number + 1);
            string text = string.Empty;
            if (nextNPCDialogueData != null)
            {
                text = "다음";
                button[0].onClick.AddListener(() => { NextNumber(); });
            }
            else
            {
                text = "확인";
                button[0].onClick.AddListener(() => { EndCommunication(); });
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

    public void EndCommunication()
    {
        if (isComunicationAnimationEnd == false) return;
        isComunicationAnimationEnd = false;

        communication.SetActive(false);
    }

    public void NextNumber()
    {
        if (isComunicationAnimationEnd == false) return;
        isComunicationAnimationEnd = false;

        number++;
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
                        break;
                    }
                    else
                    {
                        itemID.Add(i);
                        itemID.Add(playerData.ItemSlotData.ItemData[i].Count);
                    }
                }
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
            comunicationAnimationController.SetBool((int)Animator.StringToHash("NextDialogue"), true);
        }

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

                    if (playerData.ItemSlotData.ItemData[i].Count + totalCount >= outCount) // 5 + 3 >= 7
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
