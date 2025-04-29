using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Canvas Settings")]
    public Canvas canvasPrefab;          // Prefab Canvas zawieraj¹cy Slider
    private Canvas canvasInstance;
    [SerializeField] private Slider healthSlider; // Referencja do Slidera

    public float verticalOffset = 2f;

    private EnemyPatrol enemyPatrol;
    private Camera mainCamera;

    void Start()
    {
        enemyPatrol = GetComponent<EnemyPatrol>();
        mainCamera = Camera.main;

        if (enemyPatrol != null)
        {
            float initialHealth = enemyPatrol.GetHealth();

            // Tworzenie Canvasu jako niezale¿nego obiektu w scenie
            Vector3 spawnPosition = transform.position;
            canvasInstance = Instantiate(canvasPrefab, spawnPosition, Quaternion.identity);
            canvasInstance.transform.SetParent(null); // Od³¹czenie od hierarchii przeciwnika
            canvasInstance.renderMode = RenderMode.WorldSpace;
            canvasInstance.transform.localScale = Vector3.one;

            RectTransform canvasRect = canvasInstance.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(2, 0.5f);
            canvasRect.position = spawnPosition; // Ustawienie pozycji Canvasu bez offsetu

            // Pobranie referencji do Slidera
            healthSlider = canvasInstance.GetComponentInChildren<Slider>();
            if (healthSlider != null)
            {
                // Przesuniêcie Slidera o verticalOffset
                RectTransform sliderRect = healthSlider.GetComponent<RectTransform>();
                sliderRect.anchoredPosition += new Vector2(0, verticalOffset);

                healthSlider.maxValue = initialHealth;
                healthSlider.value = initialHealth;
            }
            else
            {
                Debug.LogError("Slider component not found in canvasPrefab.");
            }

            enemyPatrol.OnTakeDamage += UpdateHealth;
            Debug.Log("HealthBar - Canvas i Slider za³adowane poprawnie.");
        }
        else
        {
            Debug.LogError("EnemyPatrol component not found.");
        }
    }

    void LateUpdate()
    {
        if (enemyPatrol != null && canvasInstance != null && mainCamera != null)
        {
            // Aktualizacja pozycji Canvasu nad przeciwnikiem bez offsetu
            Vector3 worldPosition = transform.position;
            canvasInstance.transform.position = worldPosition;

            // Ustawienie rotacji Canvasu, aby patrzy³ na kamerê
            canvasInstance.transform.LookAt(canvasInstance.transform.position + mainCamera.transform.rotation * Vector3.forward,
                                           mainCamera.transform.rotation * Vector3.up);
        }
    }

    // Szybkie update ¿ycia przeciwnika 
    private void UpdateHealth(float currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            Debug.Log($"HealthBar Updated: {currentHealth}");
        }
    }

    void OnDestroy()
    {
        if (enemyPatrol != null)
        {
            enemyPatrol.OnTakeDamage -= UpdateHealth;
        }

        if (canvasInstance != null)
        {
            Destroy(canvasInstance.gameObject);
        }
    }
}
