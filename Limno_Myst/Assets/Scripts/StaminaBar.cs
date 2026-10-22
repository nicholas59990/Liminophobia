using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    // The Image that visually displays the stamina amount
    [SerializeField] private Image staminaFill;
    
    [Header("Bar Colors")]
    // The normal orange color of the stamina bar
    [SerializeField] private Color normalColor = new Color(1f, 0.65f, 0f);

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
            
            // Turn the bar red when stamina is at 20% or lower
            if (targetFill <= 0.2f)
            {
                staminaFill.color = lowColor; 
            }
            else
            {
                // Keep the bar orange when stamina is above 20%
                staminaFill.color = normalColor;
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
