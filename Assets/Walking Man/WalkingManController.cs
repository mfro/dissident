using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingManController : MonoBehaviour
{
    public float movementSpeed = 10f;
    public Vector3 direction = new Vector3(0, 1, 0);
    bool moving = false;
    public float[] yStoppingPositions;
    int positionInYPoints = 0;

    Animator _anim;

    [HideInInspector]
    public PeopleManager myManager;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position += direction * movementSpeed * Time.deltaTime;
            if (transform.position.y >= yStoppingPositions[positionInYPoints]) { SetWalking(false); }
        }

    }

    public void WalkForward()
    {
        positionInYPoints++;
        if (positionInYPoints >= yStoppingPositions.Length) 
        {
            myManager.KillMan(this.gameObject);
        }
        SetWalking(true);
    }

    public void SetWalking(bool walking)
    {
        if (moving == walking)
        {
            return;
        }
        moving = walking;
        if (!_anim) { _anim = GetComponentInChildren<Animator>(); }
        _anim.SetBool("Walking", moving);
    }

    public void ToggleWalking()
    {
        SetWalking(!moving);
    }
}
