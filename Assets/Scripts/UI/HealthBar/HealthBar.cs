
using UnityEngine;

using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image barImage;
    private void Awake()
    {
        barImage = transform.Find("healthBar").GetComponent<Image>();
    }
    public void SetHealthBar(float currentHealth, float maxHealth)
    {
        barImage.fillAmount = currentHealth / maxHealth;
    }


}
