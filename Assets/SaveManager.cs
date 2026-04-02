using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    public resource resourceManager;
    public shop shopManager;
    public TutorialPopup tutorialPopup;
    public AudioSource welcomeAudioSource;
    public AudioClip welcomeSoundClip;
    public ParticleSystem welcomeParticles;

    void Start()
    {
        LoadGame();
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) SaveGame();
    }

    void SaveGame()
    {
        PlayerPrefs.SetFloat("coins", resourceManager.coins);
        PlayerPrefs.SetFloat("rate", resourceManager.rate);
        PlayerPrefs.SetFloat("rsrc2", resourceManager.rsrc2);
        PlayerPrefs.SetFloat("rate2", resourceManager.rate2);
        PlayerPrefs.SetString("lastPlayTime", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    void LoadGame()
    {
        if (!PlayerPrefs.HasKey("lastPlayTime")) return;

        // Restore values
        resourceManager.coins = PlayerPrefs.GetFloat("coins", 0f);
        resourceManager.rate = PlayerPrefs.GetFloat("rate", 0.5f);
        resourceManager.rsrc2 = PlayerPrefs.GetFloat("rsrc2", 0f);
        resourceManager.rate2 = PlayerPrefs.GetFloat("rate2", 0f);

        // Calculate offline earnings
        long binary = Convert.ToInt64(PlayerPrefs.GetString("lastPlayTime"));
        DateTime lastTime = DateTime.FromBinary(binary);
        float secondsAway = (float)(DateTime.Now - lastTime).TotalSeconds;

        // Euler step for time away
        float offlineCoins = resourceManager.rate * secondsAway;
        float offlineRsrc2 = resourceManager.rate2 * secondsAway;
        resourceManager.coins += offlineCoins;
        resourceManager.rsrc2 += offlineRsrc2;

        // Welcome back juice
        if (offlineCoins > 0f)
        {
            if (welcomeAudioSource != null && welcomeSoundClip != null)
                welcomeAudioSource.PlayOneShot(welcomeSoundClip);

            if (welcomeParticles != null)
                welcomeParticles.Play();

            if (tutorialPopup != null)
                tutorialPopup.ShowPopup("Welcome back!\nYou earned " + offlineCoins.ToString("F0") + " Scrap while away!");
        }
    }

    // Call this from prestige or if you need to wipe saves
    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }
}