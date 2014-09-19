using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public Transform Target = null;

    // Update is called once per frame
    void Update()
    {
        if(Target != null)
            transform.LookAt(Target);
    }
}
