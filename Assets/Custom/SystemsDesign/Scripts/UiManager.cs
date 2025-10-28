using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public Slider healthSlider;
    [SerializeField] public Text nearDeathText;

    void OnEnable()
    {
        // Subscribe to health change event
        PlayerController.OnHealthChanged += UpdateHealthUI;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        PlayerController.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(int newHealth)
    {
        if (healthSlider != null)
            healthSlider.value = newHealth;
    }

    private void NearDeath()
    {
        nearDeathText.gameObject.SetActive(true);
    }
}
