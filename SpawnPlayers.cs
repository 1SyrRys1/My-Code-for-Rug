using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 PositionPlayers = transform.position;
        PhotonNetwork.Instantiate(player.name, PositionPlayers, Quaternion.identity);
    }
}
