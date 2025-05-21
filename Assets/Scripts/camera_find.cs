using UnityEngine;

public class ForceCameraToPlayer : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, -6f);
        }
    }
}
