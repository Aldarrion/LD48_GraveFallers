using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Camera[] Cameras;
    public Transform[] Spawns;

    public static GameManager Instance { get; private set; }

    public List<GameObject> Players { get; private set; }

    string[] _prefixes = new string[] { "", "P2_" };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Players = new List<GameObject>();
    }

    private void Start()
    {
        // TODO this will be from UI action, not from Start()
        StartGame(2);
    }

    void StartGame(int playerCount)
    {
        Players.Clear();
        for (int i = 0; i < Cameras.Length; ++i)
        {
            Cameras[i].gameObject.SetActive(i < playerCount);
        }

        for (int i = 0; i < playerCount; ++i)
        {
            Players.Add(Instantiate(PlayerPrefab, Spawns[i].position, Quaternion.identity));
            Cameras[i].GetComponent<CameraController>().PlayerToFollow = Players[i].transform;
            Players[i].GetComponent<CharacterController>().Prefix = _prefixes[i];
        }

        if (playerCount == 1)
        {
            Cameras[0].rect = new Rect(0, 0, 1, 1);
        }
        else if (playerCount == 2)
        {
            Cameras[0].rect = new Rect(0, 0, 0.5f, 1);
            Cameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }
}
