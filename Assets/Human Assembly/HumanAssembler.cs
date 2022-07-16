using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAssembler : MonoBehaviour
{
    SpriteRenderer background, body, clothes, head, mouth, nose, eyes, hat;

    Sprite[] backgroundList, bodyList, clothesList, headList, mouthList, noseList, eyesList, hatList;

    public bool includeHead = false;

    public bool rerandomize = false;

    [Header("Directory must be in Resources and should start with immediate child folder of Resources")]
    // I know this assignment looks silly, but it's to make the Unity Inspector play well with the header
    public string backgroundDirectory;
    public string bodyDirectory, clothesDirectory, headDirectory, mouthDirectory, noseDirectory, eyesDirectory, hatDirectory;

    // Start is called before the first frame update
    void Start()
    {
        #region Load Art Assets and save them into lists
        backgroundList = LoadAssets(backgroundDirectory);
        bodyList = LoadAssets(bodyDirectory);
        clothesList = LoadAssets(clothesDirectory);
        if (includeHead)
        {
            headList = LoadAssets(headDirectory);
        }
        mouthList = LoadAssets(mouthDirectory);
        noseList = LoadAssets(noseDirectory);
        eyesList = LoadAssets(eyesDirectory);
        hatList = LoadAssets(hatDirectory);
        #endregion

        #region Find needed children on the Human Portrait object
        background = transform.Find("Background").GetComponent<SpriteRenderer>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
        clothes = transform.Find("Clothes").GetComponent<SpriteRenderer>();
        head = transform.Find("Head").GetComponent<SpriteRenderer>();
        mouth = transform.Find("Mouth").GetComponent<SpriteRenderer>();
        nose = transform.Find("Nose").GetComponent<SpriteRenderer>();
        eyes = transform.Find("Eyes").GetComponent<SpriteRenderer>();
        hat = transform.Find("Hat").GetComponent<SpriteRenderer>();
        #endregion

        #region Assign a random asset from the art asset list to its respective child sprite
        background.sprite = AssignSprite(backgroundList);
        body.sprite = AssignSprite(bodyList);
        clothes.sprite = AssignSprite(clothesList);
        if (includeHead)
        {
            head.sprite = AssignSprite(headList);
        }
        mouth.sprite = AssignSprite(mouthList);
        nose.sprite = AssignSprite(noseList);
        eyes.sprite = AssignSprite(eyesList);
        hat.sprite = AssignSprite(hatList);
        #endregion
    }

    private Sprite[] LoadAssets(string directory)
    {
        object[] loadedAssets = Resources.LoadAll(directory, typeof(Sprite));
        Sprite[] list = new Sprite[loadedAssets.Length];
        for (int x = 0; x < loadedAssets.Length; x++)
        {
            list[x] = (Sprite)loadedAssets[x];
        }
        return list;
    }

    private Sprite AssignSprite(Sprite[] list)
    {
        if (list.Length > 0)
        {
            return list[Random.Range(0, list.Length)];
        }
        return null;
    }

    void Update()
    {
        if (rerandomize)
        {
            Start();
            rerandomize = false;
        }
    }
}
