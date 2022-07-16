using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour
{
    [SerializeField] bool isNPC;

    [SerializeField] string nameString;
    [Multiline()]
    [SerializeField] string contentString;
    [SerializeField] Sprite portrait;
    [SerializeField] string[] traits;


    [SerializeField] float awakeDelay = 0.25f;

    IEnumerator DelayedShow()
    {
        yield return new WaitForSeconds(awakeDelay);

        if (isNPC)
        {
            TooltipSystem.ShowNPC(portrait, contentString, nameString);
        } 
        else
        {
            TooltipSystem.ShowCard(portrait, contentString, traits, nameString);
        }

    }

    private void Hide()
    {

        if(isNPC)
        {
            TooltipSystem.HideNPC();
        }
        else
        {
            TooltipSystem.HideCard();
        }

    }

    public void OnMouseEnter()
    {
        StartCoroutine(DelayedShow());
        Hide();
    }

    public void OnMouseExit()
    {
        StopCoroutine(DelayedShow());
        Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
