using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TakingDamage : MonoBehaviour
{
    [SerializeField] private MainData mainData;

    

    [Header("Effects")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject deathPuddle;

    [Header("Sliders")]
    [SerializeField] private Slider healthFrontSlider;
    [SerializeField] private Slider healthBackSlider;
    

    private float currentHealth;
    private float maxHealth;
    private float healthDrainSpeed;
    private float healthDrainDelay;
    private Coroutine drainCoroutine;

    private void Start()
    {
        currentHealth = mainData.maxHealth;
        maxHealth = mainData.maxHealth;
        healthDrainDelay = mainData.healthDrainDelay;
        healthDrainSpeed = mainData.healthDrainSpeed;

        healthFrontSlider.maxValue = currentHealth;
        healthBackSlider.maxValue = currentHealth;

        healthFrontSlider.value = currentHealth;
        healthBackSlider.value = currentHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        healthFrontSlider.value = currentHealth;

        if (drainCoroutine == null) drainCoroutine = StartCoroutine(HealthDrain());

        if (currentHealth <= 0f)
        {
            Destroy(transform.parent.gameObject);
            Instantiate(deathEffect, transform.position, transform.rotation);
            GameObject Puddle = Instantiate(deathPuddle, transform.parent.transform.position, transform.rotation);
            Destroy(Puddle, 5f);
        }
    }

    private IEnumerator HealthDrain()
    {
        yield return new WaitForSeconds(healthDrainDelay);

        while (!Mathf.Approximately(healthBackSlider.value, currentHealth))
        {
            healthBackSlider.value = Mathf.MoveTowards(healthBackSlider.value, currentHealth, healthDrainSpeed * Time.deltaTime);
            yield return null;
        }
        
        healthBackSlider.value = currentHealth;
        drainCoroutine = null;
    }


}
