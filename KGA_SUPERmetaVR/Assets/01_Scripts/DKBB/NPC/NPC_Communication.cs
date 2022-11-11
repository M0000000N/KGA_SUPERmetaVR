using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC_Communication : MonoBehaviour
{
    private int sheetID;
    public int SheetID { get { return sheetID; } set { sheetID = value; } }

    private int number;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button[] button;

    PlayerData playerData;

    private void Start()
    {
        // id �������ֱ�
        playerData = GameManager.Instance.PlayerData;

        SheetID = 27002;
        Initialize();
    }

    public void Initialize()
    {
        number = 1;
        SetDialogue();
    }

    public void SetDialogue()
    {
        NPCDialogueData npcDialogueData = StaticData.GetNPCDialogueData(SheetID, number);

        if (npcDialogueData == null) return;

        dialogueText.text = npcDialogueData.Talk;

        button[0].onClick.RemoveAllListeners();
        button[1].onClick.RemoveAllListeners();

        // ��ư1
        if (npcDialogueData.SELECT1.Equals(string.Empty))
        {
            NPCDialogueData nextNPCDialogueData = StaticData.GetNPCDialogueData(SheetID, number+1);
            string text = string.Empty;
            if (nextNPCDialogueData != null)
            {
                text = "����";
                button[0].onClick.AddListener(() => { NextNumber(); });
            }
            else
            {
                text = "Ȯ��";
                button[0].onClick.AddListener(() => { EndCommunication(); });
            }
            button[0].GetComponentInChildren<TextMeshProUGUI>().text = text;


            button[0].gameObject.SetActive(true);
        }
        else
        {
            button[0].GetComponentInChildren<TextMeshProUGUI>().text = npcDialogueData.SELECT1;
            button[0].onClick.AddListener(() => { CheckCondition(1, npcDialogueData.Select1condition1, npcDialogueData.Condition1quantity); });
            button[0].gameObject.SetActive(true);
        }

        // ��ư2
        if (npcDialogueData.SELECT2.Equals(string.Empty))
        {
            button[1].gameObject.SetActive(false);
        }
        else
        {
            button[1].GetComponentInChildren<TextMeshProUGUI>().text = npcDialogueData.SELECT2;
            button[1].onClick.AddListener(() => { CheckCondition(2, npcDialogueData.Select2condition2, npcDialogueData.Condition2quantity); });
            button[1].gameObject.SetActive(true);
        }
    }

    public void EndCommunication()
    {

    }

    public void NextNumber()
    {
        number++;
        SetDialogue();
    }

    public bool CheckCondition(int _type, int _id, int _count)
    {
        List<int> itemID = new List<int>();
        List<int> itemCount = new List<int>();
        int totalCount = 0;

        for (int i = 0; i < playerData.ItemSlotData.ItemData.Length; i++)
        {
            if(playerData.ItemSlotData.ItemData[i].ID == _id && _id > 0)
            {
                for (int j = 0; j < itemCount.Count; j++)
                {
                    totalCount += itemCount[j];
                }

                if(playerData.ItemSlotData.ItemData[i].Count + totalCount >= _count)
                {
                    for (int k = 0; k < itemID.Count; k++)
                    {
                        // �� �������� ��� ������ �ı� playerData.ItemSlotData.ItemData[id[k]]
                        totalCount -= itemCount[k];
                    }

                    // �� �������� ���� totalCount ������ŭ �ı� playerData.ItemSlotData.ItemData[i]

                    // inventory -> AcqireItem
                    // TODO : ���ο� ������ ������ �ʿ��ϸ� ������ ��
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
                    SetDialogue();
                    return true;
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
        SetDialogue();
        return false;
    }
}
