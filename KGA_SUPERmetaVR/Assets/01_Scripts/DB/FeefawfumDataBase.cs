using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Photon.Pun;
using System;

public static class FeeFawFumTableInfo
{
    public static readonly string table_name = "FeeFawFumData";

    public static readonly string user_id = "user_id";

    public static readonly string cooltime = "cooltime"; // ��Ÿ�� (�̺�Ʈ�� ������ �ð�)
    public static readonly string today_count = "today_count"; // ���� �̼� �Ϸ� Ƚ��

    public static readonly string total_count = "total_count"; // �� �̼� �Ϸ� Ƚ��

    public static readonly string create_at = "create_at";
    public static readonly string update_at = "update_at";
    public static readonly string is_delete = "is_delete";
}


public class FeeFawFumDataBase : SingletonBehaviour<FeeFawFumDataBase>
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
        DataBase.Instance.InsertDB(FeeFawFumTableInfo.table_name, $"{FeeFawFumTableInfo.user_id}, {FeeFawFumTableInfo.create_at}, {FeeFawFumTableInfo.update_at}", $"'{playerData.ID}', NOW(), NOW()");
    }

    public void LoadFeefawfumData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(FeeFawFumTableInfo.table_name, "*", FeeFawFumTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                CheckTodayData(row);

                playerData.FeeFawFumData.CoolTime = row[FeeFawFumTableInfo.cooltime].ToString();
                playerData.FeeFawFumData.TodayCount = int.Parse(row[FeeFawFumTableInfo.today_count].ToString());
                playerData.FeeFawFumData.TotalCount = int.Parse(row[FeeFawFumTableInfo.total_count].ToString());
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

        if (playerData.FeeFawFumData.TodayCount < 3) // TODO : �÷��� Ƚ���� ���߿� Datatable�� ������ �� ������ ������ �߻��� �� �ֽ��ϴ�.
        {
            ++playerData.FeeFawFumData.TodayCount;
            ++playerData.FeeFawFumData.TotalCount;

            DataBase.Instance.sqlcmdall($"UPDATE {FeeFawFumTableInfo.table_name} SET " +
                                        $"{FeeFawFumTableInfo.cooltime} = NOW(), " +
                                        $"{FeeFawFumTableInfo.today_count} = {playerData.FeeFawFumData.TodayCount}, " +
                                        $"{FeeFawFumTableInfo.total_count} = {playerData.FeeFawFumData.TotalCount} " +
                                        $"WHERE {FeeFawFumTableInfo.user_id} = '{playerData.ID}'");
            return true;
        }

        UnityEngine.Debug.Log("<color=red>���� �÷��� Ƚ���� ��� ����Ͽ����ϴ�.</color>");
        return false;
    }

    public bool CheckCooltime(float _cooltime)
    {
        if (playerData.FeeFawFumData.CoolTime == "") return true;

        DateTime cooltime = DateTime.Parse(playerData.FeeFawFumData.CoolTime);
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.
        cooltime = cooltime.AddHours(_cooltime);

        if(cooltime < nowtime) // �켱 ���� �����͸� �������� �Ǵ��Ѵ�.
        {
            DataTable dataTable = DataBase.Instance.FindDB(FeeFawFumTableInfo.table_name, "*", FeeFawFumTableInfo.user_id, GameManager.Instance.PlayerData.ID);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime cooltimeData = DateTime.Parse(row[FeeFawFumTableInfo.cooltime].ToString());
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
        DateTime updateTime = DateTime.Parse(_row[FeeFawFumTableInfo.update_at].ToString());
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.

        if((nowtime - updateTime).Days > 0) // ��¥�� ������ ���
        {
            DataBase.Instance.sqlcmdall($"UPDATE {FeeFawFumTableInfo.table_name} " +
                                        $"SET {FeeFawFumTableInfo.today_count} = 0, " +
                                        $"{FeeFawFumTableInfo.update_at} = NOW() " +
                                        $"WHERE {FeeFawFumTableInfo.user_id} = '{playerData.ID}'");
        }
        else
        {
            UnityEngine.Debug.Log($"<color=red>���� �Ϸ簡 ������ �ʾҽ��ϴ�.</color> \n" +
                $"{nowtime} - {updateTime} = {(nowtime - updateTime).Days}");
        }
    }
}
