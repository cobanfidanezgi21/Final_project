using UnityEngine;
using UnityEngine.Android; // Android kütüphanesini çaðýrýyoruz

public class KameraIzniKontrol : MonoBehaviour
{
    void Start()
    {
        // Eðer kullanýcý daha önce izin vermediyse, izin iste
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }
}
