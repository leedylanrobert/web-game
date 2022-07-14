using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private int enemyDamage;
    
    [SerializeField] private HealthController _healthController;

    private FollowTouch spider;

    private int interval = 90;
    private int iFrameAmount = 90;

    public GameObject restartPanel;

    public AudioClip loss;
    public AudioClip damage;
    float volume = 1;

    public SpriteRenderer sprite;
    bool isVulnerable = true;

    void Update()
    {
        if (!isVulnerable)
        {
            if (interval >= iFrameAmount)
            {
                isVulnerable = true;
            }
            else
            {
                int remainder = interval % 15;
                int quotient = interval / 15;
                if (remainder == 0)
                {
                    if (quotient % 2 == 0)
                    {
                        sprite.color = Color.red;
                    }
                    else
                    {
                        sprite.color = Color.white;
                    }
                }
            }
        }
        interval += 1;
        Debug.Log("interval: " + interval);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision & interval >= iFrameAmount)
        {
            Damage();
            interval = 0;
        }

        void Damage()
        {
            if (_healthController.playerHealth > 0)
            {
                AudioSource.PlayClipAtPoint(damage, transform.position, volume);
                isVulnerable = false;
            }
            if (_healthController.playerHealth == 1)
            {
                AudioSource.PlayClipAtPoint(loss, transform.position, volume);
                restartPanel.SetActive(true);
            }
            _healthController.playerHealth = _healthController.playerHealth - enemyDamage;
            _healthController.UpdateHealth();
        }
    }
}
