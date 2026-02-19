using UnityEngine;

public class EnemyMovementPoint : MonoBehaviour
{
    private float rotationSpeed = 10f;
    [SerializeField] private Transform enemyPoint;

    private Vector3 previousPosition;
    private Vector3 currentDirection;
    

    private void Update()
    {
        transform.RotateAround(enemyPoint.position, transform.up, rotationSpeed * Time.deltaTime);

        transform.LookAt(enemyPoint);

    }
}
