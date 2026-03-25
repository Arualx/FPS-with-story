using UnityEngine;

[CreateAssetMenu(fileName = "MainData", menuName = "ScriptableObjects/MainData")]
public class MainData : ScriptableObject
{
    public float maxHealth = 100f;
    public float healthDrainSpeed = 100f;
    public float healthDrainDelay = 0.3f;

    public float playerWalkingSpeed = 10f;
    public float playerRunningSpeed = 20f;

    public float jumpStrenght = 5f;

    public int maxBulletCount = 100;
    public int maxMagazineBulletCount = 30;

    public float gravitation = 1f;

}
