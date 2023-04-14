using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Variable para controlar el tiempo en el que PacMan es invencible
    public float invincibleCounter = 0.0f;

    //Creamos un Singleton
    public static GameManager instance;

    public GameObject[] dots;
    public int numDots;

    public TextMeshProUGUI scoreText;
    int score;

    public AudioSource audioBox;
    public AudioClip intro;
    public AudioClip backgroundMusic;
    public AudioClip fruittyTime;

    public float timeIntro;

    public bool deadPacMan;
    public bool fruittyMode;

    public float counterBegin;

    public GameObject reloadButton;
    public TextMeshProUGUI resultText;

    public AudioSource[] allaudios;

    void Awake()
    {
        Time.timeScale = 1;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //allaudios = FindObjectsOfType(AudioSource) as AudioSource[];
        allaudios = FindObjectsOfType<AudioSource>() as AudioSource[];

        reloadButton.SetActive(false);
        resultText.enabled = false;
        fruittyMode = false;
        audioBox.loop = true;
        audioBox.Play();

        timeIntro = intro.length;

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        deadPacMan = false;
        dots = GameObject.FindGameObjectsWithTag("DOT");
        numDots = dots.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
        /*TODO
        if(timeIntro <= 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            timeIntro -= Time.unscaledDeltaTime;
            Debug.Log(timeIntro);
        }*/

        //Hacemos que el contador de tiempo vaya decreciendo hasta que se vacíe
        if (invincibleCounter > 0)
        {
            //Usando el Time.deltatime le restamos 1 cada segundo al contador, porque le restamos las partes proporcional de ese segundo dividido en frames.
            invincibleCounter -= Time.deltaTime;//Every frame it minus a frame, so 30 fps as a second, it rest 1 every second, Want two seconds? Time.deltaTime*2, Half? Time.deltaTime*0.5f;
            if (fruittyMode)
            {
                audioBox.clip = fruittyTime;
                audioBox.loop = true;
                audioBox.Play();
                fruittyMode = false;
            }
        }
        else
        {
            if (!fruittyMode)
            {
                audioBox.clip = backgroundMusic;
                audioBox.loop = true;
                audioBox.Play();
                fruittyMode = true;
            }
        }
        if (numDots <= 0)
        {
            Win();
        }
    }

    //Este método es para inicializar el contador de tiempo invencibilidad. Al llamarlo le pasamos ese tiempo por parámetros
    public void MakeInvincibleFor(float numberOfSeconds)
    {
        //Inicializamos el contador de tiepo de invencibilidad
        invincibleCounter += numberOfSeconds;
    }

    public void ScoreUp()
    {
        numDots--;
        score += 100;
        scoreText.text = "Score: " + score.ToString();
    }

    public void ReloadScene()
    {
        //SceneManager.LoadScene("Game");
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Death()
    {
        reloadButton.SetActive(true);
        resultText.text = "Winner´nt";
        resultText.enabled = true;
        StopAudios();
        Time.timeScale = 0;
    }
    public void Win()
    {
        reloadButton.SetActive(true);
        resultText.text = "Winner";
        resultText.enabled = true;
        StopAudios();
        Time.timeScale = 0;
    }

    public void StopAudios()
    {
        foreach (AudioSource audios in allaudios)
        {
            audios.Stop();
        }
    }
}
