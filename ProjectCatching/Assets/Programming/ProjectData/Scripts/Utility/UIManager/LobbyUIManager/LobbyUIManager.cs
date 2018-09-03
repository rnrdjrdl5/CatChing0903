using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour {

    public const int MAXPLAYER = 6;

    public float UIFadeTime = 0.2f;

    static private LobbyUIManager lobbyUIManager;
    static public LobbyUIManager GetInstance() {
        if (lobbyUIManager == null)
            return null;

        return lobbyUIManager;
    }

    public delegate void DeleUpdate();
    public event DeleUpdate UpdateEvent;

    public GameObject UICanvas { get; set; }


    public LobbyPanelScript lobbyPanelScript;
    public RoomPanelScript roomPanelScript;
    public WaitingRoomPanelScript waitingRoomPanelScript;

    private void Awake()
    {
        if (lobbyUIManager == null)
            lobbyUIManager = this;

        UICanvas = GameObject.Find("UICanvas").gameObject;

        InitScripts();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (UpdateEvent != null) { UpdateEvent(); }
    }

    void InitScripts()
    {

        lobbyPanelScript = new LobbyPanelScript();
        lobbyPanelScript.InitData();

        roomPanelScript = new RoomPanelScript();
        roomPanelScript.InitData();

        waitingRoomPanelScript = new WaitingRoomPanelScript();
        waitingRoomPanelScript.InitData();
    }
   
    
}
