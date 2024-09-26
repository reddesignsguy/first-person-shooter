
// Note: Make this a singleton because item refernces are always gonna be the same
using UnityEngine;

public class ItemContext : MonoBehaviour
{

    public static ItemContext Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject _itemHeld { get; set; }
    public GameObject _itemLookingAt { get; set; }
}