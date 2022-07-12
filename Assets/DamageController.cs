using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private int enemyDamage;
    
    [SerializeField] private HealthController _healthController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Spider"))
        {
            Damage();
        }

        void Damage()
        {
            _healthController.playerHealth = _healthController.playerHealth - enemyDamage;
            _healthController.UpdateHealth();
        }
    }
}
