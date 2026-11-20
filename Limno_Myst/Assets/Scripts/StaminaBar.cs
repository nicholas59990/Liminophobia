using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    // The Image that visually displays the stamina amount
    [SerializeField] private Image staminaFill;
    
    [Header("Bar Colors")]
    // The normal orange color of the stamina bar
    [SerializeField] private Color normalColor = Color.green;

    [SerializeField] private Color mediumColor = new Color(1f, 0.65f, 0f);

    // The color used when stamina is low
    [SerializeField] private Color lowColor = Color.red;

    [Header("Animation")]
    // Controls how quickly the bar follows the actual stamina amount
    [SerializeField] private float smoothSpeed = 8f;

    // The stamina amount the bar should move toward
    private float targetFill = 1f;

    // The current displayed amount of the bar
    private float currentFill = 1f;

    private void Update()
    {
        // Smoothly move the displayed amount toward the target amount
        currentFill = Mathf.Lerp(
            currentFill,
            targetFill,
            smoothSpeed * Time.deltaTime
        );

        // Update how much of the bar is filled
        if (staminaFill != null)
        {
            staminaFill.fillAmount = currentFill;
            
            // Blend from Green to Orange
            if (currentFill > 0.55f)
            {
                float texture = Mathf.InverseLerp(1f, 0.55f, currentFill);
                staminaFill.color = Color.Lerp(normalColor, mediumColor, texture); 
            }
            else
            {
                // Blend from Orange to Red
                float texture = Mathf.InverseLerp(0.55f, 0f, currentFill);
                staminaFill.color = Color.Lerp(mediumColor, lowColor, texture);
            }
        }
    }

    // Receives the current and max stamina from PlayerMovement
    public void SetStamina(float current, float max)
    {
        // Prevent divison by zero
        if (max <= 0f) {
            return;
        }

        // Convert stamina into a value between 0 and 1 for the bar
        targetFill = Mathf.Clamp01(current / max);
    }
}
