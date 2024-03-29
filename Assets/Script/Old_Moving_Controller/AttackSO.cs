using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attribute này cho phép tạo đối tượng này từ menu "Create Asset" trong Unity
[CreateAssetMenu(menuName ="Attack/Normal Attack")]
public class AttackSO : ScriptableObject
{
    // ScriptableObject là loại đối tượng đặc biệt trong Unity,
    //  thường dùng để lưu trữ dữ liệu dùng chung cho nhiều đối tượng khác.
    public AnimatorOverrideController aniOV;
}
