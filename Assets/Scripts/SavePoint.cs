using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    public string name;
    public KeyCode tipkaReset = KeyCode.R;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name) == name)
            {
                PlayerMovement.instance.transform.position = transform.position;
                PlayerMovement.instance.transform.rotation = transform.rotation;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(tipkaReset))
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name, "");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name, name);
            Debug.Log(name);
        }
    }
}
