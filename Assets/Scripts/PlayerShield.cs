using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public int shieldHealth = 3;
    public GameObject shieldVisual;

    private void Start()
    {
        UpdateShieldVisual();
    }

    public bool AbsorbHit()
    {
        if (shieldHealth > 0)
        {
            shieldHealth--;
            UpdateShieldVisual();
            return true;
        }

        return false;
    }

    private void UpdateShieldVisual()
    {
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(shieldHealth > 0);
        }
    }

    public void ResetShield()
    {
        shieldHealth = 3;
        UpdateShieldVisual();
    }
}
