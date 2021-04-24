using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] Compositions;

    List<GameObject> _chunks = new List<GameObject>();
    Vector3 _lastPos = Vector3.zero;

    void SpawnChunk()
    {
        var chunk = Instantiate(Compositions[Random.Range(0, Compositions.Length)]);
        _chunks.Add(chunk);
        chunk.transform.position = _lastPos;
        _lastPos.y -= 16;
    }

    void Start()
    {
        for (int i = 0; i < 3; ++i)
            SpawnChunk();
    }

    private void Update()
    {
        _chunks.RemoveAll(chunk =>
        {
            foreach (var player in GameManager.Instance.Players)
            {
                if (chunk.transform.position.y - player.transform.position.y < 16)
                    return false;
            }
            Destroy(chunk);
            return true;
        });

        foreach (var player in GameManager.Instance.Players)
        {
            if (player.transform.position.y - _chunks[_chunks.Count - 1].transform.position.y < 64)
            {
                SpawnChunk();
                break;
            }
        }
    }
}
