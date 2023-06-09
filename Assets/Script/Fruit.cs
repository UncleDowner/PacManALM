using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    //Tiempo espec�fico para cada fruta
    public float fruitTime;

    //M�todo para que la fruta sea recogida
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el objecto que se ha metido donde est� la fruta es el jugador
        if (collision.CompareTag("Player"))
        {
            //Llamamos al m�todo del GameManager que inicializa el contador de tiempo de invencibilidad de PacMan
            GameManager.instance.MakeInvincibleFor(fruitTime+10);

            //Eliminamos la fruta
            Destroy(gameObject);
        }
    }
}
