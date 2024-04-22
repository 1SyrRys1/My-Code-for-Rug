using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System;
using UnityEngine;


public class AK47Fire : MonoBehaviour
{
    [SerializeField] private int ammo = 30;
    [SerializeField] private int magazine = 90;
    [SerializeField] private int currentAmmo;

    [Header("Звуки")]
    [SerializeField] public AudioSource _Source;
    [SerializeField] private AudioClip[] Clips;

    [Header("Звуки и эффекты")]
    [SerializeField] public Transform _cam;
    [SerializeField] public Transform _recoil_Object;
    [SerializeField] private GameObject impact;

    public CameraShaker _cameraShaker;
    //[SerializeField] private ParticleSystem gunShootEffect;

    [Header("Прочие элементы")]
    [SerializeField] private GameObject _player;
    [SerializeField] public Text infoAk;
    [SerializeField] public PlayerMovement player;
    [SerializeField] private Slider _healtBar;
    [SerializeField] private PickUpAk _Pickup;

    private float Time_Reload;
    public bool _ToShoot;
    public bool _ToPunch;

    public float damage = 35f;

    public static bool Punching;

    Vector3 StartRot;

    Vector3 StartPos;

    Vector3 CurrentPos;

    Vector3 CurrentRot;

    Vector3 CurrentRotSin;
    void Start()
    {
        _ToShoot = true;
        StartRot = _recoil_Object.transform.localRotation * new Vector3(1, 1, 1);

        StartPos = _Pickup._weaponPos.localPosition;


    }
    void Update()
    {
        if (PickUpAk.flag == true)
        {
            infoAk.text = $"{ammo} / {magazine}";
        }


        if (Time_Reload < 0)
        {
            Time_Reload -= Time.deltaTime;

            
        }

        RaycastHit hit;

        if (Input.GetMouseButton(0) && ammo > 0 && PickUpAk.flag == true && _ToShoot == true)
        {
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, 200))
            {
                hit.collider.gameObject.GetComponent<IDamageble>()?.TakeDamage(damage);

                _Source.PlayOneShot(Clips[0]);

                GameObject Impact = Instantiate(impact, hit.point - (Vector3.forward * 0.001f), Quaternion.LookRotation(hit.normal));
                Destroy(Impact, 10f);
            }
            _cameraShaker.ShakeCamera(1f, 10, 900);

            ShootPunch();
            ShootOutPut();
            ShootSinMove();

            currentAmmo += 1;
            ammo--;
            if (magazine - currentAmmo < 0)
            {
                currentAmmo--;
            }
            StartCoroutine(TimePunch());
            StartCoroutine(TimeShoot());
        }
        else
        {
            /*if(PickUpAk.flag == true )
                _Source.PlayOneShot(Clips[1]);*/
        }

        if (Input.GetKey(KeyCode.R) && ammo >= 0 && magazine > 0)
        {
            ammo = ammo + currentAmmo;
            magazine = magazine - currentAmmo;
            currentAmmo = 0;
        }
        _recoil_Object.localRotation = Quaternion.Lerp(_recoil_Object.localRotation, Quaternion.Euler(CurrentRot), Time.deltaTime * 10f);

        _recoil_Object.localRotation = Quaternion.Lerp(_recoil_Object.localRotation, Quaternion.Euler(CurrentRotSin), Time.deltaTime * 20f);
        if (Punching == true)
        {
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, new Vector3(0, CurrentPos.y, 0), Time.deltaTime * 5f);
        }
    }
    private void ShootOutPut()
    {
        if (_ToShoot == true)
        {
            float floatCam = 0;
            floatCam += -2;

            CurrentRot.x += floatCam * Time.deltaTime * 30f;
        }
    }
    private void ShootPunch()
    {
        if (_ToShoot == true)
        {
            float Punch = 0;
            Punch -= 7f;

            CurrentPos.y += Punch * Time.deltaTime * 5f;
            Punching = true;
        }
    }

    private void ShootSinMove()
    {
        float floatCamSin = 0;
        floatCamSin += -5;



        if (_ToPunch == true)
            CurrentRotSin.y -= Mathf.Sin(floatCamSin) * Time.deltaTime * 170f;
        else
            CurrentRotSin.y += Mathf.Sin(floatCamSin) * Time.deltaTime * 170f;
    }
    private IEnumerator TimeShoot()
    {
        _ToShoot = false;

        yield return new WaitForSeconds(0.1f);

        _ToShoot = true;
        CurrentPos = StartPos + new Vector3(0, 0.9f, 0);

        //CurrentRot = -CurrentRot * Time.deltaTime * 5f;
    }
    private IEnumerator TimePunch()
    {
        _ToPunch = false;

        yield return new WaitForSeconds(0.3f);

        _ToPunch = true;
        CurrentRotSin = -CurrentRotSin * Time.deltaTime * 5f;
    }
}
