using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;

public class PlayfabAccesssor
{
    private static PlayfabAccesssor _Instance;
    public static PlayfabAccesssor Instance{
        get{
            if(_Instance == null) _Instance = new PlayfabAccesssor().RequestLogin();
            return _Instance;
        }
    }
    private PlayfabAccesssor(){ }

    static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";

    public string ID{ get; private set; }
    private PlayfabAccesssor RequestLogin(){
        AddRequest(LoginOrSignUp);
        return this;
    }
    void LoginOrSignUp(){
        string lastID = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);
        //lastID.print();
        string requestID = string.IsNullOrEmpty(lastID) ? RandomStringUtil.GenerateString(32) : lastID;

        var request = new LoginWithCustomIDRequest(){
            CustomId = requestID,
            CreateAccount = string.IsNullOrEmpty(lastID)
        };
        PlayFabClientAPI.LoginWithCustomID(
            request,
            result => {
                if(!string.IsNullOrEmpty(lastID) || result.NewlyCreated){
                    ID = result.PlayFabId;
                    //ID.print();
                    PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, requestID);
                    PlayerPrefs.Save();
                    OnSuccess();
                }
                else LoginOrSignUp();
            },
            OnError
        );
    }

    public PlayfabAccesssor RequestSetDisplayName(string name){
        AddRequest(() => SetDisplayName(name));
        return this;
    }
    void SetDisplayName(string name){
        var request = new UpdateUserTitleDisplayNameRequest{
            DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            request,
            _ => OnSuccess(),
            OnError
        );
    }

    class Wrap<T>{ public T wrapped; }
    public PlayfabAccesssor RequestSendData<T>(string key, T data){
        //key.print();
        AddRequest(() => SendData(key, data));
        return this;
    }
    void SendData<T>(string key, T data){

        string dataStr = JsonUtility.ToJson(new Wrap<T>{ wrapped = data });

        //dataStr.print();

        UpdateUserDataRequest request = new UpdateUserDataRequest{
            Data = new Dictionary<string, string>(){
                {key, dataStr}
            }
        };
        PlayFabClientAPI.UpdateUserData(
            request,
            _ => OnSuccess(),
            OnError
        );
    }

    public PlayfabAccesssor RequestGetData<T>(string[] keys, Action<Dictionary<string, T>> callback){
        AddRequest(() => GetData<T>(keys, callback));
        return this;
    }
    void GetData<T>(string[] keys, Action<Dictionary<string, T>> callback){
        GetUserDataRequest request = new GetUserDataRequest{
            Keys = keys.ToList()
        };
        PlayFabClientAPI.GetUserData(
            request,
            result => {
                Dictionary<string, T> dataDict =
                    result.Data
                    .Select(data => new KeyValuePair<string, T>(data.Key, JsonUtility.FromJson<Wrap<T>>(data.Value.Value).wrapped))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                callback.Invoke(dataDict);
                OnSuccess();
            },
            OnError
        );
    }

    public PlayfabAccesssor RequestSendScoreToRanking(int score, string sequenceName){
        AddRequest(() => SendScoreToRanking(score, sequenceName));
        return this;
    }
    void SendScoreToRanking(int score, string sequenceName){
        StatisticUpdate update = new StatisticUpdate{
            StatisticName = "test" + sequenceName,
            Value = score
        };
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{ update }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            result => {
                OnSuccess();
            },
            OnError
        );
    }

    public PlayfabAccesssor RequestGetRanking(string sequenceName, Action<List<PlayerLeaderboardEntry>> callback){
        AddRequest(() => GetRanking(sequenceName, callback));
        return this;
    }
    void GetRanking(string sequenceName, Action<List<PlayerLeaderboardEntry>> callback){
        var request = new GetLeaderboardRequest(){
            StatisticName = "test" + sequenceName,
            StartPosition = 0,
            MaxResultsCount = 100
        };
        PlayFabClientAPI.GetLeaderboard(
            request,
            result => {
                callback.Invoke(result.Leaderboard);
                OnSuccess();
            },
            OnError
        );
    }

    void OnSuccess(){
        requestQueue.Dequeue();
        ExecuteRequest();
    }

    void OnError(PlayFabError error){
        Debug.Log(error.GenerateErrorReport());
        requestQueue.Dequeue();
        ExecuteRequest();
    }

    void AddRequest(Action action){
        requestQueue.Enqueue(action);

        if(requestQueue.Count == 1){
            ExecuteRequest();
        }
    }

    void ExecuteRequest(){
        if(requestQueue.Count > 0){
            requestQueue.Peek().Invoke();
        }
    }

    Queue<Action> requestQueue = new Queue<Action>();

}

public class RandomStringUtil{
    
    const string VALID_CHARACTERS = "1234567890qawsedrftgyhujikolp";
    static System.Random random = new System.Random();
    public static string GenerateString(int length){
        StringBuilder sb = new StringBuilder(length);
        for(int i = 0; i < length; i++){
            sb.Append(VALID_CHARACTERS[random.Next(VALID_CHARACTERS.Length)]);
        }
        return sb.ToString();
    }
}

public static class PrintUtil{
    public static void print(this object msg){
        Debug.Log(msg);
    }
}
