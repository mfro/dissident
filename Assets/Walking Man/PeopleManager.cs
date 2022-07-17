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


    // Start is called before the first frame update
    void Awake()
    {
        allPeople = new List<GameObject>();
        stillWalking = false;
        SpawnPerson();
    }

    public void MoveLine()
    {
        ToggleAllWalking();
    }

    public void SpawnPerson()
    {
        GameObject man = Instantiate(manPrefab, spawnPosition, Quaternion.identity);
        allPeople.Add(man);
    }

    private void ToggleAllWalking()
    {
        for (int i = 0; i < allPeople.Count; i++)
        {
            WalkingManController wmc = allPeople[i].GetComponent<WalkingManController>();
            wmc.toggleWalking = true;
            StartCoroutine(StopWalking());
            stillWalking = !stillWalking;
        }
    }

    private IEnumerator StopWalking()
    {
        yield return new WaitForSeconds(moveDuration);
        ToggleAllWalking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
