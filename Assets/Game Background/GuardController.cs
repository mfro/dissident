using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    // Start is called before the first frame update

    private SpriteRenderer sr;

    [SerializeField] private Sprite[] disapprovalFrames;
    [SerializeField] private Sprite[] approvalFrames;
    [SerializeField] private Sprite baseFrame;

    public float approvalSpeed;
    public float disapprovalSpeed;

    bool playingApproval;
    bool playingDisapproval;

    [SerializeField] bool ToggleApproval;
    [SerializeField] bool ToggleDisapproval;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private IEnumerator PlayApproval()
    {
        for(int i = 0; i < approvalFrames.Length; i++)
        {
            yield return new WaitForSeconds(approvalSpeed);
            if (!playingApproval) break;
            sr.sprite = approvalFrames[i];
        }
        playingApproval = false;
    }

    public void GuardApproval()
    {
        playingApproval = true;
        playingDisapproval = false;
        StartCoroutine(PlayApproval());
    }

    private IEnumerator PlayDisapproval()
    {
        for (int i = 0; i < approvalFrames.Length; i++)
        {
            yield return new WaitForSeconds(disapprovalSpeed);
            if (!playingDisapproval) break;
            sr.sprite = disapprovalFrames[i];
        }
        playingDisapproval = false;
    }

    public void GuardDisapproval()
    {
        playingApproval = false;
        playingDisapproval = true;
        StartCoroutine(PlayDisapproval());
    }

    // Update is called once per frame
    void Update()
    {
        if(playingApproval == false && playingDisapproval == false)
        {
            sr.sprite = baseFrame;
        }

        if(ToggleApproval)
        {
            GuardApproval();
            ToggleApproval = false;
        }
        else if (ToggleDisapproval)
        {
            GuardDisapproval();
            ToggleDisapproval = false;
        }
    }
}
