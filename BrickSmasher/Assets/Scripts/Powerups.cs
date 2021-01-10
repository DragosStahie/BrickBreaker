using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{

    public float speed;
    public GameManager gm;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        transform.Translate(new Vector2(0f, -1f) * Time.deltaTime * speed);
        if(transform.position.y <= -6f)
        {
            Destroy(gameObject);
            gm.toDropBar_lifeMinus = true;
            gm.toDropBar_lifePlus = true;
            gm.toDropBar_widthMinus = true;
            gm.toDropBar_widthPlus = true;
        }        
    }

    
}
