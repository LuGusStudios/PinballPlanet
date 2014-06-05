using UnityEngine;
using System.Collections;

public class FadingButton : MonoBehaviour
{
    public float FadeTime = 5.0f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(FadingRoutine());
    }

    protected IEnumerator FadingRoutine()
    {
        yield return new WaitForSeconds(FadeTime);

        gameObject.SetActive(false);
    }
}
