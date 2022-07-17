using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{

    [SerializeField] private float moveDuration;
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
        SetAllWalking(true);
    }

    public void SpawnPerson()
    {
        GameObject man = Instantiate(manPrefab, spawnPosition, Quaternion.identity);
        allPeople.Add(man);
    }

    void SetAllWalking(bool state)
    {
        foreach (GameObject man in allPeople)
        {
            man.GetComponent<WalkingManController>().SetWalking(state);
        }
    }

    private void StartWalking()
    {
        SetAllWalking(true);
        StartCoroutine(StopWalking());
        stillWalking = !stillWalking;
    }

    private IEnumerator StopWalking()
    {
        yield return new WaitForSeconds(moveDuration);
        SetAllWalking(false);
    }



    // Update is called once per frame
    void Update()
    {
        if (stillWalking)
        {
            if (movingStartTime + moveDuration <= Time.time)
            {
                stillWalking = false;
                SetAllWalking(false);
            }
        }
    }
}
