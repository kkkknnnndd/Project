using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Khai báo class Death kế thừa từ MonoBehaviour
public class Death : MonoBehaviour
{
    // [SerializeField] cho phép edit biến trong inspector
    [SerializeField] private AttackSO _deathAni; // Lưu trữ Animation khi chết
    private Animator anim; // Biến lưu trữ Animator component

    // [SerializeField] cho phép edit biến trong inspector
    [SerializeField] private GameObject uiDeath; // Lưu trữ GameObject của UI báo chết

    // [SerializeField] cho phép edit biến trong inspector
    [SerializeField] private PlayerStats stats; // Tham chiếu đến script PlayerStats

    public bool isDeath = false; // Biến kiểm tra nhân vật có đang chết không

    // Hàm Start được gọi khi object được active
    private void Start()
    {
        // Tìm kiếm GameObject với tag "Player"
        GameObject tempChar = GameObject.FindGameObjectWithTag("Player");

        // Lấy Animator component từ GameObject tìm được
        anim = tempChar.GetComponent<Animator>();

        // Ngăn chặn UI báo chết hiển thị ban đầu
        uiDeath.SetActive(false);
    }

    // Hàm Update được gọi mỗi frame
    private void Update()
    {
        // Kiểm tra nếu currentHealth <= 0, nhân vật chưa chết (isDeath = false), và không đang load health
        if (stats.currentHealth <= 0 && !isDeath && !stats._isLoadCurrentHealth)
        {
            // Gán Animation chết cho Animator
            anim.runtimeAnimatorController = _deathAni.aniOV;

            // Set isDeath thành true để đánh dấu nhân vật đã chết
            isDeath = true;

            // Phát animation chết từ state "Death" với transition 0 (nếu có nhiều transition)
            // - tham số 0.05f có thể là time offset khi bắt đầu phát animation
            anim.Play("Death", 0, 0.05f);
        }

        // Kiểm tra nếu animation đang ở state "Death" (tag), 
        // đã chạy được hơn 75% (normalizedTime > 0.75f) và nhân vật đang chết
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Death")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && isDeath)
        {
            // Hiển thị UI báo chết
            uiDeath.SetActive(true);
        }

        // Kiểm tra nếu người chơi nhấn phím E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Ẩn UI báo chết
            uiDeath.SetActive(false);

            // Set isDeath thành false để đánh dấu nhân vật còn sống
            isDeath = false;

            // Set flag để script PlayerStats load lại currentHealth
            stats._isLoadCurrentHealth = true;

            // Set flag để WorldSaveGameManager load game (tùy thuộc vào thiết kế)
            WorldSaveGameManager.instance.loadGame = true;
        }
    }
}

