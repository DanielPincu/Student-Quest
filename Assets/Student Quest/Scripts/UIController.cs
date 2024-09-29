using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject canvasGame;
    [SerializeField]
    private GameObject HUD;

    [SerializeField]
    private Text txtLifes;
    [SerializeField]
    private Text txtTime;
    [SerializeField]
    private Text txtCoins;
    [SerializeField]
    private Image fillCapsule;
    [Space(10)]
    [SerializeField]
    private GameObject EndGameHolder;

    [SerializeField]
    private Text txtAllCoins;

    [SerializeField]
    private Text txtAllRedCoins;

    [SerializeField]
    private Text txtAllBricks;

    [SerializeField]
    private Text txtAllGoldenBlocks;

    [SerializeField]
    private Text txtAllTime;

    [SerializeField]
    private Text txtAllPerc;

    [Header("Events")]
    [SerializeField] private UnityEvent onGameNotStarted;
    [SerializeField] private UnityEvent onGameStarted;
    [SerializeField] private UnityEvent onStartGame;

    private static bool gameStarted = false;
    public static UIController instance;

    [SerializeField] private List<GameObject> worldCoins = new List<GameObject>();
    [SerializeField] private List<GameObject> worldRedCoins = new List<GameObject>();
    [SerializeField] private List<GameObject> worldBricks = new List<GameObject>();
    [SerializeField] private List<GameObject> worldGoldenBlocks = new List<GameObject>();

    private int moreCoins = 0;
    private int moreRedCoins = 0;

    private void Awake()
    {
        instance = this;
        Cursor.visible = false;

        txtCoins.text = "0";
        fillCapsule.fillAmount = 0;

        foreach (var brick in worldBricks)
        {
            if (brick.GetComponent<Breakable>().hitEffect)
            {
                if (brick.GetComponent<Breakable>().hitEffect.name == "CoinInBox")
                {
                    moreCoins += brick.GetComponent<Breakable>().health;
                }
                else if (brick.GetComponent<Breakable>().hitEffect.name == "CoinInBox_Red")
                {
                    moreRedCoins += 1;
                    moreCoins += 10;
                }
            }
        }

        foreach (var brick in worldGoldenBlocks)
        {
            if (brick.GetComponent<Breakable>().hitEffect)
            {
                if (brick.GetComponent<Breakable>().hitEffect.name == "CoinInBox")
                {
                    moreCoins += brick.GetComponent<Breakable>().health;
                }
                else if (brick.GetComponent<Breakable>().hitEffect.name == "CoinInBox_Red")
                {
                    moreRedCoins += 1;
                    moreCoins += 10;
                }
            }
        }

        canvasGame.SetActive(true);
    }

    private void Start()
    {
        if (!gameStarted)
        {
            onGameNotStarted.Invoke();
        }
        else
        {
            onGameStarted.Invoke();
        }
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (Input.anyKeyDown)
            {
                onStartGame.Invoke();
                enabled = false;
            }
        }
    }

    public void PauseGame(bool value)
    {
        if (value)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddCoin(int value)
    {
        txtCoins.text = value.ToString();
    }

    public void SetTime(int value)
    {
        txtTime.text = value.ToString();
    }

    public void SetLife(int value)
    {
        txtLifes.text = value.ToString();
    }

    public void Fill(float perc)
    {
        fillCapsule.fillAmount = perc;
    }

    public void ShowEndGame()
    {
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        yield return null;

        // Clear the text fields to prevent any display of previous values
        txtAllRedCoins.text = "";
        txtAllCoins.text = "";
        txtAllBricks.text = "";
        txtAllGoldenBlocks.text = "";
        txtAllTime.text = "";
        txtAllPerc.text = "";

        HUD.SetActive(false);
        GameManager.instance.PowerUpCamera();

        yield return new WaitForSeconds(1);

        Cursor.visible = true;
        GameManager.instance.EndGame();
        GameManager.instance.StopPlayer();
        EndGameHolder.SetActive(true);
    }
}
