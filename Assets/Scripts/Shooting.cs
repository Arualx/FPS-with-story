using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private MainData mainData;

    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject bullet;
    private Vector3 shootDirection;
    private bool automaticOn = false;

    private int currentBulletCount;
    private int currentMagazineBulletCount;
    private int maxBulletCount;
    private int maxMagazineBulletCount;

    private void Start()
    {
        maxBulletCount = mainData.maxBulletCount;
        maxMagazineBulletCount = mainData.maxMagazineBulletCount;
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && !PlayerMovement.instance.isRunning)
        {
            if (Physics.Raycast(PlayerMovement.instance.cameraPosition.transform.position, PlayerMovement.instance.cameraPosition.transform.forward, out RaycastHit target, 30f))
            {
                shootDirection = target.point;
            }
            else
            {
                shootDirection = PlayerMovement.instance.cameraPosition.transform.position + PlayerMovement.instance.cameraPosition.transform.forward * 30f;
            }

            shootDirection = (shootDirection - shootPosition.position).normalized;
            Instantiate(bullet, shootPosition.position, Quaternion.LookRotation(shootDirection));
        }

        /*if (Input.GetMouseButtonDown(0) && automaticOn && !isRunning && currentBulletCount > 0)
        {
            //InvokeRepeating(nameof(Shooting), 0f, 0.1f);
        }*/

    }
}
