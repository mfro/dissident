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

    public void Configure(Sprite portrait, string content, string name)
    {
        portraitRenderer.sprite = portrait;
        contentsText.text = content;
        nameText.name = name;
    }

    public void Configure(Sprite portrait, string content, string[] traits, string name)
    {
        portraitRenderer.sprite = portrait;
        contentsText.text = content;

        //TODO: keywords
        nameText.text = name;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
