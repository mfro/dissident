using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAssembler : MonoBehaviour
{
    SpriteRenderer background, backHair, body, clothes, head, mouth, nose, eyes, hat;

    Sprite[] backgroundList, bodyList, clothesList, headList, mouthList, noseList, eyesList, hatList;

    // Used for iteration across functions
    private int iter = 0;

    [Header("Runtime Testing Buttons")]
    [Tooltip("Setting true instantly re-randomizes all component sprites")]
    public bool rerandomize = false;

    [Header ("Generation Options")]
    [Tooltip("Deprecated option from when head was a separate sprite from body")]
    public bool includeHead = false;
    [Tooltip("True when there is a background hair layer")]
    public bool twoLayersOfHair = false;

    public GameObject spriteObjectPrefab;

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

        #region Create needed children on the Human Portrait object
        iter = 0;
        background = CreateChild("Background");
        background.transform.position = new Vector3(0, -1.03125f, 0) + background.transform.position;
        body = CreateChild("Body");
        clothes = CreateChild("Clothes");
        if (includeHead)
        {
            head = CreateChild("Head");
        }
        mouth = CreateChild("Mouth");
        nose = CreateChild("Nose");
        eyes = CreateChild("Eyes");
        hat = CreateChild("Hat");
        if (twoLayersOfHair)
        {
            backHair = CreateChild("Back Hair", hat.transform);
            backHair.sortingOrder = 6;
        }
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
        if (twoLayersOfHair)
        {
            AssignHair(hat, backHair);
        }
        else
        {
            hat.sprite = AssignSprite(hatList);
        }
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

    private Sprite AssignSprite(Sprite[] list, int ID = -1)
    {
        if (list is null) return null;
        if (list.Length > 0 && ID == -1)
        {
            return list[Random.Range(0, list.Length)];
        }
        if (list.Length > 0 && ID >= 0)
        {
            return list[ID];
        }
        return null;
    }

    private void AssignHair(SpriteRenderer front, SpriteRenderer back)
    {
        int backID = Random.Range(0, hatList.Length / 2) * 2;
        front.sprite = AssignSprite(hatList, backID + 1);
        backHair.sprite = AssignSprite(hatList, backID);
    }

    private SpriteRenderer CreateChild(string name, Transform parent = null)
    {
        if (parent is  null) { parent = transform; }
        SpriteRenderer renderer = Instantiate(spriteObjectPrefab, transform.position, transform.rotation, parent).GetComponent<SpriteRenderer>();
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
