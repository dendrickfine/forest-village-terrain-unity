using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public float offsetX = 0f; // Tambahkan offset X jika perlu

    public void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.x += offsetX; // Tambahkan offset X
        newPosition.y = transform.position.y; // Tetap di atas
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
