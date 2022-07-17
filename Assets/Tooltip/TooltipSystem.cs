using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{

    private static TooltipSystem current;

    public Tooltip tooltipNPC;
    public Tooltip tooltipCard;

    // Start is called before the first frame update
    void Awake()
    {
        current = this;
        current.tooltipNPC.gameObject.SetActive(false);
        current.tooltipCard.gameObject.SetActive(false);
    }

    public static void ShowNPC(string content, string name)
    {
        current.tooltipNPC.Configure(content, name);
        current.tooltipNPC.gameObject.SetActive(true);
    }

    public static void HideNPC()
    {
        current.tooltipNPC.gameObject.SetActive(false);
    }

    public static void ShowCard(Sprite portrait, string content, List<CardTrait> traits, string name)
    {
        current.tooltipCard.Configure(portrait, content, traits, name);
        current.tooltipCard.gameObject.SetActive(true);
    }

    public static void HideCard()
    {
        current.tooltipCard.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
