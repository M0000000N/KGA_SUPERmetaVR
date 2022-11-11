using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum LOBBYPLAYERSTATE
{
    SPAWNING,
    IDLE,
}

public class LobbyPlayerFSM : MonoBehaviourPun
{
    public Renderer myRenderer;
    public Material MyOpaqueMaterial { get { return opaqueMaterial; } }
    public Material MyTransparentMaterial { get { return transparentMaterial; } }

    #region 플레이어 상태들
    private LobbyPlayerSpawningState spawningState;
    private LobbyPlayerIdleState idleState;
    #endregion

    [SerializeField] private Material opaqueMaterial;
    [SerializeField] private Material transparentMaterial;

    private Dictionary<LOBBYPLAYERSTATE, LobbyPlayerState> states;
    private LobbyPlayerState nowState;

    private void Awake()
    {
        states = new Dictionary<LOBBYPLAYERSTATE, LobbyPlayerState>();

        myRenderer = GetComponentInChildren<Renderer>();

        spawningState = GetComponent<LobbyPlayerSpawningState>();
        idleState = GetComponent<LobbyPlayerIdleState>();

        AddState(LOBBYPLAYERSTATE.SPAWNING, spawningState);
        AddState(LOBBYPLAYERSTATE.IDLE, idleState);

        nowState = spawningState;
        nowState.OnEnter();
    }

    public void AddState(LOBBYPLAYERSTATE _key, LobbyPlayerState _state)
    {
        _state.Initialize(this);
        states[_key] = _state;
    }

    public void ChangeState(LOBBYPLAYERSTATE _key)
    {
        nowState.OnExit();
        nowState = states[_key];
        nowState.OnEnter();
    }

    public void UpdateFSM()
    {
        nowState.OnUpdate();
    }
}