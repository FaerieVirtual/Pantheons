using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public AudioSource source;

    public string name;

    public float volume;
    public float pitch;
    public bool loop;
    public string tag;
}
