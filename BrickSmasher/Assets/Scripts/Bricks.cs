using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public int brickPoints;
    public int brickHits;
    public Sprite hitSprite;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void BreakBrick()
    {
        GetComponent<SpriteRenderer>().sprite = hitSprite;
    }
}
