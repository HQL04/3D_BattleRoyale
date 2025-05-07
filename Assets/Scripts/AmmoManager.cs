using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; set; }
    public Text ammoDisplay;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
