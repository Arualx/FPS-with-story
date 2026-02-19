using UnityEngine;

public class EnemyMovementPath : MonoBehaviour
{
    private Rigidbody rb;
    private float enemySpeed = 10f;

    [SerializeField] private Transform startEnemyPosition;
    [SerializeField] private Transform endEnemyPosition;

    private Transform enemyTowards;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.position = startEnemyPosition.position;
        enemyTowards = endEnemyPosition;
    }


    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, startEnemyPosition.position) < 0.1f)
        {
            enemyTowards = endEnemyPosition;
        }
        else if (Vector3.Distance(transform.position, endEnemyPosition.position) < 0.1f)
        {
            enemyTowards = startEnemyPosition;
        }

        transform.LookAt(enemyTowards);
        rb.linearVelocity = transform.forward * enemySpeed;
    }
}
