using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int RowSize = 10;

    public int RowsAbove = 10;
    public int RowsBellow = 30;

    public int RowDestructionRange = 10;

    private Dictionary<CharacterMovementController, Interval> intervals;

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

    public void StartGeneration()
    {
        foreach (GameObject player in GameManager.Instance.Players)
        {
            CharacterMovementController characterController = player.GetComponent<CharacterMovementController>();
            if (characterController != null)
            {
                int currentRow = PositionToRow(characterController.transform.position.y);

                Interval interval = new Interval();
                interval.From = currentRow - RowsAbove;
                interval.To = currentRow + RowsBellow;

                intervals.Add(characterController, interval);

                for (int i= interval.From; i<=interval.To; i++)
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

            if (interval.To < currentRow + RowsBellow)
            {
                for (int i=interval.To + 1; i < currentRow + RowsBellow; i++)
                {
                    SpawnRow(i);
                    interval.To = currentRow + RowsBellow;
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
