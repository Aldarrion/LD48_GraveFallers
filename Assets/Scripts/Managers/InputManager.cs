using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private List<InputReader> inputReaders = new List<InputReader>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (InputReader inputReader in inputReaders) {

        }
    }
}
