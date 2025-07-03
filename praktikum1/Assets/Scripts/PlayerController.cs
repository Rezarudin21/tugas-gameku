using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 30f; // Kecepatan perpindahan mobil
    private float laneWidth = 3.5f; // Jarak perpindahan antar jalur
    private float swipeThreshold = 50f; // Jarak minimal swipe agar terdeteksi
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool isSwiping = false;
    private int currentLane = 0; // Posisi jalur saat ini (-1, 0, 1)

    void Update()
    {
        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        // Untuk testing di Unity Editor (gunakan mouse)
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            touchEndPos = Input.mousePosition;
            DetectSwipe();
            isSwiping = false;
        }
        #else
        // Untuk perangkat mobile (gunakan touch input)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                touchEndPos = touch.position;
                DetectSwipe();
                isSwiping = false;
            }
        }
        #endif
    }

    private void DetectSwipe()
    {
        float swipeDistanceX = touchEndPos.x - touchStartPos.x;

        if (Mathf.Abs(swipeDistanceX) > swipeThreshold)
        {
            if (swipeDistanceX > 0 && currentLane < 1) // Swipe kanan
            {
                currentLane++;
            }
            else if (swipeDistanceX < 0 && currentLane > -1) // Swipe kiri
            {
                currentLane--;
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    
}
