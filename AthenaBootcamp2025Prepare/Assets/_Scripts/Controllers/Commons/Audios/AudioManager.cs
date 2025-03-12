using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private static AudioManager instance;
    [SerializeField] private List<AudioClip> audioClipList = new();
    [SerializeField] private List<AudioSource> audioSourceList = new();
    [SerializeField] private List<string> audioNameList = new();

    public static AudioManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadAudioList();
    }

    private void LoadAudioList()
    {
        audioNameList.Clear();
        List<AudioSource> audioSources = GetComponents<AudioSource>().ToList();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
        }
        for (int i = 0; i < audioClipList.Count; i++)
        {
            if (i < audioSourceList.Count)
            {
                Debug.Log(i);
                Debug.Log(audioClipList[i]);
                Debug.Log(audioSourceList[i]);
                audioSourceList[i].clip = audioClipList[i];
                audioNameList.Add(audioClipList[i].name);
            }
            else
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = audioClipList[i];
                audioSourceList.Add(audioSource);
                audioNameList.Add(audioClipList[i].name);
            }
        }
    }

    public void PlayAudio(string audioName)
    {
        try
        {
            //TO DO: Write function to ensure audio is ready before playing
            audioSourceList[audioNameList.IndexOf(audioName)].Play();
        }
        catch { }
    }

    public void StopAudio(string audioName)
    {
        try
        {
            audioSourceList[audioNameList.IndexOf(audioName)].Stop();
        }
        catch { }
    }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
}
