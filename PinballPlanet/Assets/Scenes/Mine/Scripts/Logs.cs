using UnityEngine;
using System.Collections;

public class Logs : Breakable
{
    // Seperate log subobjects.
    private GameObject _log1, _log2, _log3;

    // Index of currently broken subobject.
    private int _breakIndex = 0;

    // Time till reset.
    public float ResetTime = 5;

    // Use this for initialization
    protected override void Start()
    {
        // Store child gameobjects.
        _log1 = transform.FindChild("Log01").gameObject;
        _log2 = transform.FindChild("Log02").gameObject;
        _log3 = transform.FindChild("Log03").gameObject;

        base.Start();
    }

    // Breaks the game object.
    public override void Break()
    {
        // Return if already broken.
        if (IsBroken)
            return;

        // Break each part seperately with each consecutive hit.
        switch (_breakIndex)
        {
            case 0:
                // Disable log.
                _log1.SetActive(false);
                ++_breakIndex;
                break;
            case 1:
                // Disable log.
                _log2.SetActive(false);
                ++_breakIndex;
                break;
            case 2:
                // Disable log.
                _log3.SetActive(false);
                // Call base Break.
                base.Break();
                break;
        }
    }

    // Restores the game object to its unbroken state.
    public override void Unbreak()
    {
        // Reset index.
        _breakIndex = 0;
        // Enable logs.
        _log1.SetActive(true);
        _log2.SetActive(true);
        _log3.SetActive(true);

        //// Play grow animation.
        //animation.Play("CrystalGrow");

        // Call base Unbreak.
        base.Unbreak();
    }

}
