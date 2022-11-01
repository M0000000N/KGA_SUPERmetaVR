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

    public static readonly string cooltime = "cooltime"; // ��Ÿ�� (�̺�Ʈ�� ������ �ð�)
    public static readonly string today_count = "today_count"; // ���� �̼� �Ϸ� Ƚ��

    public static readonly string total_count = "total_count"; // �� �̼� �Ϸ� Ƚ��

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

    // �׽�Ʈ �ڵ�
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(CheckCooltime(3))
            {
                UpdatePlayData();
            }
        }
    }
    // �׽�Ʈ �ڵ�

    public void CreateFeefawfumData() // �����Ͱ� ������ ����
    {
        DataBase.Instance.InsertDB(FeefawfumTableInfo.table_name, $"{FeefawfumTableInfo.user_id}, {FeefawfumTableInfo.create_at}, {FeefawfumTableInfo.update_at}", $"'{playerData.ID}', NOW(), NOW()");
    }

    public void LoadFeefawfumData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(FeefawfumTableInfo.table_name, "*", FeefawfumTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                CheckTodayData(row);

                playerData.PlayerFeefawfumData.CoolTime = row[FeefawfumTableInfo.cooltime].ToString();
                playerData.PlayerFeefawfumData.TodayCount = int.Parse(row[FeefawfumTableInfo.today_count].ToString());
                playerData.PlayerFeefawfumData.TotalCount = int.Parse(row[FeefawfumTableInfo.total_count].ToString());
            }


        }
        else if (dataTable.Rows.Count <= 0)
        {
            UnityEngine.Debug.Log("<color=red>FFF �����Ͱ� �����ϴ�</color>");
        }
    }

    public bool UpdatePlayData()
    {
        LoadFeefawfumData();

        if (playerData.PlayerFeefawfumData.TodayCount < 3) // TODO : �÷��� Ƚ���� ���߿� Datatable�� ������ �� ������ ������ �߻��� �� �ֽ��ϴ�.
        {
            ++playerData.PlayerFeefawfumData.TodayCount;
            ++playerData.PlayerFeefawfumData.TotalCount;

            DataBase.Instance.sqlcmdall($"UPDATE {FeefawfumTableInfo.table_name} SET " +
                                        $"{FeefawfumTableInfo.cooltime} = NOW(), " +
                                        $"{FeefawfumTableInfo.today_count} = {playerData.PlayerFeefawfumData.TodayCount}, " +
                                        $"{FeefawfumTableInfo.total_count} = {playerData.PlayerFeefawfumData.TotalCount} " +
                                        $"WHERE {FeefawfumTableInfo.user_id} = '{playerData.ID}'");
            return true;
        }

        UnityEngine.Debug.Log("<color=red>���� �÷��� Ƚ���� ��� ����Ͽ����ϴ�.</color>");
        return false;
    }

    public bool CheckCooltime(float _cooltime)
    {
        if (playerData.PlayerFeefawfumData.CoolTime == "") return true;

        DateTime cooltime = DateTime.Parse(playerData.PlayerFeefawfumData.CoolTime);
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.
        cooltime = cooltime.AddHours(_cooltime);

        if(cooltime < nowtime) // �켱 ���� �����͸� �������� �Ǵ��Ѵ�.
        {
            DataTable dataTable = DataBase.Instance.FindDB(FeefawfumTableInfo.table_name, "*", FeefawfumTableInfo.user_id, GameManager.Instance.PlayerData.ID);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime cooltimeData = DateTime.Parse(row[FeefawfumTableInfo.cooltime].ToString());
                    cooltimeData = cooltimeData.AddHours(_cooltime);
                    if(cooltimeData < nowtime) // ���� ������ ������ �����ϱ� ���Ͽ� DB������ Ȯ���� �ٽ� ���� �Ѵ�.
                    {
                        UnityEngine.Debug.Log($"{nowtime - cooltimeData} <color=blue> : ��Ÿ���� ���� ����Ǿ����ϴ�.</color>");
                        return true;
                    }
                }
            }
        }
        
        UnityEngine.Debug.Log($"{nowtime - cooltime} <color=red> : ���� ��Ÿ���� ������ �ʾҽ��ϴ�.</color>");
        return false;
    }

    public void CheckTodayData(DataRow _row)
    {
        DateTime updateTime = DateTime.Parse(_row[FeefawfumTableInfo.update_at].ToString());
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.

        if((nowtime - updateTime).Days > 0) // ��¥�� ������ ���
        {
            DataBase.Instance.sqlcmdall($"UPDATE {FeefawfumTableInfo.table_name} " +
                                        $"SET {FeefawfumTableInfo.today_count} = 0, " +
                                        $"{FeefawfumTableInfo.update_at} = NOW() " +
                                        $"WHERE {FeefawfumTableInfo.user_id} = '{playerData.ID}'");
        }
        else
        {
            UnityEngine.Debug.Log($"<color=red>���� �Ϸ簡 ������ �ʾҽ��ϴ�.</color> \n" +
                $"{nowtime} - {updateTime} = {(nowtime - updateTime).Days}");
        }
    }
}
