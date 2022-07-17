using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{

    [SerializeField] SpriteRenderer portraitRenderer;

    [SerializeField] PixelText nameText;
    [SerializeField] PixelText contentsText;
    [SerializeField] PixelText traitsText;



    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void Configure(string content, string name)
    {
        contentsText.text = content;
        nameText.name = name;
    }

    private string GetKeywordText(CardTrait trait)
    {

        return trait.ToString();
/*        switch(trait)
        {
            case CardTrait.Alive:
                return "This card is currently alive";
            case CardTrait.Anthrax:
                return "This card will kill a guard";
            case CardTrait.Document:
                return "This is a document. Guards will process them when they come in contact";
            case CardTrait.Patrol:
                return "Guards with this property move left and right in a circular pattern instead of up and down";
            case CardTrait.Police:
                return "uh";
            case CardTrait.Static:
                return "This card does not move";
            case CardTrait.Suspicious:
                return "Guards will confiscate this document, and arrest the associated citizen";
            default:
                return "";
        }*/
    }

    public void Configure(Sprite portrait, string content, List<CardTrait> traits, string name)
    {
        portraitRenderer.sprite = portrait;
        contentsText.text = content;

        string traitsString = "";
        for(int i = 0; i < traits.Count; i++)
        {
            traitsString += GetKeywordText(traits[i]);
            if (i != traits.Count - 1) traitsString += ", ";
        }

        traitsText.text = traitsString;
        nameText.text = name;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
