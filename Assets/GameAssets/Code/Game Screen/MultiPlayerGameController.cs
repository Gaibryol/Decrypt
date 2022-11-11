using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System;

public class MultiPlayerGameController : GameController, IOnEventCallback
{
    [SerializeField] private List<Player> loadedPlayers = new List<Player>();

    private Dictionary<Player, int> playerScores;

    [Header("Battle Royal Settings")]
    [SerializeField] private float playableRegionShrinkStartTime;
    [SerializeField] private float playableRegionStrinkInterval;

    protected override void Start()
    {
        base.Start();
        gameUI = GetComponent<GameUIController>();

        InitPhotonState();
        playerScores = new Dictionary<Player, int>();
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
        SyncScores();   // for now, later move to word solved
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
        if (GameManager.Instance.GamePrefs.GameType == Constants.GameType.BR)
        {
            InvokeRepeating("MovePlayableRegion", playableRegionShrinkStartTime, playableRegionStrinkInterval);
        }
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
        } else if (GameManager.Instance.GamePrefs.GameType == Constants.GameType.BR)
        {
            end = base.EndCondition() || CheckPlayerLastStanding();
        }
        return end;
    }

    private void MovePlayableRegion()
    {
        ChangeMaxLife(-1);
        gameUI.DisplayBaseline();
        for (int i = 0; i < lines.Count; i++)
        {
            if (!lines[i].GetComponent<Word>().IsMoving)
            {
                lines[i].transform.localPosition = new Vector3(lines[i].transform.localPosition.x, lines[i].transform.localPosition.y + WordPrefab.GetComponent<RectTransform>().rect.height + WordsYOffset);
            }
        }
    }

    private bool CheckPlayerLastStanding()
    {
        foreach (Player player in loadedPlayers)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                Debug.Log(player.CustomProperties["PlayerState"].ToString());
                if (player.CustomProperties["PlayerState"].ToString() == "Game")
                {
                    return false;
                }
            }
        }
        return true;
    }

    protected override void CheckPlayerPoints()
    {
        if (GameManager.Instance.GamePrefs.GameType == Constants.GameType.BR)
        {
            BRCheckPlayerPoints();
        } else
        {
            base.CheckPlayerPoints();
        }
    }

    private void BRCheckPlayerPoints()
    {
        if (playerPoints >= 10000 & currentStage == 1)
        {
            WordsManager.Instance.ChangeWordLengths(new List<int>() { 3, 4, 5 });
            currentStage += 1;
            gameUI.OnStageComplete(currentStage);
        }
        else if (playerPoints >= 25000 & currentStage == 2)
        {
            WordsManager.Instance.ChangeWordLengths(new List<int>() { 4, 5, 6 });
            currentStage += 1;
            gameUI.OnStageComplete(currentStage);
        }
    }

    private void SyncScores()
    {
        PhotonController.Instance.UpdatePlayerState("Score", playerPoints);
        //PhotonController.Instance.UpdatePlayerState("Hacks", HacksManager.Instance.ActivatedHacks.Select(x => x.GetDescription()).ToArray());
    }

    protected override void EndGame()
    {
        StopAllCoroutines();
        CancelInvoke();
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
                if (PhotonNetwork.IsMasterClient && loadedPlayers.Count == PhotonNetwork.CurrentRoom.PlayerCount)
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        string score = targetPlayer.CustomProperties.GetValueOrDefault("Score", "").ToString();
        if (score != "")
        {
            playerScores[targetPlayer] = Int32.Parse(score);
        }
    }
}
