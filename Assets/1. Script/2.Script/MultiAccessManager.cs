using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
// MonoBehaviousPunCallbacks 상속 받으려면 필요한 두 모듈
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiAccessManager : MonoBehaviourPunCallbacks
{

    public GameObject photonObject;

    public Text Status_Text;
    public Text Server_Text;
    public GameObject Sphere;
    public Button[] button_Text = new Button[6];
    public int click;
    public Text room0_Text;
    public Text room1_Text;
    public Text room2_Text;
    public Text room3_Text;
    public Text room4_Text;
    public Text room5_Text;

    public InputField InputField_Info;
    public string NickName;

    // 이거는 텍스트 + 동그라미 색깔로. 위치는 UserInfo 옆에
    // public InputField roomInput, NickNameInput;
    // 방 이름 입력받는 건 클릭했을 때 text로 소원이 씬에 넘기기로 했으니까 없애도 될 것 같고 + NickName은 UserInfo 받으니까 얘도 소원이 씬에 넘기면서 없앨까?
    // 근데 정보로 이 방 플레이어 몇 명인지 막 이런 거 하려면 필요하긴 하거든? 우선 살려두자.

    void Awake()
    {
        Screen.SetResolution(1080, 720, false);
        // 이게 첫 번째 씬이 아닌데도 괜찮은가? 우선 1080*720 => 내일 다시 이야기해보기
        // Sphere = GameObject.Find("Server_Sphere");
        Connect();
        // JoinLobby();
    }

    void Update()
    {
        Status_Text.text = PhotonNetwork.NetworkClientState.ToString();
        if (Status_Text.text == "ConnectedToMasterServer")
        {
            Server_Text.text = "서버접속완료";
        }
    }
    // 서버 연결되어 있는지 여부인데 아 그냥 텍스트로 할까
    // 저 상태text 대신에 컬러를 바꿀 수 있으면 좋을텐데 아직 거기까지 잘 모르겠어

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        // Sphere.GetComponent<Outline>().effectColor = Color.green;
    }
    // Connect함수 호출하면 서버에 접속. 그니까 로비 창에 들어가면 바로 Connect 함수 호출해야 해.

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
        //Server_Text.text = ("로비접속완료");
    }

    public void JoinOrCreateRoom0()
    {
        PhotonNetwork.JoinOrCreateRoom(room0_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        print("0번방");
        SceneManager.LoadScene("ClassScene");
    }

    public void JoinOrCreateRoom1()
    {
        PhotonNetwork.JoinOrCreateRoom(room1_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        print("1번방");
        SceneManager.LoadScene("ClassScene");
    }
    public void JoinOrCreateRoom2()
    {
        PhotonNetwork.JoinOrCreateRoom(room2_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        print("2번방");
        SceneManager.LoadScene("ClassScene");
    }
    public void JoinOrCreateRoom3()
    {
        PhotonNetwork.JoinOrCreateRoom(room3_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        print("3번방");
        SceneManager.LoadScene("ClassScene");
    }
    public void JoinOrCreateRoom4()
    {
        PhotonNetwork.JoinOrCreateRoom(room4_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        print("4번방");
        SceneManager.LoadScene("ClassScene");
    }
    public void JoinOrCreateRoom5()
    {
        PhotonNetwork.JoinOrCreateRoom(room5_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        print("5번방");       
        SceneManager.LoadScene("ClassScene");
    }

    /*
    public void JoinOrCreateRoom()
    {
        if (button_Text[click] == button_Text[0])
        {
            PhotonNetwork.JoinOrCreateRoom(room0_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
            print("0번방");
        }
        else if (button_Text[click] == button_Text[1])
        {
            PhotonNetwork.JoinOrCreateRoom(room1_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
            print("1번방");
        }
        else if (button_Text[click] == button_Text[2])
        {
            PhotonNetwork.JoinOrCreateRoom(room2_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        }
        else if (button_Text[click] == button_Text[3])
        {
            PhotonNetwork.JoinOrCreateRoom(room3_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        }
        else if (button_Text[click] == button_Text[4])
        {
            PhotonNetwork.JoinOrCreateRoom(room4_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        }
        else if (button_Text[click] == button_Text[5])
        {
            PhotonNetwork.JoinOrCreateRoom(room5_Text.text, new RoomOptions { MaxPlayers = 20 }, null);
        } 
}*/

    public override void OnConnected()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        Server_Text.text = ("방만들기완료");
        Info();
    }

    public override void OnJoinedRoom()
    {
        Server_Text.text = ("방참가완료");
        Info();
    }   

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    // 이건 소원이 씬으로 넘겨야 해


    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
}