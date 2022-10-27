using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Photon.Pun;
using System;

public static class FeefawfumTableInfo
{
    public static readonly string table_name = "FeefawfumData";

    public static readonly string user_id = "user_id";

    public static readonly string cooltime = "cooltime"; // 쿨타임 (이벤트를 진행한 시간)
    public static readonly string mission_count = "mission_count"; // 미션 완료 횟수

    public static readonly string create_at = "create_at";
    public static readonly string update_at = "update_at";
    public static readonly string is_delete = "is_delete";
}


public class FeefawfumDataBase : SingletonBehaviour<FeefawfumDataBase>
{
    private PlayerData playerData;

    void Start()
    {
        playerData = GameManager.Instance.PlayerData;
    }

    public void LoadFeefawfumData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(FeefawfumTableInfo.table_name, "*", FeefawfumTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                playerData.PlayerFeefawfumData.CoolTime = row[FeefawfumTableInfo.cooltime].ToString();
                playerData.PlayerFeefawfumData.ClearCount = int.Parse(row[FeefawfumTableInfo.mission_count].ToString());
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            CreateFeefawfumData();
        }
    }

    public void CreateFeefawfumData() // 데이터가 없으면 생성
    {
        DataBase.Instance.InsertDB(FeefawfumTableInfo.table_name, $"{FeefawfumTableInfo.user_id}, {FeefawfumTableInfo.create_at}, {FeefawfumTableInfo.update_at}", $"'{playerData.ID}', NOW(), NOW()");
    }

    public void FFFUpdateCooltime()
    {
        DataBase.Instance.sqlcmdall($"UPDATE {FeefawfumTableInfo.table_name} SET {FeefawfumTableInfo.cooltime} = NOW(), {FeefawfumTableInfo.mission_count} =  WHERE {FeefawfumTableInfo.user_id} = '{playerData.ID}'");
    }

    public bool FFFCheckCooltime()
    {
        return false;
    }

    public void FFFClearMission()
    {

    }

}
