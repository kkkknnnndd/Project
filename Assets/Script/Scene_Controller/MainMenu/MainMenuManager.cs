using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // Các đối tượng trong menu chính
    [Header("Main Menu Object")]
    [SerializeField] private GameObject Loading_Text; // Đối tượng chữ "Loading"
    [SerializeField] private GameObject[] objectToHide; // Mảng các đối tượng cần ẩn

    private void Awake()
    {
        // Khi khởi chạy game:
        // - Tắt chữ "Loading"
        // - Ẩn các đối tượng trong mảng objectToHide (trừ 2 đối tượng đầu tiên)
        Loading_Text.SetActive(false);
        for (int i = objectToHide.Length - 1; i > 1; i--)
        {
            objectToHide[i].SetActive(false);
        }
    }

    public void StartGame()
    {
        // Khi bắt đầu game:
        // - Ẩn các đối tượng menu
        // - Bật chữ "Loading"
        // - Bật các đối tượng khác
        // - Thiết lập WorldSaveGameManager.instance.loadGame = true (có thể sử dụng để tải game)
        HideMenu();
        Loading_Text.SetActive(true);
        for (int i = objectToHide.Length - 1; i > 1; i--)
        {
            objectToHide[i].SetActive(true);
        }
        WorldSaveGameManager.instance.loadGame = true;
    }

    private void HideMenu()
    {
        // Ẩn 2 đối tượng đầu tiên trong mảng objectToHide (có thể là đối tượng menu chính cần ẩn)
        for (int i = 0; i < 2; i++)
        {
            objectToHide[i].SetActive(false);
        }
    }
}

