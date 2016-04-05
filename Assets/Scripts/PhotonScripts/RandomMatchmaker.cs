using UnityEngine;

public class RandomMatchmaker : Photon.PunBehaviour
{
    private PhotonView myPhotonView;

    // Use this for initialization
    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnPhotonRandomJoinFailed()
    {
		Debug.Log("Can't join random room!"); 
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
		GameObject car = PhotonNetwork.Instantiate("Cars/Car Root", (Vector3.forward * Random.Range(0,10)) + (Vector3.left * Random.Range(0,10)), Quaternion.identity, 0);
		car.GetComponent<SimpleCarController>().IsControllable = true;
        myPhotonView = car.GetComponent<PhotonView>();
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
        {
            bool shoutMarco = GameLogic.playerWhoIsIt == PhotonNetwork.player.ID;

            if (shoutMarco && GUILayout.Button("Marco!"))
            {
                myPhotonView.RPC("Marco", PhotonTargets.All);
            }
            if (!shoutMarco && GUILayout.Button("Polo!"))
            {
                myPhotonView.RPC("Polo", PhotonTargets.All);
            }
        }
    }
}
