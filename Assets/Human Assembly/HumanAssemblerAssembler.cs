using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAssemblerAssembler : MonoBehaviour
{
    public GameObject[] portraitPrefabs;
    GameObject chosenPortrait;

    // Start is called before the first frame update
    void Start()
    {
        if (portraitPrefabs.Length > 0)
        {
            chosenPortrait = portraitPrefabs[Random.Range(0, portraitPrefabs.Length)];
            Instantiate(chosenPortrait, transform);
        }
    }
}
