
using UnityEngine;

public class TrapsDamage : MonoBehaviour
{
    AttackDetails attackDetails;

    [SerializeField]
    private float trapsDamageCooldown = 4f;
    private float lastDamageTime = float.NegativeInfinity;
    private void OnCollisionStay2D(Collision2D other)
    {

        if (other.gameObject.name == "Player")
        {
            if (Time.time >= lastDamageTime + trapsDamageCooldown)
            {
                attackDetails.damageAmount = 10f;
                attackDetails.upDirection = true;
                other.gameObject.SendMessage("Damage", attackDetails);
                lastDamageTime = Time.time;
            }

        }
        else
        {
            attackDetails.damageAmount = 5f;
            other.gameObject.transform.parent.SendMessage("Damage", attackDetails);
        }

    }
}
