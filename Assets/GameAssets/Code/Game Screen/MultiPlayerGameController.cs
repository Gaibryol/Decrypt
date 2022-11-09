using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class MultiPlayerGameController : GameController, IOnEventCallback
{
    private List<Player> loadedPlayers = new List<Player>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        InitPhotonState();
    }


    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);

    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);

    }

    protected override void Update()
    {
        base.Update();
        SyncScores();
    }

    private void InitPhotonState()
    {
        loadedPlayers.Clear();
        // Update player state to be in game. Updates player custom property.
        PhotonController.Instance.UpdatePlayerState("PlayerState", "Game");

        // Clients automatically sync with master. Conventional method of ChangeState doesn't work
        GameManager.Instance.GameState = Constants.GameStates.Game;

        // Stop syncing scenes and set next scene to room scene. Allows navigation to room scene without sync.
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public override void StartGame()
    {
        InitVariables();
        gameUI.StartGame();
    }

    protected override void InitVariables()
    {
        base.InitVariables();
        WordsManager.Instance.UpdatePrefs();
    }

    protected override bool EndCondition()
    {
        bool end = false;
        if (GameManager.Instance.GamePrefs.GameType == Constants.GameType.Default)
        {
            end = base.EndCondition();
        } else if (GameManager.Instance.GamePrefs.GameType == Constants.GameType.Timed)
        {
            end = base.EndCondition() || gameTime >= GameManager.Instance.GamePrefs.Timer;
        }
        return end;
    }

    private void SyncScores()
    {
        PhotonController.Instance.UpdatePlayerState("Score", playerPoints);
        PhotonController.Instance.UpdatePlayerState("Hacks", HacksManager.Instance.ActivatedHacks.Select(x => x.GetDescription()).ToArray());
    }

    protected override void EndGame()
    {
        StopAllCoroutines();
        GameManager.Instance.ChangeState(Constants.GameStates.End);
    }

    public void OnEvent(EventData photonEvent)
    {
        // Start game when all players have loaded into the scene
        if (photonEvent.Code == Constants.PlayerReadyEventCode)
        {
            if (!loadedPlayers.Contains((Player)photonEvent.CustomData))
            {
                loadedPlayers.Add((Player)photonEvent.CustomData);
                if (loadedPlayers.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    // All players loaded, send start code
                    PhotonController.Instance.SendPhotonEvent(Constants.GameStartEventCode, PhotonNetwork.Time, ReceiverGroup.All);
                }
            }
        }
        else if (photonEvent.Code == Constants.GameStartEventCode)
        {
            StartGame();
            PhotonController.Instance.SetNextScene("EndScene");
        }

    }
}
