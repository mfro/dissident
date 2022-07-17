using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCTooltipTrigger : MonoBehaviour
{

    [SerializeField] string nameString;
    [Multiline()]
    [SerializeField] string contentString;
    [SerializeField] string[] traits;


    [SerializeField] float awakeDelay = 0.25f;

    IEnumerator DelayedShow()
    {
        yield return new WaitForSeconds(awakeDelay);
        TooltipSystem.ShowNPC(contentString, nameString);
    }

    private void Hide()
    {
        TooltipSystem.HideNPC();
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
