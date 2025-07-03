using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float scrollSpeed = 5f; // Kecepatan pergerakan background
    public float resetPositionZ = -30f; // Posisi Z di mana background direset
    public float startPositionZ = 30f; // Posisi awal setelah direset

    void Update()
    {
        // Geser background ke belakang
        transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime);

        // Jika posisi background melewati batas, pindahkan ke awal
        if (transform.position.z <= resetPositionZ)
        {
            ResetBackground();
        }
    }

    private void ResetBackground()
    {
        Vector3 newPosition = transform.position;
        newPosition.z = startPositionZ; // Pindahkan background kembali ke depan
        transform.position = newPosition;
    }
}
