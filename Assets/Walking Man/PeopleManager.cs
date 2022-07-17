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
    }

    void StartAllWalking()
    {
        foreach (GameObject man in allPeople)
        {
            man.GetComponent<WalkingManController>().WalkForward();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
