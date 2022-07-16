using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAssembler : MonoBehaviour
{
    SpriteRenderer background, body, head, mouth, nose, eyes, hat;

    public Sprite[] backgroundList, bodyList, headList, mouthList, noseList, eyesList, hatList;

    // Start is called before the first frame update
    void Start()
    {
        background = transform.Find("Background").GetComponent<SpriteRenderer>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
        head = transform.Find("Head").GetComponent<SpriteRenderer>();
        mouth = transform.Find("Mouth").GetComponent<SpriteRenderer>();
        nose = transform.Find("Nose").GetComponent<SpriteRenderer>();
        eyes = transform.Find("Eyes").GetComponent<SpriteRenderer>();
        hat = transform.Find("Hat").GetComponent<SpriteRenderer>();

        background.sprite = backgroundList[Random.Range(0, backgroundList.Length)];
        body.sprite = bodyList[Random.Range(0, bodyList.Length)];
        head.sprite = headList[Random.Range(0, headList.Length)];
        mouth.sprite = mouthList[Random.Range(0, mouthList.Length)];
        nose.sprite = noseList[Random.Range(0, noseList.Length)];
        eyes.sprite = eyesList[Random.Range(0, eyesList.Length)];
        hat.sprite = hatList[Random.Range(0, hatList.Length)];
    }
}
