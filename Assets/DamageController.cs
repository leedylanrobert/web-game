using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Collider2D spiderCollider;

    public SpawnEnemy SpawnEnemy;

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if (spiderCollider.IsTouching(enemy.GetComponent<Collider2D>())) {
                Collision();
                break;
            }
            
        }

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
    }
    
    private void Damage()
    {
        if (_healthController.playerHealth > 1)
        {
            AudioSource.PlayClipAtPoint(damage, transform.position, volume);
            isVulnerable = false;

            _healthController.playerHealth = _healthController.playerHealth - enemyDamage;
            _healthController.UpdateHealth();
        }
        else if (_healthController.playerHealth == 1)
        {
            AudioSource.PlayClipAtPoint(loss, transform.position, volume);

            Purge();

            restartPanel.SetActive(true);
        }
    }

    private void Collision() {
        if (interval >= iFrameAmount) {
            Damage();
            interval = 0;
        }
    }

    private void Purge() {

        SpawnEnemy.keepSpawning = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        GameObject[] countdowns = GameObject.FindGameObjectsWithTag("Countdown");
        foreach (var countdown in countdowns)
        {
            Destroy(countdown);
        }
        for (int i=0; i < _healthController.hearts.Length; i++)
        {
            _healthController.hearts[i].enabled = false;
        }
        Destroy(GameObject.FindGameObjectWithTag("Hearts"));
        Destroy(GameObject.FindGameObjectWithTag("Spider"));

    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     // if(collision & interval >= iFrameAmount)

    //     if(collision)
    //     {
    //         Damage();
    //     }

    //     void Damage()
    //     {
    //         if (_healthController.playerHealth > 0)
    //         {
    //             AudioSource.PlayClipAtPoint(damage, transform.position, volume);
    //             isVulnerable = false;
    //         }
    //         if (_healthController.playerHealth == 1)
    //         {
    //             AudioSource.PlayClipAtPoint(loss, transform.position, volume);
    //             restartPanel.SetActive(true);
    //         }
    //         _healthController.playerHealth = _healthController.playerHealth - enemyDamage;
    //         _healthController.UpdateHealth();
    //     }
    // }
}
