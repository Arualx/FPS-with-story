using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 5f;
    private Rigidbody rb;

    private GameObject player;
    private Vector3 playerPosition;

    private float distanceCatch = 20f;
    private float distanceHit = 3f;

    private Vector3 followVector;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = PlayerMovement.instance.gameObject;
    }

    private void FixedUpdate()
    {
        playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        float distance = Vector3.Distance(playerPosition, rb.position);

        if (distance <= distanceHit)
        {
            player.GetComponent<TakingDamage>().TakeDamage(50f);
            rb.linearVelocity = Vector3.zero;
        }
        else if (distance <= distanceCatch)
        {
            Vector3 direction = (playerPosition - rb.position);
            direction.y = 0;

            followVector = direction.normalized;
            rb.linearVelocity = new Vector3(followVector.x * speed, rb.linearVelocity.y, followVector.z * speed);


            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.MoveRotation(targetRotation);
            }
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }

    }

}
