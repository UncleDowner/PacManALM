using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDot : MonoBehaviour
{
    //M�todo para conocer un objeto se ha metido en la zona de trigger de la bola
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el objeto que ha entrado en el trigger est� etiquetado como Player
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<MrPacManMovement>().SoundTimerUp();
            //collision.GetComponent<MrPacManMovement>().audioBox.Play();
            //Podr�a sumar puntos
            GameManager.instance.ScoreUp();
            //Recogemos el objeto PacDot concreto, en nuestro caso lo eliminamos
            Destroy(gameObject);
        }
    }
}
