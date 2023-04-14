using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    //Velocidad inicial del fantasma
    public float speed = 0.3f;
    //Necesitamos un array de posiciones llamado Waypoints(puntos de ruta). Cada fantasma puede tener un número de puntos de ruta distintos
    public Transform[] waypoints;
    //Inicializo la posición en la que se encuentra el fantasma en la posicion 0 del array. Luego este valor irá variando
    public int currentWaypoint = 0;
    public int returnWaypoint = 0;
    public AudioSource audioBox;
    public AudioClip normalRun;
    public AudioClip ghostRun;

    bool ghostMode;
    bool alive;
    bool checkWaypoints;

    // Start is called before the first frame update
    void Start()
    {
        checkWaypoints = false;
        alive = true;
        ghostMode = false;
        audioBox.loop = true;
        audioBox.Play();

    }
    void Update()
    {
        //si PacMan sigue siendo invencible
        if (GameManager.instance.invincibleCounter > 0)
        {
            //El fantasma cambia a color azul
            GetComponent<Animator>().SetBool("PacManInvincible", true);
            gameObject.tag = "EdibleGhost";
            if (ghostMode)
            {
                audioBox.clip = ghostRun;
                audioBox.loop = true;
                audioBox.Play();
                ghostMode = false;
            }
        }
        else
        {
            //El fantasma queda como al principio
            GetComponent<Animator>().SetBool("PacManInvincible", false);
            gameObject.tag = "Ghost";
            if (!ghostMode)
            {
                audioBox.clip = normalRun;
                audioBox.loop = true;
                audioBox.Play();
                ghostMode = true;
            }
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (alive)
        {
            //Distancia al punto de ruta, entre la posición actual y el punto de ruta hacia el que se está dirigiendo
            float distanceToWaypoint = Vector2.Distance((Vector2)transform.position, (Vector2)waypoints[currentWaypoint].position);//Current position of the go, and mines the position as waypoint, (Vector2) to change it to 2 vector, not 3 that it has as default in transform

            //Si la distancia hasta el punto de ruta es menor que 0.1 es que he llegado a la posicion
            if (distanceToWaypoint < 0.1f)
            {
                //Si el número del punto en el que está el fantasma es menor que los que hay guardados
                if (currentWaypoint < waypoints.Length - 1)
                {
                    //Avanzamos al siguiente punto
                    currentWaypoint++;
                }
                //Si por el contrario el número del punto en el que está el fantasma es igual o mayor que los que hay guardados
                else
                {
                    //Reseteamos al primer puntos de los guardados
                    currentWaypoint = 0;
                }

                //Nueva dirección para calcular la animación si cambiamos de dirección: hacia donde va - donde está ahora
                Vector2 newDirection = waypoints[currentWaypoint].position - transform.position;
                //Cambiamos las animaciones
                GetComponent<Animator>().SetFloat("DirX", newDirection.x);
                GetComponent<Animator>().SetFloat("DirY", newDirection.y);

            }
            //Y si no ha llegado al destino el fantasma
            else
            {
                if (!GameManager.instance.deadPacMan)
                {
                    //Creo un vector para moverme desde donde está el fantasma ahora mismo, hasta el siguiente waypoint a una velocidad dada
                    Vector2 newPos = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);
                    //Hacemos que se mueva a la posición que le toca
                    GetComponent<Rigidbody2D>().MovePosition(newPos);
                }
            }
        }
        else
        {
            
            if(currentWaypoint > 0)
            {
                //Distancia al punto de ruta, entre la posición actual y el punto de ruta hacia el que se está dirigiendo
                float distanceToWaypoint = Vector2.Distance((Vector2)transform.position, (Vector2)waypoints[currentWaypoint - 1].position);//Current position of the go, and mines the position as waypoint, (Vector2) to change it to 2 vector, not 3 that it has as default in transform

                //Si la distancia hasta el punto de ruta es menor que 0.1 es que he llegado a la posicion
                if (distanceToWaypoint < 0.1f)
                {
                    //Si el número del punto en el que está el fantasma es menor que los que hay guardados
                    if (currentWaypoint < waypoints.Length)
                    {

                        //Avanzamos al siguiente punto
                        currentWaypoint--;
                    }
                    //Si por el contrario el número del punto en el que está el fantasma es igual o mayor que los que hay guardados
                    else
                    {
                        Debug.Log("i crash here");
                        //Reseteamos al primer puntos de los guardados
                        currentWaypoint = 0;
                    }

                    //Nueva dirección para calcular la animación si cambiamos de dirección: hacia donde va - donde está ahora
                    Vector2 newDirection = waypoints[currentWaypoint].position - transform.position;
                    //Cambiamos las animaciones
                    GetComponent<Animator>().SetFloat("DirX", newDirection.x);
                    GetComponent<Animator>().SetFloat("DirY", newDirection.y);

                }
                //Y si no ha llegado al destino el fantasma
                else
                {
                    if (!GameManager.instance.deadPacMan)
                    {
                        if (currentWaypoint > 0)
                        {
                            //Creo un vector para moverme desde donde está el fantasma ahora mismo, hasta el siguiente waypoint a una velocidad dada
                            Vector2 newPos = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint - 1].position, speed * 0.4f);
                            //Hacemos que se mueva a la posición que le toca
                            GetComponent<Rigidbody2D>().MovePosition(newPos);
                            Debug.Log("GOing back");
                        }

                    }
                }
            }
            else
            {
                currentWaypoint = 0;
                alive = true;
            }
            
            /* if (checkWaypoints)
             {
                 returnWaypoint = currentWaypoint;
                 checkWaypoints = false;
             }

             for(int i =0; i < returnWaypoint; i--)
             {
                 Vector2 newPos = Vector2.MoveTowards(transform.position, waypoints[returnWaypoint].position, speed * Time.deltaTime);
                 //Hacemos que se mueva a la posición que le toca
                 GetComponent<Rigidbody2D>().MovePosition(newPos);
                 returnWaypoint--;
                 if (returnWaypoint == 0)
                 {
                     Debug.Log("ima alive");
                 }
             }*/


        }


    }

    //Reacción de los fantasmas al choque con otros objetos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.tag == "Ghost")
        {
            //Destruye a PacMan(obteniendo de este GO suPara poder coger de este el método MachoPacManDeath()
            collision.GetComponent<MrPacManMovement>().MachoPacManDeath();
        }
        if (collision.CompareTag("Player") && gameObject.tag == "EdibleGhost")
        {
            //Goback 
            alive = false;
            checkWaypoints = true;
        }
    }
}
