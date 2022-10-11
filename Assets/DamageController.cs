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

    public float deltaTimeCounter = 1.5f;

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
            if (deltaTimeCounter >= 1.5f)
            {
                isVulnerable = true;
            }
            else
            {
                switch(System.Math.Round(deltaTimeCounter, 2)){
                    case 0:
                        sprite.color = Color.red;
                        break;
                    case 0.25:
                        sprite.color = Color.white;
                        break;
                    case 0.5:
                        sprite.color = Color.red;
                        break;
                    case 0.75:
                        sprite.color = Color.white;
                        break;
                    case 1:
                        sprite.color = Color.red;
                        break;
                    case 1.25:
                        sprite.color = Color.white;
                        break;
                }
                // double remainder = deltaTimeCounter % 0.25;
                // double quotient = deltaTimeCounter / 0.25;
                // if (remainder == 0)
                // {
                //     if (quotient % 2 == 0)
                //     {
                //         sprite.color = Color.red;
                //         Debug.Log("delatime" + deltaTimeCounter);
                //     }
                //     else
                //     {
                //         sprite.color = Color.white;
                //     }
                // }
            }
        }
        deltaTimeCounter += Time.deltaTime;
        Debug.Log(deltaTimeCounter);
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
        if (deltaTimeCounter >= 1.5f) {
            Damage();
            deltaTimeCounter = 0f;
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
