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

    public static readonly string cooltime = "cooltime"; // 쿨타임 (이벤트를 진행한 시간)
    public static readonly string today_count = "today_count"; // 오늘 미션 완료 횟수

    public static readonly string total_count = "total_count"; // 총 미션 완료 횟수

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

    public void CreatePaperswanData() // 데이터가 없으면 생성
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

                // 데이터 들어감

                //playerData.PlayerFeefawfumData.CoolTime = row[FeefawfumTableInfo.cooltime].ToString();
                //playerData.PlayerFeefawfumData.TodayCount = int.Parse(row[FeefawfumTableInfo.today_count].ToString());
                //playerData.PlayerFeefawfumData.TotalCount = int.Parse(row[FeefawfumTableInfo.total_count].ToString());
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            UnityEngine.Debug.Log("<color=red>Paperswan 데이터가 없습니다</color>");
        }
    }

    public bool UpdatePlayData()
    {
        LoadPaperswanData();

        if (playerData.PaperSwanData.TodayCount < 3) // TODO : 플레이 횟수는 나중에 Datatable로 관리될 수 있으니 수정이 발생할 수 있습니다.
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

        UnityEngine.Debug.Log("<color=red>오늘 플레이 횟수를 모두 사용하였습니다.</color>");
        return false;
    }

    public bool CheckCooltime(float _cooltime)
    {
        if (playerData.PaperSwanData.CoolTime == "") return true;

        DateTime cooltime = DateTime.Parse(playerData.PaperSwanData.CoolTime);
        DateTime nowtime = DateTime.UtcNow; // TODO : 현재 UTC 기준으로 9시간 차이가 있습니다.
        cooltime = cooltime.AddHours(_cooltime);

        if (cooltime < nowtime) // 우선 로컬 데이터를 기준으로 판단한다.
        {
            DataTable dataTable = DataBase.Instance.FindDB(PaperSwanTableInfo.table_name, "*", PaperSwanTableInfo.user_id, GameManager.Instance.PlayerData.ID);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime cooltimeData = DateTime.Parse(row[PaperSwanTableInfo.cooltime].ToString());
                    cooltimeData = cooltimeData.AddHours(_cooltime);
                    if (cooltimeData < nowtime) // 로컬 데이터 변조를 방지하기 위하여 DB데이터 확인을 다시 진행 한다.
                    {
                        UnityEngine.Debug.Log($"{nowtime - cooltimeData} <color=blue> : 쿨타임이 지나 실행되었습니다.</color>");
                        return true;
                    }
                }
            }
        }

        UnityEngine.Debug.Log($"{nowtime - cooltime} <color=red> : 아직 쿨타임이 지나지 않았습니다.</color>");
        return false;
    }

    public void CheckTodayData(DataRow _row)
    {
        DateTime updateTime = DateTime.Parse(_row[PaperSwanTableInfo.update_at].ToString());
        DateTime nowtime = DateTime.UtcNow; // TODO : 현재 UTC 기준으로 9시간 차이가 있습니다.

        if ((nowtime - updateTime).Days > 0) // 날짜가 지났을 경우
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
