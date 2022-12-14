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

    public static readonly string cooltime = "cooltime"; // 쿨타임 (이벤트를 진행한 시간)
    public static readonly string be_rewarded = "be_rewarded"; // 보상 받았는지 확인
    public static readonly string today_count = "today_count"; // 오늘 미션 완료 횟수

    public static readonly string total_count = "total_count"; // 총 미션 완료 횟수

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

    public void CreateFlyDragonData(string _id) // 데이터가 없으면 생성
    {
        DataBase.Instance.InsertDB(FlyDragonTableInfo.table_name, 
            $"{FlyDragonTableInfo.user_id}, {FlyDragonTableInfo.create_at}, {FlyDragonTableInfo.update_at}", 
            $"'{_id}', NOW(), NOW()");
    }

    public void LoadFlyDragonData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(FlyDragonTableInfo.table_name, "*", FlyDragonTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                CheckTodayData(row);

                // 데이터 들어감
                playerData.PaperSwanData.CoolTime = row[FlyDragonTableInfo.cooltime].ToString();
                playerData.PaperSwanData.beRewarded = int.Parse(row[FlyDragonTableInfo.be_rewarded].ToString());
                playerData.PaperSwanData.TodayCount = int.Parse(row[FlyDragonTableInfo.today_count].ToString());
                playerData.PaperSwanData.TotalCount = int.Parse(row[FlyDragonTableInfo.total_count].ToString());
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            UnityEngine.Debug.Log("<color=red>Paperswan 데이터가 없습니다</color>");
        }
    }

    public bool UpdatePlayData()
    {
        LoadFlyDragonData();

        if (playerData.PaperSwanData.TodayCount > -1) // 횟수 8 -> 무제한
        {
            ++playerData.PaperSwanData.TodayCount;
            ++playerData.PaperSwanData.TotalCount;

            DataBase.Instance.sqlcmdall($"UPDATE {FlyDragonTableInfo.table_name} SET " +
                                        $"{FlyDragonTableInfo.cooltime} = NOW(), " +
                                        $"{FlyDragonTableInfo.be_rewarded} = {playerData.PaperSwanData.beRewarded}, " +
                                        $"{FlyDragonTableInfo.today_count} = {playerData.PaperSwanData.TodayCount}, " +
                                        $"{FlyDragonTableInfo.total_count} = {playerData.PaperSwanData.TotalCount} " +
                                        $"WHERE {FlyDragonTableInfo.user_id} = '{playerData.ID}'");
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
            DataTable dataTable = DataBase.Instance.FindDB(FlyDragonTableInfo.table_name, "*", FlyDragonTableInfo.user_id, GameManager.Instance.PlayerData.ID);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime cooltimeData = DateTime.Parse(row[FlyDragonTableInfo.cooltime].ToString());
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
        DateTime updateTime = DateTime.Parse(_row[FlyDragonTableInfo.update_at].ToString());
        DateTime nowtime = DateTime.UtcNow; // TODO : 현재 UTC 기준으로 9시간 차이가 있습니다.

        if (nowtime.Day - updateTime.Day > 0) // 날짜가 지났을 경우
        {
            DataBase.Instance.sqlcmdall($"UPDATE {FlyDragonTableInfo.table_name} " +
                                        $"SET {FlyDragonTableInfo.today_count} = 0, " +
                                        $"{FlyDragonTableInfo.update_at} = NOW() " +
                                        $"WHERE {FlyDragonTableInfo.user_id} = '{playerData.ID}'");
        }
        else
        {

        }
    }
}
