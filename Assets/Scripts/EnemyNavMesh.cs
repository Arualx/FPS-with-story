using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public static EnemyNavMesh instance;
    private NavMeshAgent agent;
    private Vector3 playerPosition;
    private bool Catch;
    private float distanceCatch = 15f;
    private float distanceStop = 20f;
    private float distanceClose = 2f;

    private Vector3 startPosition;
    private float timeStop = 7f;


    
    public GameObject bullet;
    public Transform shootPosition;
    private float minShoot = 0.1f;
    private float maxShoot = 1f;
    private float timeShoot;

    public Animator anim;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //make it as a posiiton in world and instaniate the player in world on that position instead of this
        startPosition = transform.position;
        anim.SetBool("Walking", false);

    }

    private void Update()
    {

        playerPosition = PlayerMovement.instance.transform.position;
        Shooting();

        if (agent.velocity.magnitude > 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

    }

    private void Shooting()
    {
        if (!Catch)
        {
            if (Vector3.Distance(transform.position, playerPosition) < distanceCatch)
            {
                Catch = true;
                timeShoot = 1f;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, playerPosition) > distanceClose)
            {
                agent.destination = playerPosition;
            }
            else
            {
                agent.destination = transform.position;
            }
        }

        if (Vector3.Distance(transform.position, playerPosition) > distanceStop)
        {
            Catch = false;
            Invoke(nameof(BackToStart), timeStop);

        }

        timeShoot -= Time.deltaTime;
        if (timeShoot < minShoot)
        {
            timeShoot = Random.Range(minShoot, maxShoot);
            shootPosition.LookAt(playerPosition);
            Vector3 direction = playerPosition - transform.position;
            float angle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);
            if (Mathf.Abs(angle) <= 30)
            {
                Instantiate(bullet, shootPosition.position, shootPosition.rotation);
            }

        }
    }

    private void BackToStart()
    {
        //he has problems with going to the position and then registring the player again
        agent.destination = startPosition;
    }

}
