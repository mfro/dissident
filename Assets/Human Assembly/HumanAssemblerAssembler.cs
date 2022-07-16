using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAssemblerAssembler : MonoBehaviour
{
    public GameObject malePrefab;
    public GameObject femalePrefab;
    public PixelText nameObject;

    public string maleNameFile;
    public string femaleNameFile;

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
        return "Jeffrey Epstein";
    }
}
