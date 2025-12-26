using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class HandCursorController : MonoBehaviour
{
    // --- INSPECTOR AYARLARI ---
    public int port = 5052; 
    public float sensitivity = 15.0f; // Hassasiyet
    public float smoothing = 5.0f;    // Yumusaklik

    // --- OZEL DEGISKENLER ---
    Thread receiveThread;
    UdpClient client;
    Vector3 targetPosition;
    bool dataReceived = false;

    void Start()
    {
        targetPosition = transform.position;
        
        // Arka planda dinlemeyi baslat
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);

                string[] coordinates = text.Split(',');
                
                // Koordinatlari cevir
                float x = float.Parse(coordinates[0], CultureInfo.InvariantCulture);
                float y = float.Parse(coordinates[1], CultureInfo.InvariantCulture);

                // Unity Dunyasina Uyarlamak
                float unityX = (x - 0.5f) * sensitivity;
                float unityY = ((1 - y) - 0.5f) * sensitivity;

                targetPosition = new Vector3(unityX, unityY, 0);
                dataReceived = true;
            }
            catch (System.Exception err)
            {
                // Hatalari yut, oyunu durdurma
            }
        }
    }

    void Update()
    {
        // Hedefe yumusakca git
        if (dataReceived)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothing);
        }
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null) receiveThread.Abort();
        if (client != null) client.Close();
    }
}