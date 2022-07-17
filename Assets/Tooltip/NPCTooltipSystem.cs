using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTooltipSystem : MonoBehaviour
{
    Tooltip tooltipNPC;

    // Start is called before the first frame update
    void Awake()
    {
        tooltipNPC = GetComponentInChildren<Tooltip>();

        tooltipNPC.gameObject.SetActive(false);
    }

    public void ShowNPC(string content, string name)
    {
        tooltipNPC.Configure(content, name);
        tooltipNPC.gameObject.SetActive(true);
    }

    public void HideNPC()
    {
        tooltipNPC.gameObject.SetActive(false);
    }
}
