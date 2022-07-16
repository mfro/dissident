using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAssemblerAssembler : MonoBehaviour
{
    public GameObject malePrefab;
    public GameObject femalePrefab;
    public PixelText nameObject;

    bool male;
    string myName;

    // Start is called before the first frame update
    void Start()
    {
        male = Random.Range(0, 2) == 0;
        Instantiate(male ? malePrefab : femalePrefab, transform);
        myName = GenerateName();
        nameObject.text = myName;
    }

    string GenerateName()
    {
        string[] firstNames = male ? GameManager.gm.maleNames : GameManager.gm.femaleNames;
        string[] lastNames = GameManager.gm.lastNames;
        return firstNames[Random.Range(0, firstNames.Length)] + lastNames[Random.Range(0, lastNames.Length)];
    }
}
