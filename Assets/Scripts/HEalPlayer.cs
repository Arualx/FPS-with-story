using UnityEngine;

public class HEalPlayer : MonoBehaviour

{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMovement.instance.gameObject.GetComponent<TakingDamage>().Heal();
            Destroy(gameObject);
        }
    }

}
