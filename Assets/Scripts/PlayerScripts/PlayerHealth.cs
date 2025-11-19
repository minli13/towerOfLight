using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("UI Elements")]
    public GameObject healthPanel;
    public Image[] healthIcons;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public TextMeshProUGUI healthText;

    // Events
    public static event Action<int> OnHealthChanged;
    public static event Action OnPlayerDeath;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        // Update health icons and text
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (healthIcons[i] != null)
            {
                if (i < currentHealth)
                {
                    healthIcons[i].sprite = fullHeart;
                }
                else
                {
                    healthIcons[i].sprite = emptyHeart;
                }
            }
        }
        // Update health text
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    public void TakeDamage(int damageAmount = 1)
    {
        if (currentHealth <= 0) return; // Already dead

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount = 1)
    {
        if (currentHealth <= 0) return; // Can't heal if dead
        if (currentHealth >= maxHealth) return; // Already at max health
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void FullHeal()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
        Time.timeScale = 0f; // Pause the game

        // Show game over UI
        // GameOverUI.Instance.ShowGameOver();
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public bool IsFullHealth()
    {
        return currentHealth >= maxHealth;
    }
}
