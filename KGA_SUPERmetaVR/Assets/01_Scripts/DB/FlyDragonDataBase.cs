using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Photon.Pun;
using System;

public class FlyDragonTableInfo
{
    public static readonly string table_name = "paperswandata";

    public static readonly string user_id = "user_id";

    public static readonly string cooltime = "cooltime"; // ��Ÿ�� (�̺�Ʈ�� ������ �ð�)
    public static readonly string today_count = "today_count"; // ���� �̼� �Ϸ� Ƚ��

    public static readonly string total_count = "total_count"; // �� �̼� �Ϸ� Ƚ��

    public static readonly string create_at = "create_at";
    public static readonly string update_at = "update_at";
    public static readonly string is_delete = "is_delete";
}

public class FlyDragonDataBase : SingletonBehaviour<FlyDragonDataBase>
{

    private PlayerData playerData;

    void Start()
    {
        playerData = GameManager.Instance.PlayerData;
    }

    public void CreateFlyDragonData() // �����Ͱ� ������ ����
    {
        DataBase.Instance.InsertDB(FlyDragonTableInfo.table_name, $"{FlyDragonTableInfo.user_id}, {FlyDragonTableInfo.create_at}, {FlyDragonTableInfo.update_at}", $"'{playerData.ID}', NOW(), NOW()");
    }

    public void LoadFlyDragonData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(FlyDragonTableInfo.table_name, "*", FlyDragonTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                CheckTodayData(row);

                // ������ ��
                playerData.PaperSwanData.CoolTime = row[FlyDragonTableInfo.cooltime].ToString();
                playerData.PaperSwanData.TodayCount = int.Parse(row[FlyDragonTableInfo.today_count].ToString());
                playerData.PaperSwanData.TotalCount = int.Parse(row[FlyDragonTableInfo.total_count].ToString());
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            UnityEngine.Debug.Log("<color=red>Paperswan �����Ͱ� �����ϴ�</color>");
        }
    }

    public bool UpdatePlayData()
    {
        LoadFlyDragonData();

        if (playerData.PaperSwanData.TodayCount < 3) // TODO : �÷��� Ƚ���� ���߿� Datatable�� ������ �� ������ ������ �߻��� �� �ֽ��ϴ�.
        {
            ++playerData.PaperSwanData.TodayCount;
            ++playerData.PaperSwanData.TotalCount;

            DataBase.Instance.sqlcmdall($"UPDATE {FlyDragonTableInfo.table_name} SET " +
                                        $"{FlyDragonTableInfo.cooltime} = NOW(), " +
                                        $"{FlyDragonTableInfo.today_count} = {playerData.PaperSwanData.TodayCount}, " +
                                        $"{FlyDragonTableInfo.total_count} = {playerData.PaperSwanData.TotalCount} " +
                                        $"WHERE {FlyDragonTableInfo.user_id} = '{playerData.ID}'");
            return true;
        }

        UnityEngine.Debug.Log("<color=red>���� �÷��� Ƚ���� ��� ����Ͽ����ϴ�.</color>");
        return false;
    }

    public bool CheckCooltime(float _cooltime)
    {
        if (playerData.PaperSwanData.CoolTime == "") return true;

        DateTime cooltime = DateTime.Parse(playerData.PaperSwanData.CoolTime);
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.
        cooltime = cooltime.AddHours(_cooltime);

        if (cooltime < nowtime) // �켱 ���� �����͸� �������� �Ǵ��Ѵ�.
        {
            DataTable dataTable = DataBase.Instance.FindDB(FlyDragonTableInfo.table_name, "*", FlyDragonTableInfo.user_id, GameManager.Instance.PlayerData.ID);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime cooltimeData = DateTime.Parse(row[FlyDragonTableInfo.cooltime].ToString());
                    cooltimeData = cooltimeData.AddHours(_cooltime);
                    if (cooltimeData < nowtime) // ���� ������ ������ �����ϱ� ���Ͽ� DB������ Ȯ���� �ٽ� ���� �Ѵ�.
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
        DateTime updateTime = DateTime.Parse(_row[FlyDragonTableInfo.update_at].ToString());
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.

        if ((nowtime - updateTime).Days > 0) // ��¥�� ������ ���
        {
            DataBase.Instance.sqlcmdall($"UPDATE {FlyDragonTableInfo.table_name} " +
                                        $"SET {FlyDragonTableInfo.today_count} = 0, " +
                                        $"{FlyDragonTableInfo.update_at} = NOW() " +
                                        $"WHERE {FlyDragonTableInfo.user_id} = '{playerData.ID}'");
        }
        else
        {
            UnityEngine.Debug.Log($"<color=red>���� �Ϸ簡 ������ �ʾҽ��ϴ�.</color> \n" +
                $"{nowtime} - {updateTime} = {(nowtime - updateTime).Days}");
        }
    }
}
