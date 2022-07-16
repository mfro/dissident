using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipNPCTrigger : MonoBehaviour
{

    [SerializeField] string nameNPC;
    [Multiline()]
    [SerializeField] string content;
    [SerializeField] Sprite portrait;

    [SerializeField] float awakeDelay = 0.25f;
    [SerializeField] float sleepDelay = 0.1f;


    IEnumerator DelayedShow()
    {
        yield return new WaitForSeconds(awakeDelay);
        TooltipSystem.ShowNPC(portrait, content, nameNPC);
    }

    IEnumerator DelayedHide()
    {
        yield return new WaitForSeconds(sleepDelay);
        TooltipSystem.HideNPC();
    }

    public void OnMouseEnter()
    {
        StartCoroutine(DelayedShow());
        StopCoroutine(DelayedHide());
    }

    public void OnMouseExit()
    {
        StopCoroutine(DelayedShow());
        StartCoroutine(DelayedHide());
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
