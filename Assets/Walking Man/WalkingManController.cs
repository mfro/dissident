using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingManController : MonoBehaviour
{
    public float movementSpeed = 10f;
    public Vector3 direction = new Vector3(0, 1, 0);
    bool moving = false;

    Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position += direction * movementSpeed * Time.deltaTime;
        }
    }

    public void SetWalking(bool walking)
    {
        if (moving == walking)
        {
            return;
        }
        moving = walking;
        _anim.SetBool("Walking", moving);
    }

    public void ToggleWalking()
    {
        SetWalking(!moving);
    }
}
