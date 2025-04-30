// PlayerHealth.cs
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    [SerializeField] private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void PlayerTakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0); // Zapobiega spadkowi zdrowia poni¿ej 0
        Debug.Log("Player took damage: " + amount + " | Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player died!");
        if (animator != null)
        {
            animator.SetTrigger("Dematerialize"); // Uruchamia animacjê dematerializacji
        }
        StartCoroutine(ReloadLevelAfterDelay(3f));
    }

    IEnumerator ReloadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
