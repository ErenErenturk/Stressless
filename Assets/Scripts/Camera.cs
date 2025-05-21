using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    public Transform oyuncu;
    public Vector3 offset = new Vector3(0, 0, -10); // Kamera oyuncudan ne kadar uzak olsun?

    void LateUpdate()
    {
        if (oyuncu != null)
        {
            transform.position = oyuncu.position + offset;
        }
    }
}

