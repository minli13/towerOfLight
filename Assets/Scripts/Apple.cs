using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public int healAmount = 1;
    public TextMeshProUGUI interactionMessage;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ConsumeApple(other.gameObject);
        }
    }

    private void ConsumeApple(GameObject player)
    {
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.Heal(healAmount);
            Destroy(gameObject);
            interactionMessage.text = "You consumed an apple and restored " + healAmount + " health.";
        }
    }
}
