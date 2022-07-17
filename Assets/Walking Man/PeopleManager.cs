using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{

    private List<GameObject> allPeople;

    [SerializeField] private GameObject manPrefab;

    [SerializeField] Vector3 spawnPosition;

    public bool stillWalking;

    float movingStartTime;

    List<GameObject> killList = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        allPeople = new List<GameObject>();
        stillWalking = false;
    }

    public void MoveLine()
    {
        movingStartTime = Time.time;
        stillWalking = true;

        SpawnPerson();
        StartAllWalking();
    }

    public void SpawnPerson()
    {
        GameObject man = Instantiate(manPrefab, spawnPosition, Quaternion.identity);
        allPeople.Add(man);
        man.GetComponent<WalkingManController>().myManager = this;
    }

    void StartAllWalking()
    {
        foreach (GameObject man in allPeople)
        {
            if (man is null)
            {
                allPeople.Remove(man);
            }
            man.GetComponent<WalkingManController>().WalkForward();
        }
        foreach (GameObject man in killList)
        {
            allPeople.Remove(man);
            Destroy(man);
        }
        killList = new List<GameObject>();
    }

    public void KillMan(GameObject deadGuy)
    {
        killList.Add(deadGuy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
