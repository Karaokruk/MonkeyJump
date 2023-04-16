using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    /*** Public variables ***/
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    /*** Private variables ***/
    private Resolution[] resolutions;

    private void Start()
    {
        this.resolutions =
            Screen
                .resolutions
                .Select(resolution =>
                    new Resolution {
                        width = resolution.width,
                        height = resolution.height
                    })
                .Distinct()
                .ToArray();
        this.resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < this.resolutions.Length; i++)
        {
            string option =
                this.resolutions[i].width + "x" + this.resolutions[i].height;
            options.Add (option);

            if (
                this.resolutions[i].width == Screen.width &&
                this.resolutions[i].height == Screen.height
            )
            {
                currentResolutionIndex = i;
            }
        }

        this.resolutionDropdown.AddOptions(options);
        this.resolutionDropdown.value = currentResolutionIndex;
        this.resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        this.audioMixer.SetFloat("Volume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = this.resolutions[resolutionIndex];
        Screen
            .SetResolution(resolution.width,
            resolution.height,
            Screen.fullScreen);
    }
}
