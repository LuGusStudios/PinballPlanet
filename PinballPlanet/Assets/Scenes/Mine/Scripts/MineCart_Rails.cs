using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the mine cart and rails.
/// </summary>
public class MineCart_Rails : MonoBehaviour
{
    // Prefab of the mine cart.
    public GameObject MineCartPrefab;

    // Light that controls the rail switch.
    public FloorLight_Toggle SwitchLight;

    public bool RailsSwitched
    {
        get { return SwitchLight.IsBroken; }
    }

    // Use this for initialization
    void Start()
    {
        SpawnMineCart();
    }

    public void OnMineCartDestroyed()
    {
        SpawnMineCart();
    }

    private void SpawnMineCart()
    {
        Instantiate(MineCartPrefab);
    }
}
