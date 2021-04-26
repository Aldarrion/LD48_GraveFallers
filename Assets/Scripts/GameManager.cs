using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //-------------------------------
    public GameObject[] PlayerPrefab;
    public Camera[] Cameras;
    public Transform[] Spawns;
    public int StartLifeCount;
    public LevelGenerator LevelGenerator;
    public Color[] PlayerColors;
    public int SingleplayerTimeLimit;
    public int MultiplayerTimeLimit;
    public int Countdown;
    public AudioSource Music;

    [Space]
    public Image SplitImage;
    public GameObject UI_P2;
    public Text[] Distance;
    public Text TimeRemaining;
    public GameObject[] LifeContainers;
    public GameObject MenuUI;
    public Text MenuHeader;
    public Text EndMessage;
    public GameObject CreditsMessage;
    public GameObject GameOverMessage;
    public GameObject ContinueButton;
    public GameObject Hud;
    public Text CountdownText;

    //-------------------------------

    public readonly string[] PlayerPrefixes = new string[] { "P1_", "P2_" };
    public List<GameObject> Players { get; private set; }

    public bool IsGameRunning { get; private set; }

    public void SetLifeCount(int player, int count)
    {
        for (int i = 0; i < LifeContainers[player].transform.childCount; ++i)
        {
            LifeContainers[player].transform.GetChild(i).gameObject.SetActive(i < count);
        }
    }

    public void OnItemSpawned(GameObject item)
    {
        _itemsToDestroy.Add(item);
    }

    //-------------------------------
    // Private
    //-------------------------------
    private float _timeLimit;
    private float _countdownRemaining;
    private List<GameObject> _itemsToDestroy = new List<GameObject>();
    private bool _isPauseMenu;

    //-------------------------------
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Players = new List<GameObject>();
        IsGameRunning = false;
    }

    private void Start()
    {
        // TODO this will be from UI action, not from Start()
        //StartGame(2, 60);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ContinueGame()
    {
        Debug.Assert(_isPauseMenu);
        TogglePause(false);
    }

    public void StartSingleplayer()
    {
        StartGame(1, SingleplayerTimeLimit);
    }

    public void StartMultiplayer()
    {
        StartGame(2, MultiplayerTimeLimit);
    }

    public void TogglePause(bool isPaused)
    {
        if (isPaused)
            Music.Pause();
        else
            Music.UnPause();

        _isPauseMenu = isPaused;

        MenuUI.SetActive(isPaused);
        ContinueButton.SetActive(isPaused);

        CreditsMessage.SetActive(true);
        MenuHeader.gameObject.SetActive(true);
        GameOverMessage.SetActive(false);

        IsGameRunning = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;
    }

    void StartGame(int playerCount, float limitInSeconds)
    {
        Music.Stop();
        Hud.SetActive(true);
        _isPauseMenu = false;

        for (int i = 0; i < Players.Count; ++i)
        {
            Destroy(Players[i]);
        }
        Players.Clear();

        for (int i = 0; i < _itemsToDestroy.Count; ++i)
        {
            Destroy(_itemsToDestroy[i]);
        }
        _itemsToDestroy.Clear();

        for (int i = 0; i < Cameras.Length; ++i)
        {
            Cameras[i].gameObject.SetActive(i < playerCount);
        }

        for (int i = 0; i < playerCount; ++i)
        {
            Players.Add(Instantiate(PlayerPrefab[i], Spawns[i].position, Quaternion.identity));
            Cameras[i].GetComponent<CameraController>().PlayerToFollow = Players[i].transform;
            Players[i].GetComponent<CharacterMovement>().Prefix = playerCount == 1 ? "" : PlayerPrefixes[i];
            Players[i].GetComponent<CharacterLogic>().Init(i, StartLifeCount);
        }

        bool hasTwoPlayers = playerCount == 2;
        SplitImage.gameObject.SetActive(hasTwoPlayers);
        UI_P2.SetActive(hasTwoPlayers);

        if (playerCount == 1)
        {
            Cameras[0].rect = new Rect(0, 0, 1, 1);
        }
        else if (playerCount == 2)
        {
            Cameras[0].rect = new Rect(0, 0, 0.5f, 1);
            Cameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
        }

        _timeLimit = limitInSeconds;

        _countdownRemaining = Countdown + 1;

        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();

        LevelGenerator.Reset();
        levelGenerator.StartGeneration(Players);

        MenuUI.SetActive(false);
        CreditsMessage.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void UpdateTime()
    {
        _timeLimit -= Time.deltaTime;
        _timeLimit = Mathf.Max(0, _timeLimit);

        int minutes = (int)(_timeLimit / 60);
        int seconds = (int)(_timeLimit % 60);

        TimeRemaining.text = $"{minutes}:{seconds:00}";

        // Game over
        if (_timeLimit == 0)
        {
            MenuUI.SetActive(true);
            IsGameRunning = false;
            Time.timeScale = 0.0f;

            MenuHeader.gameObject.SetActive(false);
            GameOverMessage.SetActive(true);
            CreditsMessage.SetActive(false);
            EndMessage.gameObject.SetActive(true);
            if (Players.Count == 1)
            {
                EndMessage.text = $"Nice try!\nYou have fallen {GetPlayerDistance(0):0.0} meters deep";
            }
            else
            {
                float p1 = GetPlayerDistance(0);
                float p2 = GetPlayerDistance(1);

                if (p1 == p2)
                {
                    EndMessage.text = "It's a tie!\n";
                }
                else
                {
                    string winPlayer = p1 > p2 ? "one" : "two";
                    EndMessage.text = $"Player {winPlayer} won!\n";
                }
                EndMessage.text += $"Player one fell {GetPlayerDistance(0):0.0} meters deep\n" +
                        $"Player two fell {GetPlayerDistance(1):0.0} meters deep";
            }
        }
    }

    void UpdateDistance()
    {
        for (int i = 0; i < Players.Count; ++i)
        {
            Distance[i].text = GetPlayerDistanceString(i);
        }
    }

    float GetPlayerDistance(int player)
    {
        return Mathf.Max(0, -Players[player].transform.position.y);
    }

    string GetPlayerDistanceString(int player)
    {
        float dist = GetPlayerDistance(player);
        return $"{dist:0.0}m";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (IsGameRunning || _isPauseMenu))
        {
            TogglePause(!_isPauseMenu);
        }

        if (_countdownRemaining > 0)
        {
            _countdownRemaining -= Time.deltaTime;
            CountdownText.gameObject.SetActive(true);

            float frac = _countdownRemaining - (int)_countdownRemaining;
            Color c = CountdownText.color;
            c.a = frac;
            CountdownText.color = c;

            if (_countdownRemaining > 1)
            {
                int digit = Mathf.FloorToInt(_countdownRemaining);
                CountdownText.text = $"{digit}";
            }
            else
            {
                CountdownText.text = "GO!";
                IsGameRunning = true;
                Music.Play();
                Music.volume = 0.5f;
            }
        }
        else
        {
            CountdownText.gameObject.SetActive(false);
        }

        if (IsGameRunning)
        {
            UpdateDistance();
            UpdateTime();
        }
    }
}
