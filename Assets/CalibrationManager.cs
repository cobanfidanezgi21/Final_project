using UnityEngine;

public class CalibrationManager : MonoBehaviour
{
    public GameObject calibrationPoints; // Noktalarin oldugu kutu

    void Update()
    {
        // Bilgisayarda test ederken 'C' tusuna basarsan calisir
        if (Input.GetKeyDown(KeyCode.C))
        {
            TogglePoints();
        }
    }

    // Bu fonksiyonu telefondaki butona baglayacagiz
    public void TogglePoints()
    {
        // Açýksa kapatýr, kapalýysa açar (Tersini yapar)
        bool durum = calibrationPoints.activeSelf;
        calibrationPoints.SetActive(!durum);
    }
}