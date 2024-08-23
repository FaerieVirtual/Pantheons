using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    private string themeQueued;
    private string themePlaying;
    public bool themePause = false;

    [Range(0, 1)] public float musicVolume;
    private List<Sound> music = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null) { Destroy(gameObject); }
        if (instance == null) { instance = this; }
    }
    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.name = sound.name;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
            sound.source.pitch = sound.pitch;
            //if (sound.tag == null) { sound.source.tag = "Untagged"; }
            //if (sound.tag != null) { sound.source.tag = sound.tag; }
        }
        SortByTag();
        
        themePlaying = "MainTheme";
    }
    private void Update()
    {
        CheckForPause();
        PlayTheme();
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (!sound.source.isPlaying)
        {
            if (sound.tag == "Theme")
            {
                Stop(tag: "Theme");
            }
            sound.source.Play();
        }
    }
    public void Stop(string name = null, string tag = null)
    {
        if (name != null)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == name);
            if (sound != null && sound.source.isPlaying) { sound.source.Stop(); }
        }
        if (tag != null)
        {
            foreach (Sound sound in sounds)
            { 
                if (sound.tag == tag && sound.source.isPlaying) { sound.source.Stop(); }
            }
        }
    }
    #region Options
    private void SortByTag()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.tag == "Theme" || sound.tag == "Music") { music.Add(sound); }
        }

    }

    public void UpdateSoundOptions() 
    {
        music.ForEach(sound => { sound.source.volume = sound.source.volume * musicVolume; });
    }
    #endregion
    #region Listener actions
    public void PlayTheme()
    {
        void ThemeQueue() 
        { 
            switch (SceneManager.GetActiveScene().buildIndex) 
            {
                case 0: themeQueued = "MainTheme"; break;
                case 1: case 2: themeQueued = "APTheme"; break;
            }
        }
        ThemeQueue();
        if (themePlaying != themeQueued || themePause == true)
        {
            Stop("Theme");
            themePlaying = themeQueued;
        }
        if (themePause == false) { Play(themeQueued); }   
    }
    public void CheckForPause()
    {
        GamePausedState paused = new GamePausedState(GameManager.instance.machine);
        if (GameManager.instance.machine.currentState == paused && themePause == false) { themePause = true; }
        if (GameManager.instance.machine.currentState != paused && themePause == true) { themePause = false; }
    }


    public void OnPlayerRun() { Play("Run"); }
    //public void OnPlayerRunStop() { Stop("Run"); }
    public void OnPlayerAttack() { Play("Attack"); }
    public void OnPlayerFall() { Play("Fall"); }
    public void OnPlayerFallStop() { Stop("Fall"); }
    public void OnPlayerJump() { Play("Jump"); }
    public void OnPlayerHit()
    {
        System.Random r = new();
        int option = r.Next(0, 3);
        switch (option)
        {
            case 0: Play("Hurt1"); break;
            case 1: Play("Hurt2"); break;
            case 3: Play("Hurt3"); break;
        }
    }
    public void OnPlayerDeath() { Play("Death"); }
    //public void OnPause()
    //{
    //    playThemes = false;
    //    StopByTag("Theme");
    //    Play("Pause");
    //}
    //public void OnResume()
    //{
    //    Play("Pause");
    //    playThemes = true;
    //}
    #endregion
}
