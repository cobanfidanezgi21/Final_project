using UnityEngine;
using UnityEngine.UI;

public class ZoneInteraction : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    private Vector3 originalScale;

    [Header("1. Görsel Ayarlar")]
    public Color interactionColor = Color.green;

    [Header("2. Ses Ayarları")]
    public AudioSource soundSource;

    [Header("3. Efektler (Kelebek İçin)")]
    public ParticleSystem particleEffect;

    [Header("4. Animasyon (Kedi İçin)")]
    public bool useScaleEffect = false;
    public float scaleAmount = 2.5f;

    [Header("5. Bilgi Kartı (İnek İçin)")]
    public GameObject infoPanel;

    // --- YENİ EKLENEN KISIM ---
    [Header("6. Özel Objeler (Süt Kutusu)")]
    public GameObject sutKutusu; // Buraya hiyerarşideki çocuğun olan Milk objesini sürükleyeceksin
    // -------------------------

    [Header("7. Puan ve Mesaj Sistemi")]
    public Text scoreText;
    public Text messageText;

    public static int totalScore = 0;
    public static int animalsTouched = 0;
    private bool hasBeenTouched = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null) originalColor = rend.material.color;
        originalScale = transform.localScale;

        if (infoPanel != null) infoPanel.SetActive(false);
        if (particleEffect != null) particleEffect.Stop();

        // --- YENİ EKLENEN KISIM ---
        // Oyun başında süt kutusu çocuğun olsa bile gizlensin
        if (sutKutusu != null) sutKutusu.SetActive(false);
        // -------------------------

        if (messageText != null && animalsTouched == 0)
            messageText.text = "Hoş Geldiniz!";

        UpdateScoreUI();
    }

    void OnTriggerEnter(Collider other)
    {
        // Hem isminde "handcursor" geçiyor mu hem de Tag'i "Player" veya "Hand" mi diye bakar
        if (other.gameObject.name.ToLower().Contains("handcursor") || other.CompareTag("Player") || other.CompareTag("Hand"))
        {
            if (rend != null) rend.material.color = interactionColor;
            if (soundSource != null) soundSource.Play();
            if (particleEffect != null) particleEffect.Play();
            if (useScaleEffect) transform.localScale = originalScale * scaleAmount;
            if (infoPanel != null) infoPanel.SetActive(true);

            // --- YENİ EKLENEN KISIM ---
            // El değdiğinde süt kutusu görünsün
            if (sutKutusu != null) sutKutusu.SetActive(true);
            // -------------------------

            // PUAN KAZANMA
            if (!hasBeenTouched)
            {
                totalScore += 10;
                animalsTouched++;
                hasBeenTouched = true;
                UpdateScoreUI();

                if (messageText != null) messageText.text = "";

                if (animalsTouched >= 3)
                {
                    if (messageText != null) messageText.text = "Tebrikler, Oyun Bitti!";
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("handcursor") || other.CompareTag("Player") || other.CompareTag("Hand"))
        {
            if (rend != null) rend.material.color = originalColor;
            if (particleEffect != null) particleEffect.Stop();
            if (useScaleEffect) transform.localScale = originalScale;
            if (infoPanel != null) infoPanel.SetActive(false);

            // İstersen elini çekince süt kutusu kaybolsun (Kalsın istersen bu satırı sil)
            if (sutKutusu != null) sutKutusu.SetActive(false);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Puan: " + totalScore;
    }
}