using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    private ParticleSystem particle;
    private Color particleColor;

    private float hitMove = 0.8f;
    private Rigidbody rb;
    

    private float bulletSpeed = 20f;
    private float bulletLifeTime = 5f;

    private bool onEnemy = false;


    void Awake()
    {
        if (this.gameObject.CompareTag("Enemy")) onEnemy = true;
        rb = GetComponent<Rigidbody>();
        particle = hitEffect.GetComponent<ParticleSystem>();
    }

    void Start()
    {
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        Invoke(nameof(DestroyBullet), bulletLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            return;
        } 
        else if (other.gameObject.CompareTag("Enemy") && !onEnemy || other.gameObject.CompareTag("Player") && onEnemy)
        {
            other.gameObject.GetComponent<TakingDamage>().TakeDamage(Random.Range(10, 40));
        }

        //color based on hit object
        particleColor = other.gameObject.GetComponent<Renderer>().material.color;
        var main = particle.main;
        main.startColor = new ParticleSystem.MinMaxGradient(particleColor);

        //hit effect
        Instantiate(hitEffect, transform.position - transform.forward * hitMove, transform.rotation);
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
