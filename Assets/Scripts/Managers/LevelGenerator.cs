using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public const int ROW_SKIP = 3;

    public int RowSize = 1;

    public int RowsAbove = 10;
    public int RowsBellow = 30;

    public int RowDestructionRange = 10;

    public GameObject NormalPlatform;
    public GameObject SpikyPlatform;

    private Dictionary<CharacterMovementController, Interval> intervals = new Dictionary<CharacterMovementController, Interval>();

    private Dictionary<int, List<GameObject>> rowObjects = new Dictionary<int, List<GameObject>>();

    private bool _started = false;

    private class Interval
    {
        public int From;
        public int To;
    }


    public void Reset()
    {
        intervals.Clear();
        foreach (List<GameObject> gameObjects in rowObjects.Values)
        {
            foreach(GameObject gameObject in gameObjects)
            {
                Destroy(gameObject);
            }
        }
        rowObjects.Clear();
    }

    public void Start()
    {
        StartGeneration();
    }

    public void StartGeneration()
    {


        foreach (CharacterMovementController player in FindObjectsOfType<CharacterMovementController>())
        {
            CharacterMovementController characterController = player.GetComponent<CharacterMovementController>();
            if (characterController != null)
            {
                int currentRow = PositionToRow(characterController.transform.position.y);

                Interval interval = new Interval();
                interval.From = currentRow + RowsAbove;
                interval.To = currentRow - RowsBellow;

                intervals.Add(characterController, interval);

                for (int i = interval.From; i>=interval.To; i--)
                {
                    SpawnRow(i);
                }
            }
        }

        _started = true;
    }

    void Update()
    {
        if (!_started)
        {
            return;
        }

        foreach (CharacterMovementController characterController in intervals.Keys)
        {
            Interval interval = intervals[characterController];

            int currentRow = PositionToRow(characterController.transform.position.y);

            if (interval.To > currentRow - RowsBellow)
            {
                for (int i =interval.To - 1; i > currentRow - RowsBellow; i--)
                {
                    SpawnRow(i);
                    interval.To = currentRow - RowsBellow;
                }
            }
        }
    }

    void SpawnRow(int rowId)
    {
        if (rowObjects.ContainsKey(rowId))
        {
            // already spawned
            return;
        }

        List<GameObject> gameObjects = new List<GameObject>();

        rowObjects.Add(rowId, gameObjects);

        // generate row
        GenerateRow(rowId, gameObjects);
    }

    void GenerateRow(int rowId, List<GameObject> gameObjects)
    {
        System.Random random = new System.Random(rowId * 967);

        //if (rowId % 2 != 0)
        //{
        //    int position = random.Next(-4, 4);

        //    Instantiate(NormalPlatform, new Vector3(RowToPosition(position-2), RowToPosition(rowId)), Quaternion.identity);
        //    Instantiate(NormalPlatform, new Vector3(RowToPosition(position), RowToPosition(rowId)), Quaternion.identity);
        //    Instantiate(NormalPlatform, new Vector3(RowToPosition(position+2), RowToPosition(rowId)), Quaternion.identity);
        //}

        if (rowId % 2 != 0)
        {
            int startSkip = 0;
            int skip;

            int probabilityToSpawn = Mathf.Max(random.Next(0, 20), 0) + Mathf.Max(random.Next(0, 20), 0) + Mathf.Max(random.Next(-10, 10), 0);

            if (random.Next(0, 100) < probabilityToSpawn)
            {
                SpawnObject(NormalPlatform, 0, rowId, gameObjects);
                startSkip = ROW_SKIP;
            }

            skip = startSkip;

            int firstWay = random.Next() > 0 ? 1 : -1;

            for (int i = 1; i <= 6; i++)
            {
                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                if (i < ROW_SKIP)
                {
                    startSkip = ROW_SKIP - i;
                }


                if (random.Next(0, 100) < probabilityToSpawn)
                {
                    SpawnObject(NormalPlatform, i * firstWay, rowId, gameObjects);
                    skip = ROW_SKIP;
                }

            }

            skip = startSkip;

            for (int i = 1; i <= 6; i++)
            {
                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                if (random.Next(0, 100) < probabilityToSpawn)
                {
                    SpawnObject(NormalPlatform, i * -firstWay, rowId, gameObjects);
                    skip = ROW_SKIP;
                }

            }
        } 
        else
        {
            // filler rows
        }
    }

    void SpawnObject(GameObject prefab, int positionInRow, int rowNumber, List<GameObject> gameObjects)
    {
        GameObject gameObject = Instantiate(prefab, new Vector3(RowToPosition(positionInRow), RowToPosition(rowNumber)), Quaternion.identity);
        gameObjects.Add(gameObject);
    }

    void DestroyRow(int rowId)
    {
        List<GameObject> gameObjects;
        
        if (!rowObjects.TryGetValue(rowId, out gameObjects))
        {
            // does not exist
            return;
        }

        foreach (GameObject gameObject in gameObjects)
        {
            Destroy(gameObject);
        }
    }


    int PositionToRow(float position)
    {
        return (int)(position / RowSize);
    }

    float RowToPosition(int row)
    {
        return row * RowSize;
    }
}
