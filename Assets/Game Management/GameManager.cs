using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public bool visibleMouse = true;

    void Awake()
    {
        if (gm == null)
        {
            gm = this.GetComponent<GameManager>();
            DontDestroyOnLoad(this);
        }

        else
        {
            GameManager.gm.Start();
            Destroy(this);
            return;
        }

    }

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
