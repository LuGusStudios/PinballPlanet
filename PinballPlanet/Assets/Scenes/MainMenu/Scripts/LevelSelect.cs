using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 25), "Load halloween pinball"))
            Application.LoadLevel("Pinball_Halloween");
        if (GUI.Button(new Rect(10, 50, 150, 25), "Load pirate pinball"))
            Application.LoadLevel("Pinball_Ship");
        if (GUI.Button(new Rect(10, 100, 150, 25), "Load mine pinball"))
            Application.LoadLevel("Pinball_Mine");
    }
}
