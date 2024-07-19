using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    private int areaTemp;
    private bool playThemes = true;

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
            if (sound.tag == null) { sound.source.tag = "Untagged"; }
            if (sound.tag != null) { sound.source.tag = sound.tag; }
        }
    }
    private void Update()
    {
        PlayTheme(playThemes);
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (!sound.source.isPlaying)
        {
            sound.source.Play();
        }
    }
    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound != null && sound.source.isPlaying) { sound.source.Stop(); }
    }

    public void PlayTheme(bool activate)
    {
        if (GameManager.Area != areaTemp)
        {
            StopByTag("Theme");
            areaTemp = GameManager.Area;
        }
        switch (areaTemp)
        {
            case 0: Play("MainTheme"); break;
            case 1: Play("DeathTheme"); break;
            case 2: Play("APTheme"); break;
        }
    }
    void StopByTag(string soundTag)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.tag == soundTag)
            {
                Stop(sound.name);
            }
        }
    }

    public void OnPlayerRun() { Play("Run"); }
    public void OnPlayerRunStop() { Stop("Run"); }
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
    public void OnPause() 
    {
        playThemes = false;
        StopByTag("Theme");
        Play("Pause");
    }
    public void OnResume() 
    {
        Play("Pause");
        playThemes = true;
    }
}
