using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionStarter : MonoBehaviour
{
    public GameObject uiCanvas; // Tham chiếu đến canvas UI
    private bool isInside = false; // Biến để xác định xem nhân vật có ở bên trong cube không
    [SerializeField] private GameObject Loading_Text;
    private bool _isQuestion = false;
    private void Awake()
    {
        if (!isInside)
        {
            Loading_Text.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Kiểm tra nếu nhân vật đi vào trong cube
        if (other.CompareTag("Player") && !_isQuestion)
        {
            isInside = true;
            if (isInside)
            {
                Loading_Text.SetActive(true);
            }
            else
            {
                Loading_Text.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Kiểm tra nếu nhân vật rời khỏi cube
        if (other.CompareTag("Player"))
        {
            _isQuestion = false;
            isInside = false;
            Loading_Text.SetActive(false);
            uiCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Kiểm tra nếu nhân vật ở bên trong cube và người chơi nhấn phím F
        if (isInside && Input.GetKeyDown(KeyCode.F))
        {
            _isQuestion = true;
            Loading_Text.SetActive(false);
            // Hiển thị canvas UI
            uiCanvas.SetActive(true);
        }
    }
}
