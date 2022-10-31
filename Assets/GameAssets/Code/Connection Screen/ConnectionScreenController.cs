using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionScreenController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private TMP_InputField nicknameInput;

    [SerializeField] private GameObject selector;

    [SerializeField] private float yOffset;

    public void CreateRoom()
    {
        Debug.Log(createInput.text);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PublishUserId = true;
        
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        createButton.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined");
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    //public override void OnCreatedRoom()
    //{
    //    Debug.Log("created");

    //    PhotonNetwork.LoadLevel("LobbyScene");
    //}


    public void OnHoverEnter(GameObject obj)
    {
        SoundEffectsManager.Instance.PlayOneShotSFX("Hover");
        selector.SetActive(true);

        selector.transform.localPosition = new Vector3(selector.transform.localPosition.x, obj.transform.localPosition.y + yOffset);
    }

    public void OnHoverExit(GameObject obj)
    {
        selector.SetActive(false);
    }

    //private void OnEnable()
    //{
    //    createButton.onClick.AddListener(CreateRoom);
    //    joinButton.onClick.AddListener(JoinRoom);

    //    selector.SetActive(false);
    //}

    //private void OnDisable()
    //{
    //    createButton.onClick.RemoveListener(CreateRoom);
    //    joinButton.onClick.RemoveListener(JoinRoom);

    //    selector.SetActive(false);
    //}
}
