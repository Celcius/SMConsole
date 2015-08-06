using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {

	// Use this for initialization
    void Start()
    {

        SMConsole.Log("This is a normal tagless log");
        SMConsole.Log("This is a normal tagged log", "TAG");
        SMConsole.Log("This is a different normal tagged log", "TAG2");

        SMConsole.LogWarning("This is a warning tagless log");
        SMConsole.Log("This is a warning tagged log", "WARNING_TAG", SMLogType.WARNING);
        SMConsole.LogWarning("This is a different warning tagged log", "WARNING_TAG2");



        SMConsole.LogError("This is an error tagless log");
        SMConsole.Log("This is an error tagged log", "ERROR_TAG", SMLogType.ERROR);
        SMConsole.LogError("This is a different error tagged log", "ERROR_TAG2");

        Debug.Log("YUP");
        Debug.LogWarning("KINDA");
    }

    void Update()
    {
        Debug.Log("YUP");
        Debug.LogWarning("KINDA");
    }
}
