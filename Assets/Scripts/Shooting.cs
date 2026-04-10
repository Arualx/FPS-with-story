using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private MainData mainData;

    [SerializeField] private GameObject bullet;
    private Vector3 shootDirection;
    [SerializeField] private bool automaticOn = false;

    [SerializeField] private int currentBulletCount  = 30;
    private int currentMagazineBulletCount;
    private int maxBulletCount;
    private int maxMagazineBulletCount;

    private bool StillShooting = false;
    [SerializeField] private float downTimeLenght = 0.1f;

    [SerializeField] private GameObject AmmoLeftInMagazine;
    [SerializeField] private GameObject Ammo;

    [SerializeField] private List<Transform> ShootPoints = new List<Transform>();
    [SerializeField] private int SelectedWepon;
    [SerializeField] private List<GameObject> Wepon = new List<GameObject>();

    private void Start()
    {
        maxBulletCount = mainData.maxBulletCount;
        maxMagazineBulletCount = mainData.maxMagazineBulletCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectedWepon = 0; Wepon[0].SetActive(true); Wepon[1].SetActive(false); Wepon[2].SetActive(false); automaticOn = false; }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectedWepon = 1; Wepon[0].SetActive(false); Wepon[1].SetActive(true); Wepon[2].SetActive(false); automaticOn = true; }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectedWepon = 2; Wepon[0].SetActive(false); Wepon[1].SetActive(false); Wepon[2].SetActive(true); automaticOn = false; }
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && !PlayerMovement.instance.isRunning)
        {
            if (!automaticOn) { ManualShoot(); }
            else if (automaticOn && currentBulletCount > 0) 
            { 
                Debug.Log("Blackboard"); 
                StartCoroutine(AutoShoot()); 
                StillShooting = true;  
            }
        } 
        if (Input.GetMouseButtonUp(0) || PlayerMovement.instance.isRunning) { StillShooting = false; }
    }

    private void ManualShoot(){
        if (Physics.Raycast(PlayerMovement.instance.cameraPosition.transform.position, PlayerMovement.instance.cameraPosition.transform.forward, out RaycastHit target, 30f))
        {
            shootDirection = target.point;
        }
        else
        {
            shootDirection = PlayerMovement.instance.cameraPosition.transform.position + PlayerMovement.instance.cameraPosition.transform.forward * 30f;
        }

        shootDirection = (shootDirection - ShootPoints[SelectedWepon].position).normalized;
        Instantiate(bullet, ShootPoints[SelectedWepon].position, Quaternion.LookRotation(shootDirection));
    }
    private IEnumerator AutoShoot()
    {
        //Getting Direction
        if (Physics.Raycast(PlayerMovement.instance.cameraPosition.transform.position, PlayerMovement.instance.cameraPosition.transform.forward, out RaycastHit target, 30f))
        {
            shootDirection = target.point;
        }
        else
        {
            shootDirection = PlayerMovement.instance.cameraPosition.transform.position + PlayerMovement.instance.cameraPosition.transform.forward * 30f;
        }
        //Calculate projectile direction
        shootDirection = (shootDirection - ShootPoints[SelectedWepon].position).normalized;

        //shoot
        currentBulletCount -= 1;
        AmmoLeftInMagazine.GetComponent<TextMeshProUGUI>().text = currentBulletCount.ToString();
        Instantiate(bullet, ShootPoints[SelectedWepon].position, Quaternion.LookRotation(shootDirection));

        yield return new WaitForSeconds(downTimeLenght);

        if (StillShooting && currentBulletCount > 0) StartCoroutine(AutoShoot());
        else if(StillShooting && currentBulletCount == 0) StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        yield return new WaitForSeconds(1);
        if (maxBulletCount > maxMagazineBulletCount)
        {
            maxBulletCount -= maxMagazineBulletCount;
            currentBulletCount = maxMagazineBulletCount;
        }
        else
        { 
            currentBulletCount = maxBulletCount;
            maxBulletCount = 0;
        }
        


        //Displays
        AmmoLeftInMagazine.GetComponent<TextMeshProUGUI>().text = currentBulletCount.ToString();
        Ammo.GetComponent<TextMeshProUGUI>().text = maxBulletCount.ToString();
        //ContinueShooting
        if (StillShooting) { StartCoroutine(AutoShoot()); }
    }

}
