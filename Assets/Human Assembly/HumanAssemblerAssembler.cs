using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HumanAssemblerAssembler : MonoBehaviour
{
    public GameObject malePrefab;
    public GameObject femalePrefab;
    public PixelText nameObject;
    public TextMeshPro flavorObject;

    bool male;
    string myName;
    string myFlavor;

    TooltipSystem theSystem;
    NPCTooltipTrigger myTrigger;

    // Start is called before the first frame update
    void Start()
    {
        male = Random.Range(0, 2) == 0;
        Instantiate(male ? malePrefab : femalePrefab, transform);
        myName = GenerateName();
        myFlavor = GenerateFlavor();
        nameObject.text = myName;
        flavorObject.text = myFlavor;

        theSystem = FindObjectOfType<TooltipSystem>();
        myTrigger = GetComponent<NPCTooltipTrigger>();
    }

    string GenerateName()
    {
        string[] firstNames = male ? GameManager.gm.maleNames : GameManager.gm.femaleNames;
        string[] lastNames = GameManager.gm.lastNames;
        return firstNames[Random.Range(0, firstNames.Length)] + " " + lastNames[Random.Range(0, lastNames.Length)];
    }

    string GenerateFlavor()
    {
        return GameManager.gm.flavors[Random.Range(0, GameManager.gm.flavors.Length)];
    }
}
