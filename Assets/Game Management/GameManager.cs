using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject[] CardPrefabs;

    public static GameManager gm;
    public bool visibleMouse = true;

    [Range(0f, 1f)]
    public float masterVolume = 0.5f;

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
      Destroy(this);
      return;
    }

    CardPrefabs = Resources.LoadAll<GameObject>("Cards");
  }

    void Start()
    {
        maleNames = ParseFile(maleNameFile);
        femaleNames = ParseFile(femaleNameFile);
        lastNames = ParseFile(lastNameFile);
    }

    private void Update()
    {
        AudioListener.volume = masterVolume;
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
        AudioListener.volume = masterVolume = value;
    }
}
