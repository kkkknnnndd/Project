using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region AttackVariables
    public List<AttackSO> combo; // Danh sách các đòn đánh combo
    private float lastpress = 0; // Thời gian nhấn nút đánh cuối cùng (Cooldown)
    private float lastcomboEnd; // Thời gian kết thúc combo trước
    private int ComboCounter; // Số đòn đánh thực hiện trong combo hiện tại
    private Animator anim; // Tham chiếu đến Animator của nhân vật
    private Animator anime; // Biến dự phòng (gỡ bỏ)
    public static bool isAttacking = false; // Kiểm tra xem nhân vật đang tấn công không
    public AudioClip attackSound; // Âm thanh đánh
    private PlayerStats playerStats; // Tham chiếu đến Script PlayerStats
    private int attackStaminaCost = 20; // Lượng Stamina tiêu tốn khi đánh
    private AudioSource audioFX; // Tham chiếu đến AudioSource đánh hiệu ứng
    #endregion
    void Start()
    {
        anim = GetComponent<Animator>(); // Lấy Animator của GameObject này
        GameObject tempchar = GameObject.FindGameObjectWithTag("Player"); // Tìm kiếm GameObject với tag "Player"
        audioFX = tempchar.GetComponent<CharacterController>().GetComponent<AudioSource>(); // Lấy AudioSource từ CharacterController của Player
        anime = tempchar.GetComponentInChildren<Animator>(); // Lấy Animator con của Player (có thể gỡ bỏ vì đã có anim)
        playerStats = tempchar.GetComponent<PlayerStats>(); // Lấy Script PlayerStats của Player
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) // Kiểm tra nếu nút J được bấm xuống
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge") // Không đang né tránh
            && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Impact") // Không đang trong trạng thái va chạm
            && playerStats.currentStamina > attackStaminaCost) // Stamina đủ để đánh
        {
            Attack(); // Thực hiện đòn đánh
        }
        ExitAttack(); // Kiểm tra để kết thúc combo
    }

    private void Attack()
    {
        if (Time.time - lastcomboEnd >= 0.5f && ComboCounter < combo.Count) // Kiểm tra khoảng cách giữa các đòn combo và chưa quá số đòn
        {
            CancelInvoke("EndCombo"); // Hủy lệnh kết thúc combo cũ (nếu có)
            if (Time.time - lastpress >= 0.6f) // Kiểm tra cooldown giữa các đòn đánh
            {
                anim.runtimeAnimatorController = combo[ComboCounter].aniOV; // Set Animation Controller cho đòn đánh hiện tại
                anim.Play("Attack", 0, 0.15f); // Phát animation "Attack"
                audioFX.PlayOneShot(attackSound); // Phát âm thanh đánh
                isAttacking = true; // Đánh dấu đang tấn công
                ComboCounter++; // Tăng số đòn đánh trong combo
                playerStats.TakeStaminaDamage(attackStaminaCost); // Giảm Stamina
                lastpress = Time.time; // Lưu thời gian nhấn nút đánh mới nhất

                if (ComboCounter >= combo.Count) // Kiểm tra nếu vượt quá số đòn combo
                {
                    ComboCounter = 0; // Reset lại số đòn
                }
            }
        }
    }

    private void ExitAttack()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1); // after 1 sec
        }    
    }
    private void EndCombo()
    {
        ComboCounter = 0; // Reset lại số đòn đánh
        isAttacking = false; // Đánh dấu ngừng tấn công
        lastcomboEnd = Time.time; // Lưu thời gian kết thúc combo
    }

}
