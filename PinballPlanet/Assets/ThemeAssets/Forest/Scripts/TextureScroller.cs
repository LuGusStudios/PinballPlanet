using UnityEngine;
using System.Collections;

public class TextureScroller : MonoBehaviour 
{

    public float scrollMultiplier = 0.5f;
    public bool horizontal = false;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 offset = rend.material.mainTextureOffset;

        if (horizontal == false)
        {
            offset.y = Time.time * scrollMultiplier;
        }
        else
        {
            offset.x = Time.time * scrollMultiplier;
        }

        rend.material.mainTextureOffset = offset;

    }
}
