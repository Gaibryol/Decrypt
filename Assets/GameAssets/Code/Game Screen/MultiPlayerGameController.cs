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
    [SerializeField] private GameObject BattleRoyalButtons;
    [SerializeField] private int numberHackActivations;

    protected override void Start()
    {
        base.Start();
        gameUI = GetComponent<GameUIController>();

        InitPhotonState();
        playerScores = new Dictionary<Player, int>();

        if (PhotonController.Instance.IsMaster)
        {
            StartCoroutine(NotifyGameStart());
        }
    }


    public override void OnEnable()
    {
        base.OnEnable();
        PhotonController.Instance.AddCallbackTarget(this);

    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonController.Instance.RemoveCallbackTarget(this);

    }

    protected override void Update()
    {
        base.Update();
    }

    public override void UpdatePlayerPoints(float points)
    {
        base.UpdatePlayerPoints(points);
        SyncScores();
    }

    private IEnumerator NotifyGameStart()
    {
        yield return new WaitUntil(() => PhotonController.Instance.AllPlayersInState("GameWaiting"));

        // Here if all players are GameWaiting
        while (!PhotonController.Instance.AllPlayersInState("Game"))
        {
            SendGameStartEvent();
            yield return new WaitForSeconds(1f);
        }
    }

    private void SendGameStartEvent()
    {
        PhotonController.Instance.SendPhotonEvent(Constants.GameStartEventCode, PhotonController.Instance.Time, ReceiverGroup.All);
    }

    private void InitPhotonState()
    {
        loadedPlayers.Clear();
        // Update player state to be in game. Updates player custom property.
        PhotonController.Instance.UpdatePlayerState("PlayerState", "GameWaiting");

        // Clients automatically sync with master. Conventional method of ChangeState doesn't work
        GameManager.Instance.GameState = Constants.GameStates.Game;

        // Stop syncing scenes and set next scene to room scene. Allows navigation to room scene without sync.
        PhotonController.Instance.AutoSyncScene = false;

        //SendPhotonEvent(Constants.PlayerReadyEventCode, PhotonNetwork.LocalPlayer, ReceiverGroup.All);

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
        foreach (Player player in PhotonController.Instance.Players)
        {
            if (player != PhotonController.Instance.Me)
            {
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
            numberHackActivations += 1;
            gameUI.OnStageComplete(currentStage);
        }
        else if (playerPoints >= 25000 & currentStage == 2)
        {
            WordsManager.Instance.ChangeWordLengths(new List<int>() { 4, 5, 6 });
            currentStage += 1;
            numberHackActivations += 1;
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
        if (photonEvent.Code == Constants.GameStartEventCode && subState == Constants.SubState.Loading)
        {
            StartGame();
            PhotonController.Instance.UpdatePlayerState("PlayerState", "Game");
            PhotonController.Instance.SetNextScene("EndScene");
        } else if (photonEvent.Code == Constants.HackSelectedEventCode)
        {
            if (PhotonController.Instance.GetPlayer(photonEvent.Sender) != PhotonController.Instance.Me)
            {
                HacksManager.Instance.AddHack(photonEvent.CustomData.ToString());
            }
            Debug.Log(photonEvent.CustomData);
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
