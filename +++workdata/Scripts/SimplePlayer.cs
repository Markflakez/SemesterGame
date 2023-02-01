using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}