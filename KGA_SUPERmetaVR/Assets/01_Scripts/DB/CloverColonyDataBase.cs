using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Photon.Pun;
using System;

public class CloverColonyTableInfo
{
    public static readonly string table_name = "clovercolonydata";

    public static readonly string user_id = "user_id";

    public static readonly string cooltime = "cooltime"; // ��Ÿ�� (�̺�Ʈ�� ������ �ð�)
    public static readonly string today_count = "today_count"; // ���� �̼� �Ϸ� Ƚ��

    public static readonly string total_count = "total_count"; // �� �̼� �Ϸ� Ƚ��

    public static readonly string create_at = "create_at";
    public static readonly string update_at = "update_at";
    public static readonly string is_delete = "is_delete";
}

public class CloverColonyDataBase : SingletonBehaviour<CloverColonyDataBase>
{

    private PlayerData playerData;

    void Start()
    {
        playerData = GameManager.Instance.PlayerData;
    }

    public void CreateCloverColonyData() // �����Ͱ� ������ ����
    {
        DataBase.Instance.InsertDB(CloverColonyTableInfo.table_name, $"{CloverColonyTableInfo.user_id}, {CloverColonyTableInfo.create_at}, {CloverColonyTableInfo.update_at}", $"'{playerData.ID}', NOW(), NOW()");
    }

    public void LoadCloverColonyData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(CloverColonyTableInfo.table_name, "*", CloverColonyTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                CheckTodayData(row);

                // ������ ��

                //playerData.PlayerFeefawfumData.CoolTime = row[FeefawfumTableInfo.cooltime].ToString();
                //playerData.PlayerFeefawfumData.TodayCount = int.Parse(row[FeefawfumTableInfo.today_count].ToString());
                //playerData.PlayerFeefawfumData.TotalCount = int.Parse(row[FeefawfumTableInfo.total_count].ToString());
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            UnityEngine.Debug.Log("<color=red>CloverColony �����Ͱ� �����ϴ�</color>");
        }
    }

    public bool UpdatePlayData()
    {
        LoadCloverColonyData();

        if (playerData.CloverColonyData.TodayCount < 3) // TODO : �÷��� Ƚ���� ���߿� Datatable�� ������ �� ������ ������ �߻��� �� �ֽ��ϴ�.
        {
            ++playerData.CloverColonyData.TodayCount;
            ++playerData.CloverColonyData.TotalCount;

            DataBase.Instance.sqlcmdall($"UPDATE {CloverColonyTableInfo.table_name} SET " +
                                        $"{CloverColonyTableInfo.cooltime} = NOW(), " +
                                        $"{CloverColonyTableInfo.today_count} = {playerData.CloverColonyData.TodayCount}, " +
                                        $"{CloverColonyTableInfo.total_count} = {playerData.CloverColonyData.TotalCount} " +
                                        $"WHERE {CloverColonyTableInfo.user_id} = '{playerData.ID}'");
            return true;
        }

        UnityEngine.Debug.Log("<color=red>���� �÷��� Ƚ���� ��� ����Ͽ����ϴ�.</color>");
        return false;
    }

    public bool CheckCooltime(float _cooltime)
    {
        if (playerData.CloverColonyData.CoolTime == "") return true;

        DateTime cooltime = DateTime.Parse(playerData.CloverColonyData.CoolTime);
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.
        cooltime = cooltime.AddHours(_cooltime);

        if (cooltime < nowtime) // �켱 ���� �����͸� �������� �Ǵ��Ѵ�.
        {
            DataTable dataTable = DataBase.Instance.FindDB(CloverColonyTableInfo.table_name, "*", CloverColonyTableInfo.user_id, GameManager.Instance.PlayerData.ID);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime cooltimeData = DateTime.Parse(row[CloverColonyTableInfo.cooltime].ToString());
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
        DateTime updateTime = DateTime.Parse(_row[CloverColonyTableInfo.update_at].ToString());
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.

        if ((nowtime - updateTime).Days > 0) // ��¥�� ������ ���
        {
            DataBase.Instance.sqlcmdall($"UPDATE {CloverColonyTableInfo.table_name} " +
                                        $"SET {CloverColonyTableInfo.today_count} = 0, " +
                                        $"{CloverColonyTableInfo.update_at} = NOW() " +
                                        $"WHERE {CloverColonyTableInfo.user_id} = '{playerData.ID}'");
        }
        else
        {

        }
    }
}
