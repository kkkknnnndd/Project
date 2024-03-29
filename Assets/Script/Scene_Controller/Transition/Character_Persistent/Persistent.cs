using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour
{
    // Biến static kiểu Persistent lưu trữ instance duy nhất của class
    public static Persistent instance;

    private void Awake()
    {
        // Hàm Awake được gọi khi object được kích hoạt
        if (instance == null) // Kiểm tra xem đã có instance nào chưa
        {
            instance = this; // Nếu chưa có, thiết lập instance là object này
            DontDestroyOnLoad(gameObject); // Giữ object này không bị hủy khi load scene mới
        }
        else // Nếu đã có instance khác
        {
            Destroy(gameObject); // Hủy object này
        }
    }
}
