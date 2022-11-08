using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Photon.Pun;
using System;

public class PaperSwanTableInfo
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

public class PaperSwanDataBase : SingletonBehaviour<PaperSwanDataBase>
{

    private PlayerData playerData;

    void Start()
    {
        playerData = GameManager.Instance.PlayerData;
    }

    public void CreatePaperswanData() // �����Ͱ� ������ ����
    {
        DataBase.Instance.InsertDB(PaperSwanTableInfo.table_name, $"{PaperSwanTableInfo.user_id}, {PaperSwanTableInfo.create_at}, {PaperSwanTableInfo.update_at}", $"'{playerData.ID}', NOW(), NOW()");
    }

    public void LoadPaperswanData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(PaperSwanTableInfo.table_name, "*", PaperSwanTableInfo.user_id, GameManager.Instance.PlayerData.ID);
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
            UnityEngine.Debug.Log("<color=red>Paperswan �����Ͱ� �����ϴ�</color>");
        }
    }

    public bool UpdatePlayData()
    {
        LoadPaperswanData();

        if (playerData.PaperSwanData.TodayCount < 3) // TODO : �÷��� Ƚ���� ���߿� Datatable�� ������ �� ������ ������ �߻��� �� �ֽ��ϴ�.
        {
            ++playerData.PaperSwanData.TodayCount;
            ++playerData.PaperSwanData.TotalCount;

            DataBase.Instance.sqlcmdall($"UPDATE {PaperSwanTableInfo.table_name} SET " +
                                        $"{PaperSwanTableInfo.cooltime} = NOW(), " +
                                        $"{PaperSwanTableInfo.today_count} = {playerData.PaperSwanData.TodayCount}, " +
                                        $"{PaperSwanTableInfo.total_count} = {playerData.PaperSwanData.TotalCount} " +
                                        $"WHERE {PaperSwanTableInfo.user_id} = '{playerData.ID}'");
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
            DataTable dataTable = DataBase.Instance.FindDB(PaperSwanTableInfo.table_name, "*", PaperSwanTableInfo.user_id, GameManager.Instance.PlayerData.ID);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime cooltimeData = DateTime.Parse(row[PaperSwanTableInfo.cooltime].ToString());
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
        DateTime updateTime = DateTime.Parse(_row[PaperSwanTableInfo.update_at].ToString());
        DateTime nowtime = DateTime.UtcNow; // TODO : ���� UTC �������� 9�ð� ���̰� �ֽ��ϴ�.

        if ((nowtime - updateTime).Days > 0) // ��¥�� ������ ���
        {
            DataBase.Instance.sqlcmdall($"UPDATE {PaperSwanTableInfo.table_name} " +
                                        $"SET {PaperSwanTableInfo.today_count} = 0, " +
                                        $"{PaperSwanTableInfo.update_at} = NOW() " +
                                        $"WHERE {PaperSwanTableInfo.user_id} = '{playerData.ID}'");
        }
        else
        {

        }
    }
}
