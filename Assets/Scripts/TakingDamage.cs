using UnityEngine;
using UnityEngine.UI;

public class TakingDamage : MonoBehaviour
{
    private float health = 100f;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject deathPuddle;

    [SerializeField] private GameObject healthSlider;

    private void Awake()
    {
        healthSlider.GetComponent<Slider>().maxValue = health;
    }

    private void Update()
    {
        healthSlider.GetComponent<Slider>().value = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(transform.parent.gameObject);
            Instantiate(deathEffect, transform.position, transform.rotation);
            GameObject Puddle = Instantiate(deathPuddle, transform.parent.transform.position, transform.rotation);
            Destroy(Puddle, 5f);
            

        }
    }
}
