using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Mouse_ook _mouse_ook;
    [SerializeField] private GameObject _camera;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject _playerModel;
    // Start is called before the first frame update
    void Start()
    {

        if(!_photonView.IsMine)
        {
            _photonView.enabled = false;
            _playerMovement.enabled = false;
            _mouse_ook.enabled = false;
            _camera.SetActive(false);
            _playerModel.SetActive(true);
        }
    }
}
