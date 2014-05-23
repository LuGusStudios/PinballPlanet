using UnityEngine;

public class TextureScroll : MonoBehaviour
{

    public float ScrollMultiplyer = 0.5f;
    public bool Horizontal = false;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 offset = rend.material.mainTextureOffset;
        if (Horizontal == false)
        {
            offset.y = Time.time * ScrollMultiplyer;
        }
        else
        {
            offset.x = Time.time * ScrollMultiplyer;
        }

        rend.material.mainTextureOffset = offset;

    }
}
