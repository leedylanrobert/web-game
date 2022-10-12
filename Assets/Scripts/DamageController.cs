using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageController : MonoBehaviour
{
    [SerializeField] private int enemyDamage;
    
    [SerializeField] private HealthController _healthController;

    private FollowTouch spider;

    public GameObject restartPanel;

    public AudioClip loss;
    public AudioClip damage;
    float volume = 1;

    public SpriteRenderer sprite;
    bool isVulnerable = true;

    public Collider2D spiderCollider;

    public SpawnEnemy SpawnEnemy;

    public float deltaTimeCounter = 1.5f;

    AudioSource backgroundMusic;

    void Start()
    {
        backgroundMusic = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckCollisions();
        HandleiFrames();

        deltaTimeCounter += Time.deltaTime;
    }

    private void HandleiFrames()
    {
        if (!isVulnerable)
        {
            if (deltaTimeCounter >= 1.5f)
            {
                isVulnerable = true;
            }
            else
            {
                double newDeltaTimeCounter = System.Math.Round(deltaTimeCounter, 2);
                if ((newDeltaTimeCounter > 0 & newDeltaTimeCounter <= 0.25) | (newDeltaTimeCounter > 0.5 & newDeltaTimeCounter <= 0.75) | (newDeltaTimeCounter > 1.0 & newDeltaTimeCounter <= 1.25)){
                    sprite.color = Color.red;
                } else if ((newDeltaTimeCounter > 0.25 & newDeltaTimeCounter <= 0.5) | (newDeltaTimeCounter > 0.75 & newDeltaTimeCounter <= 1.0) | (newDeltaTimeCounter > 1.25 & newDeltaTimeCounter <= 1.5)){
                    sprite.color = Color.white;
                }
            }
        }
    }

    private void CheckCollisions()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if (spiderCollider.IsTouching(enemy.GetComponent<Collider2D>())) {
                Collision();
                break;
            }
            
        }
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
            backgroundMusic.Pause();
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
}
