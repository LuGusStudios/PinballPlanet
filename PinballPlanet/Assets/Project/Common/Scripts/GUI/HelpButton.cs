using UnityEngine;

public class HelpButton : MonoBehaviour
{
    protected Camera uiCamera = null;

    // Use this for initialization
    void Start()
    {
        uiCamera = GameObject.Find("UICamera").camera;
    }

    // Update is called once per frame
    void Update()
    {
        Transform t = LugusInput.use.RayCastFromMouseDown(uiCamera);

        if (t == this.transform)
        {
            //ShareScore();
            GameObject.Find("JESUS").GetComponent<UIGameController>().ShowPauzeMenu();
        }
    }
}
