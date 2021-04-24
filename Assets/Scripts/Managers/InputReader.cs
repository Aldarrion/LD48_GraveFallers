using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader
{
    private string _playerPrefix;

    private HashSet<InputAction> activeActions = new HashSet<InputAction>();

    float horizontalSum = 0;
    int horizontalCount = 0;

    public InputReader(string playerPrefix)
    {
        _playerPrefix = playerPrefix;
    }

    public void Update()
    {
        float horizontal = Input.GetAxis(_playerPrefix + "Horizontal");

        horizontalSum += horizontal;
        horizontalCount += 1;

        if (Input.GetButtonDown(_playerPrefix + "Jump"))
        {
            activeActions.Add(InputAction.Jump);
        }
    }

    public bool IsAction(InputAction inputAction)
    {
        return activeActions.Contains(inputAction);
    }

    public float GetHorizontal()
    {
        return (horizontalCount == 0 ? 0 : horizontalSum / horizontalCount);
    }

    public void Reset()
    {
        activeActions.Clear();
        horizontalSum = 0;
        horizontalCount = 0;
    }
}
