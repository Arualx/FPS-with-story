using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{

    private GameObject StartDropdown;
    private GameObject SettingsDropdown;

    public void Awake()
    {
        StartDropdown = GameObject.FindWithTag("StartDropdown");
        StartDropdown.SetActive(false);

        SettingsDropdown = GameObject.FindWithTag("SettingsDropdown");
        SettingsDropdown.SetActive(false);
    }


    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
            Application.Quit();
    }

    public void OpenStart()
    {
        StartDropdown.SetActive(!StartDropdown.activeSelf);
    }

    public void OpenSettings()
    {
        SettingsDropdown.SetActive(!SettingsDropdown.activeSelf);
    }

}
