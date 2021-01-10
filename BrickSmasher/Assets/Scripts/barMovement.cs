using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barMovement : MonoBehaviour
{
    public float barSpeed = 500f;
    public GameManager gm;

    AudioSource extraLife_sound, minusLife_sound, plusBar_sound, minusBar_sound;

    private void Start()
    {
        var aSources = GetComponents<AudioSource>();
        extraLife_sound = aSources[0];
        minusLife_sound = aSources[1];
        plusBar_sound = aSources[2]; 
        minusBar_sound = aSources[3];   
    }

    void FixedUpdate()
    {
        if(gm.gameover || gm.gamewon)
        {
            return;
        }

        /*float horizontal_keyboard = Input.GetAxis("Horizontal");

        transform.Translate(Vector2.right * barSpeed * horizontal_keyboard * Time.deltaTime);
        */

        Vector2 horizontal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(horizontal.x, transform.position.y);

        if(transform.position.x < gm.leftScreenEdge)
        {
            transform.position = new Vector2(gm.leftScreenEdge,transform.position.y);
        }

        if(transform.position.x > gm.rightScreenEdge)
        {
            transform.position = new Vector2(gm.rightScreenEdge,transform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("extraLifePowerup"))
        {
            extraLife_sound.Play();
            gm.UpdateLives(1);
            gm.toDropBar_lifePlus = true;
            Destroy(other.gameObject);
        }
        else
        {
            if(other.CompareTag("minusLifePowerup"))
            {
                minusLife_sound.Play();
                gm.UpdateLives(-1);
                gm.toDropBar_lifeMinus = true;
                Destroy(other.gameObject);
            }
            else
            {
                if(other.CompareTag("plusBarWidth"))
                {
                    if(!gm.isActivated_plus)
                    {
                        gm.isActivated_plus = true;
                        plusBar_sound.Play();
                        gm.UpdateBarWidth(1);
                        StartCoroutine(gm.StartCountdown(5f,1));
                        gm.toDropBar_widthPlus = false;
                    }
                    Destroy(other.gameObject);
                }
                else
                {
                    if(other.CompareTag("minusBarWidth"))
                    {
                        if(!gm.isActivated_minus)
                        {
                            gm.isActivated_minus = true;
                            minusBar_sound.Play();
                            gm.UpdateBarWidth(-1);
                            StartCoroutine(gm.StartCountdown(5f,-1));
                            gm.toDropBar_widthMinus = false;
                        }
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}
