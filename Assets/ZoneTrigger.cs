using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bir obje zone'a girdi: " + other.gameObject.name);
    }
}
