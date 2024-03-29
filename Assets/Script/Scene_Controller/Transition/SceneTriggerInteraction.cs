using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTriggerInteraction : TriggerInteractionBase
{
    // Enum lưu trữ các vị trí spawn có thể (tối đa 32 vị trí)
    public enum SceneChangeSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven,
        Twelve,
        Thirdteen,
        Fourteen,
        Fiveteen,
        Sixteen,
        Seventeen,
        Eighteen,
        Nineteen,
        Twenty,
        Twenty_One,
        Twenty_Two,
        Twenty_Three,
        Twenty_Four,
        Twenty_Five,
        Twenty_Six,
        Twenty_Seven,
        Twenty_Eight,
        Twenty_Nine,
        Thirty,
        Thirty_One,
        Thirty_Two
    }

    // [Header("Spawn TO")] - Tiêu đề trong Inspector để nhóm các thuộc tính liên quan đến vị trí spawn đến
    [SerializeField] private SceneChangeSpawnAt SceneToSpawnTo; // Vị trí spawn đến trong scene mới
    [SerializeField] private SceneField _sceneToLoad; // Scene sẽ được load đến

    // [Header("This Scene")] - Tiêu đề trong Inspector để nhóm các thuộc tính liên quan đến scene hiện tại
    [SerializeField] public SceneChangeSpawnAt currentScenePosition; // Vị trí của trigger trong scene hiện tại

    // Hàm được gọi khi tương tác với trigger
    public override void Interact()
    {
        // Sử dụng SceneSpawnManager để chuyển scene
        SceneSpawnManager.instance.SwapSceneFromSceneTrig(_sceneToLoad, SceneToSpawnTo);

        // Reset cờ tương tác trong NozoCharacterController (có thể để tránh kích hoạt tương tác nhiều lần)
        NozoCharacterController.WasInteractPressed = false;
    }
}


