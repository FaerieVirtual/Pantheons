using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public List<Sound> playingSFX;
    public static AudioManager Instance;

    private bool musicActive = true;
    private bool sfxActive = true;

    [Range(0, 1)] public float musicVolume;
    [Range(0, 1)] public float sfxVolume;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null) { Destroy(gameObject); }
        if (Instance == null) { Instance = this; }
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
        }
    }
    private void Update()
    {
        MusicPlayer(musicActive);
        SfxPlayer(sfxActive);
    }

    public void Play(string name, float volume, float delay = 0)
    {
        Sound sound = Array.Find(sounds, sound => sound.name.ToLower() == name.ToLower());
        if (sound == null || sound.source.isPlaying) return;

        sound.source.volume *= volume;

        if (delay == 0) { sound.source.Play(); }
        else { sound.source.PlayDelayed(delay); }
    }
    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name.ToLower() == name.ToLower());
        if (sound != null && sound.source.isPlaying) sound.source.Stop();
    }

    public void MusicPlayer(bool active)
    {
        var musicMap = new Dictionary<string, string>
        {
            { "Ancient Path", "At the beginning" },
            { "WeaponItem's Gully", "Ancestors' bated breath"},
            { "Hangman's Grove", "The sound of Regret" },
            { "Verdant Grottos", "Greener grass" }
        };

        if (!active)
        {
            foreach (var entry in musicMap) Stop(entry.Key);
            return;
        }

        foreach (var entry in musicMap)
        {
            Sound sound = Array.Find(sounds, sound => sound.name.ToLower() == entry.Key.ToLower());

            if (!GameManager.Instance.Area.ToLower().Contains(entry.Key) && sound.source.isPlaying) Stop(entry.Key);
            if (GameManager.Instance.Area.ToLower().Contains(entry.Key.ToLower()) && !sound.source.isPlaying) Play(entry.Value, musicVolume, 2);
        }
    }

    public void SfxPlayer(bool active, string sfx = null, bool play = false)
    {
        void HandleSound(string sfx, bool play) 
        {
            Sound sound = Array.Find(sounds, sound => sound.name.ToLower() == sfx.ToLower());
            if (sound == null) 
            {
                Console.WriteLine($"Error: Could not find sound {sfx}");
                return;
            }

            if (!sound.source.isPlaying && play) 
            { 
                Play(sfx, sfxVolume);
                playingSFX.Add(sound);
            }
            if (sound.source.isPlaying && !play) 
            { 
                Stop(sfx); 
                playingSFX.Remove(sound);
            }
        }

        if (!active) 
        {
            foreach (Sound sound in playingSFX) 
            { 
                Stop(sound.name);
                playingSFX.Remove(sound);
            }
            return;
        }

        if (sfx != null) HandleSound(sfx, play); 
    }
    #region Options
    public void UpdateSoundOptions(float musicVolumePref, float sfxVolumePref)
    {
        musicVolume = musicVolumePref;
        if (musicVolume == 0) musicActive = false;
        sfxVolume = sfxVolumePref;
        if (sfxVolume == 0) sfxActive = false;
    }
    #endregion
    #region Listener actions
    //public void MusicPlayer()
    //{
    //    void ThemeQueue() 
    //    { 
    //        switch (SceneManager.GetActiveScene().buildIndex) 
    //        {
    //            case 0: themeQueued = "MainTheme"; break;
    //            case 1: case 2: themeQueued = "APTheme"; break;
    //        }
    //    }
    //    ThemeQueue();
    //    if (themePlaying != themeQueued || themePause == true)
    //    {
    //        Stop("Theme");
    //        themePlaying = themeQueued;
    //    }
    //    if (themePause == false) { Play(themeQueued); }   
    //}
    //public void CheckForPause()
    //{
    //    GamePausedState paused = new GamePausedState(GameManager.Instance.machine);
    //    if (GameManager.Instance.machine.currentState == paused && themePause == false) { themePause = true; }
    //    if (GameManager.Instance.machine.currentState != paused && themePause == true) { themePause = false; }
    //}


    public void OnPlayerRun() { Play("Run", sfxVolume); }
    public void OnPlayerRunStop() { Stop("Run"); }
    public void OnPlayerAttack() { Play("Attack", sfxVolume); }
    public void OnPlayerFall() { Play("Fall", sfxVolume); }
    public void OnPlayerFallStop() { Stop("Fall"); }
    public void OnPlayerJump() { Play("Jump", sfxVolume); }
    public void OnPlayerHit()
    {
        System.Random r = new();
        int option = r.Next(0, 3);
        switch (option)
        {
            case 0: Play("Hurt1", sfxVolume); break;
            case 1: Play("Hurt2", sfxVolume); break;
            case 2: Play("Hurt3", sfxVolume); break;
        }
    }
    public void OnPlayerDeath() { Play("Death", sfxVolume); }
    //public void OnPause()
    //{
    //    musicActive = false;
    //    StopByTag("Theme");
    //    Play("Pause");
    //}
    //public void OnResume()
    //{
    //    Play("Pause");
    //    musicActive = true;
    //}
    public void OnPause()
    {
        //musicActive = false;
        //StopByTag("Theme");
        foreach (Sound sound in sounds)
        {
            if (sound.source.isPlaying) sound.source.Pause();
        }
        Play("PauseSFX", sfxVolume);
        Play("Pause", musicVolume);

    }
    public void OnResume()
    {
        Play("ResumeSFX", sfxVolume);
        foreach (Sound sound in sounds)
        {
            sound.source.UnPause();
        }
        //musicActive = true;
    }
    #endregion
}
