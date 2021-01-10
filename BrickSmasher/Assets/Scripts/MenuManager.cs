using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TextMeshProUGUI highScoreText;
    AudioSource menuButton_sound;

    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i=0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
            resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        int highScore = PlayerPrefs.GetInt("HIGHSCORE");
        highScoreText.text = "High Score: " + highScore;
        var aSources = GetComponents<AudioSource>();
        menuButton_sound = aSources[0];        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playGame()
    {
        menuButton_sound.Play();
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("You quit the game");
    }

    public void buttonSound()
    {
        menuButton_sound.Play();
    }

    public void SetScreenResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution (resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
