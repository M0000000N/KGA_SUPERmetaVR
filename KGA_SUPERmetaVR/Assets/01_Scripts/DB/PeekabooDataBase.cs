using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public static class PeekabooTableInfo
{
    public static readonly string table_name = "peekaboodata";

    public static readonly string user_id = "user_id";
    public static readonly string nickname = "nickname";

    public static readonly string character_now = "character_now"; // �������� ĳ����
    public static readonly string character_list = "character_list"; // ĳ���� ����Ʈ

    public static readonly string play_count = "play_count"; // �÷��� Ƚ��
    public static readonly string win_count = "win_count"; // �¸� Ƚ��
    public static readonly string die_count = "die_count"; // ��� Ƚ��
    public static readonly string survive_time = "survive_time"; // ���� �ð�
    public static readonly string attack_player = "attack_player"; // PC ���� Ƚ��
    public static readonly string attack_npc = "attack_npc"; // NPC ���� Ƚ��

    public static readonly string create_at = "create_at";
    public static readonly string update_at = "update_at";
    public static readonly string is_delete = "is_delete";
}

public class PeekabooDataBase : SingletonBehaviour<PeekabooDataBase>
{

    private PlayerData playerData;
    private string playerPeekabooCharacterList;

    void Start()
    {
        playerData = GameManager.Instance.PlayerData;
    }

    public void CreatePeekabooData() // �����Ͱ� ������ ����
    {
        DataBase.Instance.InsertDB(PeekabooTableInfo.table_name, $"{PeekabooTableInfo.user_id}, {PeekabooTableInfo.create_at}, {PeekabooTableInfo.update_at}", $"'{playerData.ID}', NOW(), NOW()");
    }

    public void SaveCharacterList() // ĳ���� ���� �� ������� �߻���
    {
        playerPeekabooCharacterList = JsonUtility.ToJson(playerData.PeekabooData.CharacterList);
        UnityEngine.Debug.Log("playerPeekabooCharacterList : " + playerPeekabooCharacterList);
        DataBase.Instance.UpdateDB(PeekabooTableInfo.table_name, PeekabooTableInfo.character_list, playerPeekabooCharacterList, PeekabooTableInfo.user_id, playerData.ID);
    }

    public void SaveSelectCharater() // ���� ��������� ������ ����
    {
        int selectCharacter = playerData.PeekabooData.SelectCharacter;
        DataBase.Instance.UpdateDB(PeekabooTableInfo.table_name, PeekabooTableInfo.character_now, selectCharacter.ToString(), PeekabooTableInfo.user_id, playerData.ID);
    }

    public void LoadPeekabooData() // �α��ν� �����Ͱ� �ִٸ� ����, ������ Create�� �̵� �ʿ�
    {
        DataTable dataTable = DataBase.Instance.FindDB(PeekabooTableInfo.table_name, "*", UserTableInfo.user_id, playerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            // TODO : �г��� ���� �߰��� �� �ű� �ڵ�
            DataBase.Instance.UpdateDB(PeekabooTableInfo.table_name, PeekabooTableInfo.nickname, playerData.Nickname, PeekabooTableInfo.user_id, playerData.ID);

            foreach (DataRow row in dataTable.Rows)
            {
                playerData.PeekabooData.SelectCharacter = int.Parse(row[PeekabooTableInfo.character_now].ToString());
                playerPeekabooCharacterList = row[PeekabooTableInfo.character_list].ToString();
                playerData.PeekabooData.CharacterList = JsonUtility.FromJson<PlayerCharacter>(playerPeekabooCharacterList);

                playerData.PeekabooData.PlayCount = int.Parse(row[PeekabooTableInfo.play_count].ToString());
                playerData.PeekabooData.WinCount = int.Parse(row[PeekabooTableInfo.win_count].ToString());
                playerData.PeekabooData.DieCount = int.Parse(row[PeekabooTableInfo.die_count].ToString());
                playerData.PeekabooData.SurviveTime = float.Parse(row[PeekabooTableInfo.survive_time].ToString());
                playerData.PeekabooData.AttackPC = int.Parse(row[PeekabooTableInfo.attack_player].ToString());
                playerData.PeekabooData.AttackNPC = int.Parse(row[PeekabooTableInfo.attack_npc].ToString());
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            CreatePeekabooData();
        }
    }

    public DataTable SortRanking(string _rankingData, string _sort = "DESC")
    {
        DataTable dataTable = DataBase.Instance.SortDB(PeekabooTableInfo.table_name, "*", _rankingData, _sort);

        return dataTable;
    }
}
