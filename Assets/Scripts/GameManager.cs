using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    public static GameManager Instance { get; private set; }
    
    public List<GameObject> Players { get; private set; }

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
        StartGame();
    }

    void StartGame()
    {
        Players.Clear();
        Players.Add(Instantiate(PlayerPrefab, new Vector3(0, 6), Quaternion.identity));
        Camera.main.GetComponent<CameraController>().PlayerToFollow = Players[0].transform;
    }
}
