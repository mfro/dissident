using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameObject[] CardPrefabs;

  public static GameManager gm;
  public bool visibleMouse = true;

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

  public Card MakeCard(string name, GameObject parent)
  {
    var prefab = CardPrefabs.First(o => o.name == name);
    var instance = Instantiate(prefab, parent.transform);

    instance.name = prefab.name;

    return instance.GetComponent<Card>();
  }
}
