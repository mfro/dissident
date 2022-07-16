using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAssembler : MonoBehaviour
{
    SpriteRenderer background, backHair, body, clothes, head, mouth, nose, eyes, hat;

    Sprite[] backgroundList, backHairList, bodyList, clothesList, headList, mouthList, noseList, eyesList, hatList;

    // Used for iteration across functions
    private int iter = 0;

    [Header("Runtime Testing Buttons")]
    [Tooltip("Setting true instantly re-randomizes all component sprites")]
    public bool rerandomize = false;

    [Header ("Generation Options")]
    [Tooltip("Deprecated option from when head was a separate sprite from body")]
    public bool includeHead = false;
    [Tooltip("True when there is a background hair layer. Currently just used for female")]
    public bool twoLayersOfHair = false;

    public GameObject spriteObjectPrefab;

    [Header("Directory must be in Resources and should start with immediate child folder of Resources")]
    // I know this assignment looks silly, but it's to make the Unity Inspector play well with the header
    public string backgroundDirectory;
    public string backHairDirectory, bodyDirectory, clothesDirectory, headDirectory, mouthDirectory, noseDirectory, eyesDirectory, hatDirectory;

    // Start is called before the first frame update
    void Start()
    {
        #region Load Art Assets and save them into lists
        backgroundList = LoadAssets(backgroundDirectory);
        backHairList = LoadAssets(backHairDirectory);
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

        #region Create needed children on the Human Portrait object
        iter = 0;
        background = CreateChild("Background");
        backHair = CreateChild("Back Hair");
        body = CreateChild("Body");
        clothes = CreateChild("Clothes");
        head = CreateChild("Head");
        mouth = CreateChild("Mouth");
        nose = CreateChild("Nose");
        eyes = CreateChild("Eyes");
        hat = CreateChild("Hat");
        #endregion

        #region Assign a random asset from the art asset list to its respective child sprite
        background.sprite = AssignSprite(backgroundList);
        backHair.sprite = AssignSprite(backHairList);
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
        if (directory == "")
        {
            return null;
        }
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
        if (list is null) return null;
        if (list.Length > 0)
        {
            return list[Random.Range(0, list.Length)];
        }
        return null;
    }

    private SpriteRenderer CreateChild(string name)
    {
        SpriteRenderer renderer = Instantiate(spriteObjectPrefab, transform.position, transform.rotation, transform).GetComponent<SpriteRenderer>();
        renderer.gameObject.name = name;
        renderer.sortingOrder = iter += 5;
        return renderer;
    }

    void Update()
    {
        if (rerandomize)
        {
            foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>()) {
                Destroy(child.gameObject);
            }
            Start();
            rerandomize = false;
        }
    }
}
