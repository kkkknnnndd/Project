using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractionBase : MonoBehaviour, IInteractable
{
    // Tham chiếu đến player
    public GameObject player { get; set; }

    // Kiểm tra xem có thể tương tác hay không
    public bool CanInteract { get; set; }

    private void Start()
    {
        // Tìm kiếm GameObject player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Nếu có thể tương tác và phím tương tác được bấm
        if (CanInteract && NozoCharacterController.WasInteractPressed)
        {
            // Thực hiện hành vi tương tác
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Nếu player va chạm vào trigger
        if (other.gameObject == player)
        {
            // Thiết lập khả năng tương tác
            CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Nếu player thoát khỏi trigger
        if (other.gameObject == player)
        {
            // Tháo bỏ khả năng tương tác
            CanInteract = false;
        }
    }

    // Hàm tương tác, cần được triển khai bởi các class kế thừa
    public virtual void Interact() { }
}

