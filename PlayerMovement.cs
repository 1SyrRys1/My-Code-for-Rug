using System.Collections;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks, IDamageble
{
    public CharacterController controller;

    public Transform weapon_pos;

    [SerializeField] private Camera gameCamera;

    [SerializeField] private GameObject _cameraHolder;

    [SerializeField] private AudioClip walkSong;

    [SerializeField] private AudioSource sourceCharacter;

    [SerializeField] private Animator _animatorControler;

    [SerializeField] private float _posAmount = 0.009f;

    [SerializeField] private float _posSmooth = 100f;

    public float speed = 3f;

    [SerializeField] private float gravity = -20f;

    public Transform groundCheck;

    public LayerMask groundMask;

    Vector3 velocity;

    bool _walkAllow;

    PhotonView PV;

    public bool isDeath;

    public static float _amountChange = 10;

    float Krujka;

    public bool isWalking;

    // Update is called once per frame

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        _animatorControler.SetBool("isDeath", isDeath);

        if (isDeath == false)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            _animatorControler.SetFloat("Blend", speed);
            
            //Передвижение персонажа
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            _cameraHolder.transform.localPosition = new Vector3(_cameraHolder.transform.localPosition.x, Krujka, _cameraHolder.transform.localPosition.z);

            if (controller.transform.position.magnitude > 1f && isWalking == true && !_walkAllow)
            {
                sourceCharacter.PlayOneShot(walkSong);
                StartCoroutine(SHAGINAVERNO());
            }
            HeadPlayAnim(HeadPos());
        }
    }
    Vector3 HeadPos()
    {
        Vector3 pos = Vector3.zero;

        if (controller.transform.position.magnitude > 1f && isWalking == true)
        {
            pos.y += Mathf.Lerp(pos.y, Mathf.Cos(Time.time * _amountChange) * _posAmount * 1.3f, _posSmooth * Time.deltaTime);
        }
        return pos;
    }

    private void HeadPlayAnim(Vector3 pos)
    {
        Krujka += pos.y;
    }

    private IEnumerator SHAGINAVERNO()
    {
        _walkAllow = true;

        yield return new WaitForSeconds(0.8f);

        _walkAllow = false;
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, 35);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (PV.IsMine)
            return;

        Debug.Log("took damage: " + damage);
    }

}
