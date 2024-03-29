using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // - player: Lưu trữ GameObject của người chơi.
    // - CanInteract: Biến kiểm tra xem object có thể tương tác hay không.
    GameObject player { get; set; }
    bool CanInteract { get; set; }

    // - Interact(): Hàm thực hiện hành vi tương tác với object.
    void Interact();
}
