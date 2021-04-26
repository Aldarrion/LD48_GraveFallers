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

    [Space]
    public Image SplitImage;
    public GameObject UI_P2;
    public Text[] Distance;
    public Text TimeRemaining;
    public GameObject[] LifeContainers;

    //-------------------------------
    public readonly string[] PlayerPrefixes = new string[] { "", "P2_" };
    public List<GameObject> Players { get; private set; }

    public void SetLifeCount(int player, int count)
    {
        for (int i = 0; i < LifeContainers[player].transform.childCount; ++i)
        {
            LifeContainers[player].transform.GetChild(i).gameObject.SetActive(i < count);
        }
    }

    //-------------------------------
    // Private
    //-------------------------------
    private float _timeLimit;
    private bool _isGameRunning;

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
    }

    private void Start()
    {
        // TODO this will be from UI action, not from Start()
        StartGame(2, 60);
    }

    void StartGame(int playerCount, float limitInSeconds)
    {
        Players.Clear();
        for (int i = 0; i < Cameras.Length; ++i)
        {
            Cameras[i].gameObject.SetActive(i < playerCount);
        }

        for (int i = 0; i < playerCount; ++i)
        {
            Players.Add(Instantiate(PlayerPrefab[i], Spawns[i].position, Quaternion.identity));
            Cameras[i].GetComponent<CameraController>().PlayerToFollow = Players[i].transform;
            Players[i].GetComponent<CharacterMovement>().Prefix = PlayerPrefixes[i];
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

        _isGameRunning = true;
        // TODO init countdown 3, 2, 1, GO!

        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();

        levelGenerator.StartGeneration(Players);
    }

    void UpdateTime()
    {
        _timeLimit -= Time.deltaTime;
        _timeLimit = Mathf.Max(0, _timeLimit);

        int minutes = (int)(_timeLimit / 60);
        int seconds = (int)(_timeLimit % 60);

        TimeRemaining.text = $"{minutes}:{seconds:00}";
    }

    void UpdateDistance()
    {
        for (int i = 0; i < Players.Count; ++i)
        {
            float dist = Mathf.Max(0, -Players[i].transform.position.y);
            Distance[i].text = $"{dist:0.0}m";
        }
    }

    private void Update()
    {
        if (!_isGameRunning)
            return;

        UpdateDistance();
        UpdateTime();
    }
}
