using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject[] CardPrefabs;
    public AudioSource effectMaker;
    AudioSource musicMaker;
    AudioSource ambianceMaker;

    public static GameManager gm;
    public bool visibleMouse = true;

    [Range(0f, 1f)]
    public float masterVolume = 0.5f;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float effectsVolume = 0.5f;

    [SerializeField]
    TextAsset maleNameFile;
    [SerializeField]
    TextAsset femaleNameFile;
    [SerializeField]
    TextAsset lastNameFile;

    [HideInInspector]
    public string[] maleNames;
    [HideInInspector]
    public string[] femaleNames;
    [HideInInspector]
    public string[] lastNames;

    public bool playGuardAnnouncement = false;

    void Awake()
  {
    if (gm == null)
    {
      gm = this.GetComponent<GameManager>();
      DontDestroyOnLoad(this);
    }

    else
    {
      // GameManager.gm.Start();
      Destroy(this.gameObject);
      return;
    }

    CardPrefabs = Resources.LoadAll<GameObject>("Cards");
  }

    void Start()
    {
        maleNames = ParseFile(maleNameFile);
        femaleNames = ParseFile(femaleNameFile);
        lastNames = ParseFile(lastNameFile);
        musicMaker = this.GetComponent<AudioSource>();
        ambianceMaker = Camera.main.GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateVolumes();

        if (playGuardAnnouncement)
        {
            playGuardAnnouncement = false;
            PlaySound(SoundEffects.guardAnnouncement);
        }
    }

    private string[] ParseFile(TextAsset file)
    {
        return file.text.Split('\n');
    }

    public Card MakeCard(string name, GameObject parent)
  {
    var prefab = CardPrefabs.FirstOrDefault(o => o.name == name);
    if (prefab == null) {
      throw new KeyNotFoundException($"no card: {name}");
    }

    var instance = Instantiate(prefab, parent.transform);
    instance.name = prefab.name;
    return instance.GetComponent<Card>();
  }

    public void UpdateMasterVolume(float value)
    {
        masterVolume = value;
        UpdateVolumes();
    }

    public void UpdateMusicVolume(float value)
    {
        musicVolume = value;
        UpdateVolumes();
    }

    public void UpdateEffectsVolume(float value)
    {
        effectsVolume = value;
        UpdateVolumes();
    }

    void UpdateVolumes()
    {
        AudioListener.volume = masterVolume;
        musicMaker.volume = musicVolume;
        effectMaker.volume = effectsVolume;
        if (ambianceMaker) { ambianceMaker.volume = effectsVolume; }
    }

    public enum SoundEffects
    {
        cardInspect, cardShuffle, guardAnnouncement, papersAccept, papersReject, peopleShuffle
    }

    public void PlaySound(SoundEffects effect)
    {
        switch (effect)
        {
            case SoundEffects.cardInspect:
                effectMaker.PlayOneShot(FindEffect("Audio/Effects/Card Noises/Card Inspect"));
                break;

            case SoundEffects.cardShuffle:
                effectMaker.PlayOneShot(FindEffect("Audio/Effects/Card Noises/Card Shuffle"));
                break;

            case SoundEffects.guardAnnouncement:
                effectMaker.PlayOneShot(FindEffect("Audio/Effects/Guard Noises/Guard Announcement"));
                break;

            case SoundEffects.peopleShuffle:
                effectMaker.PlayOneShot(FindEffect("Audio/Effects/People Noises/People Shuffling"));
                break;

            case SoundEffects.papersAccept:
                effectMaker.PlayOneShot(FindEffect("Audio/Effects/Other Noises/Papers Accepted"));
                break;

            case SoundEffects.papersReject:
                effectMaker.PlayOneShot(FindEffect("Audio/Effects/Other Noises/Papers Rejected"));
                break;
        }
    }

    AudioClip FindEffect(string directory)
    {
        if (directory == "")
        {
            return null;
        }
        object[] loadedAssets = Resources.LoadAll(directory);
        AudioClip[] list = new AudioClip[loadedAssets.Length];
        for (int x = 0; x < loadedAssets.Length; x++)
        {
            list[x] = (AudioClip)loadedAssets[x];
        }
        return list[Random.Range(0, list.Length)];
    }
}
