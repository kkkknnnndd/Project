using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    // Biến static lưu trữ instance duy nhất của class
    public static SceneFadeManager instance;

    // Tham chiếu đến Image dùng để fade
    [SerializeField] private Image _fadeOutImage;

    // Tốc độ fade (out/in)
    [Range(0.1f, 10f), SerializeField] private float _fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float _fadeInSpeed = 5f;

    // Màu bắt đầu của fade out
    [SerializeField] private Color _fadeOutStartColor;

    // Kiểm tra trạng thái fade
    public bool IsFadingOut { get; private set; }
    public bool ISFadingIn { get; private set; }

    private void Awake()
    {
        // Thiết lập instance
        if (instance == null)
        {
            instance = this;
        }

        // Khởi tạo màu sắc fade
        _fadeOutStartColor.a = 0f; // Alpha bằng 0 (hoàn toàn trong suốt)
        _fadeOutImage.gameObject.SetActive(false); // Ẩn image fade
    }

    private void Update()
    {
        // Cập nhật fade out
        if (IsFadingOut)
        {
            _fadeOutImage.gameObject.SetActive(true); // Hiện image fade

            // Kiểm tra nếu chưa fade hoàn toàn (alpha < 1)
            if (_fadeOutImage.color.a < 1f)
            {
                _fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed; // Tăng alpha theo thời gian
                _fadeOutImage.color = _fadeOutStartColor; // Áp dụng màu mới
            }
            else // Nếu đã fade out hoàn toàn
            {
                IsFadingOut = false; // Reset cờ fade out
            }
        }

        // Cập nhật fade in
        if (ISFadingIn)
        {
            // Kiểm tra nếu chưa fade in hoàn toàn (alpha > 0)
            if (_fadeOutImage.color.a > 0f)
            {
                _fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed; // Giảm alpha theo thời gian
                _fadeOutImage.color = _fadeOutStartColor; // Áp dụng màu mới
            }
            else // Nếu đã fade in hoàn toàn
            {
                ISFadingIn = false; // Reset cờ fade in
                _fadeOutImage.gameObject.SetActive(false); // Ẩn image fade
            }
        }
    }

    // Bắt đầu fade out
    public void StartFadeOut()
    {
        _fadeOutImage.color = _fadeOutStartColor; // Khởi tạo màu fade out
        IsFadingOut = true; // Thiết lập cờ fade out
    }

    // Bắt đầu fade in
    public void StartFadeIn()
    {
        // Kiểm tra nếu đang fade out hoàn toàn (alpha >= 1)
        if (_fadeOutImage.color.a >= 1f)
        {
            _fadeOutImage.color = _fadeOutStartColor; // Khởi tạo màu fade in
            ISFadingIn = true; // Thiết lập cờ fade in
        }
    }
}

