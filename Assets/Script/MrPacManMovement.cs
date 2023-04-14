using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrPacManMovement : MonoBehaviour
{
    //Velocidad de PacMan
    public float speed = 0.4f;
    public float soundTimer;
    public bool isOn;

    //Referencia al Rigidbody al jugador
    private Rigidbody2D theRB;
    //Referencia al animator del jugador
    private Animator anim;

    //Audio
    public AudioSource audioBox;
    //public AudioClip nomnomSound;
    public AudioClip ouchyUwuPacmanded;
    public AudioClip nomnom;
    public AudioClip nomnomGhost;
    public AnimationClip death;
    

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        soundTimer = 0;
        //Inicializamos el Rigidbody
        theRB = GetComponent<Rigidbody2D>();
        //Inicializamos el Animator
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()//FixedUpdate for a better continuous movement
    {
        if (!GameManager.instance.deadPacMan)
        {
            //Si pulso la tecla izquierda
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                //Movemos al personaje en esa dirección
                theRB.velocity = new Vector2(-speed, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                theRB.velocity = new Vector2(speed, 0);
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                theRB.velocity = new Vector2(0, speed);
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                theRB.velocity = new Vector2(0, -speed);
            }

            //ANIMATIONS
            //Dependiendo del valor de la velicdad del Rigidbody en X, Pacman mirará a la derecha o a la izquierda
            anim.SetFloat("DirX", theRB.velocity.x);//Accedemos al Animator de Pacman, y aplicando un cambio en su parámetro DirX, conseguimos el cambio en la animación
                                                    //Para la Y aplicariamos lo mismo que para la X
            anim.SetFloat("DirY", theRB.velocity.y);
        }
        else
        {
            theRB.velocity = new Vector2(0, 0);
        }
        /*
        if(soundTimer > 0)
        {
            isOn = true;
            soundTimer -= Time.deltaTime;
        }
        else if(soundTimer <=0)
        {
            Debug.Log("not playing sound");
            isOn = false;
            audioBox.Stop();
        }
        if (isOn)
        {
            Debug.Log("playing sound");
            audioBox.Play();
        }*/
        
    }
    public void MachoPacManDeath()
    {
        //Time.timeScale = 0f;
        audioBox.Stop();
        GameManager.instance.StopAudios();
        audioBox.PlayOneShot(ouchyUwuPacmanded);
        GameManager.instance.deadPacMan = true;
        anim.SetBool("Death", true);
        float animt = death.length;
        //(float)anim.GetCurrentAnimatorClipInfo(0).LongLength; //Returns 1, instead of 1.6 of the length of current animation

        //Destroy(gameObject, 1.6f);
        Invoke("GMmethod", animt);
        Destroy(gameObject, animt);
        
    }
    public void SoundTimerUp()
    {
        soundTimer++;
    }
    public void SoundOn()
    {
        audioBox.Play();
    }
    public void SoundOff()
    {
        audioBox.Stop();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            audioBox.PlayOneShot(nomnom);
        }

        if (collision.gameObject.CompareTag("EdibleGhost"))
        {
            audioBox.PlayOneShot(nomnomGhost);
            //killghost
        }
    }

    public void GMmethod()
    {
        Debug.Log("GMmehjdodoaosd");
        GameManager.instance.Death();
    }

}
