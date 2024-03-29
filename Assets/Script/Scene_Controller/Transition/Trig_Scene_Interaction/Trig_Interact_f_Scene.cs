using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trig_Interact_f_Scene : MonoBehaviour
{
    // Tham chiếu đến GameObject hiển thị chữ "Loading"
    [SerializeField] private GameObject Loading_Text;

    // Biến đánh dấu trạng thái đang loading (có thể là đang chuyển scene)
    private bool _loadingTime;

    private void Awake()
    {
        // Hàm Awake được gọi khi object được kích hoạt
        if (!_loadingTime) // Kiểm tra xem _loadingTime là false
        {
            Loading_Text.SetActive(false); // Tắt chữ "Loading"
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _loadingTime) // Kiểm tra nếu F được bấm và _loadingTime là false
        {
            _loadingTime = true; // Thiết lập _loadingTime sang true (bắt đầu loading)
            NozoCharacterController.WasInteractPressed = true; // Thiết lập cờ tương tác trong
                                                               // NozoCharacterController (có thể kích hoạt hành vi tương tác)
        }
    }

    private void OnTriggerStay(Collider other)
    {
        _loadingTime = true;
        // Hàm OnTriggerStay được gọi khi collider khác giữ trên trigger collider này

        if (_loadingTime) // Kiểm tra xem _loadingTime là false
        {
            Loading_Text.SetActive(true); // Bật chữ "Loading"
        }
        else // Ngược lại (_loadingTime là true)
        {
            Loading_Text.SetActive(false); // Tắt chữ "Loading"
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        // Hàm OnTriggerExit được gọi khi collider khác thoát khỏi trigger collider này

        _loadingTime = false; // Reset _loadingTime về false (dừng loading)
        Loading_Text.SetActive(false); // Tắt chữ "Loading"
        NozoCharacterController.WasInteractPressed = false; // Reset cờ tương tác trong NozoCharacterController
    }
}

