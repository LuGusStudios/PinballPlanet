using UnityEngine;
using System.Collections;

public class MultiBallResetter : MonoBehaviour
{
    private MultiBall _multiball;

    // Use this for initialization
    void Start()
    {
        _multiball = GetComponent<MultiBall>();
    }

    // Update is called once per frame
    void Update()
    {
        // Activate multiball if disabled and only a single ball is in play.
        if (!_multiball.Activated)
            if (Player.use.BallsInPlay.Count == 1)
                _multiball.ActivateMultiBall();
    }
}
