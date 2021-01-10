using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float ballForce = 200;
    public bool inPlay;
    int contor_Left = 0, contor_Right = 0;
    bool toTell = true;
    public Transform bar;
    public Transform explosion;
    public GameManager gm;
    public Transform extraLife, minusLife, barPlus, barMinus;
    AudioSource brickHit_sound, brickBreak_sound, ball_outOfBounds_sound;

    void Start()
    {
        var aSources = GetComponents<AudioSource>();
        brickHit_sound = aSources[0];
        brickBreak_sound = aSources[1];
        ball_outOfBounds_sound = aSources[2];
    }



    void Update()
    {
        if(gm.gameover)
        {
            return;
        }
        if(gm.gamewon)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if(!inPlay)
        {
           transform.position = new Vector2 (bar.position.x, bar.position.y + 0.5f);
           if(Input.GetMouseButtonDown(0))
           {
                rb.AddForce (Vector2.up * ballForce, ForceMode2D.Impulse);
                inPlay = true;
           }
        }
        else
        {
            if((contor_Left >= 2) && (contor_Right >= 2) && Input.GetMouseButtonDown(0))
            {
                contor_Left = 0;
                contor_Right = 0;
                toTell = true;               
                inPlay = false;
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("deathTrigger"))
        {
            ball_outOfBounds_sound.Play();
            gm.UpdateLives(-1);
            rb.velocity = Vector2.zero;
            inPlay = false;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.CompareTag("brick"))
        {
            Bricks brick = other.gameObject.GetComponent<Bricks>();
            brick.brickHits --;
            if(brick.brickHits < 1)
            {
                brickBreak_sound.Play();
                int randomChance = Random.Range(1,101);
                if(randomChance <= 15 && gm.toDropBar_lifePlus)
                {
                    Instantiate (extraLife, other.transform.position, other.transform.rotation);
                    gm.toDropBar_lifePlus = false;
                }
                else
                {
                    if(randomChance <= 25 && gm.toDropBar_lifeMinus)
                    {
                        Instantiate (minusLife, other.transform.position, other.transform.rotation);
                        gm.toDropBar_lifeMinus = false;
                    }
                    else
                    {
                        if(randomChance <= 45)
                        {
                            if(randomChance <= 35)
                            {
                                if(gm.toDropBar_widthPlus)
                                {
                                    Instantiate (barPlus, other.transform.position, other.transform.rotation);
                                    gm.toDropBar_widthPlus = false;
                                }
                            }
                            else
                            {
                                if(gm.toDropBar_widthMinus)
                                {
                                    Instantiate (barMinus, other.transform.position, other.transform.rotation);
                                    gm.toDropBar_widthPlus = false;
                                }
                            }                            
                        }
                    }
                }

                gm.UpdateScore(brick.brickPoints);
                Transform newExplosion = Instantiate (explosion, other.transform.position, other.transform.rotation);
                Destroy( newExplosion.gameObject, 2.5f);
                Destroy (other.gameObject);
            }
            else
            {
                brickHit_sound.Play();
                brick.BreakBrick();
            }
        }
        else
        {
            if(other.transform.CompareTag("ScreenBorderLeft"))
                {
                    if(other.relativeVelocity.y < 0.4f && other.relativeVelocity.y > -0.4f)
                    {
                        contor_Left ++;
                        tellPlayer();
                    }
                    else
                    {
                        contor_Left = 0;
                    }
                }
                else
                {
                    if(other.transform.CompareTag("ScreenBorderRight"))
                    {
                        if(other.relativeVelocity.y < 0.4f && other.relativeVelocity.y > -0.4f)
                        {
                            contor_Right ++;
                            tellPlayer();
                        }
                        else
                        {
                            contor_Right = 0;
                        }
                    }
                }
        }
    }

    void tellPlayer()
    {
        if((contor_Left >= 2) && (contor_Right >= 2) && toTell)
        {
            Debug.Log("Press space to reset ball");
            toTell = false;
        }
    }

}
    