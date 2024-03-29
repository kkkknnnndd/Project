using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// Class này giúp quản lý việc chuyển scene và spawn player tại vị trí
public class SceneSpawnManager : MonoBehaviour
{
    // Biến static lưu trữ instance duy nhất của class
    public static SceneSpawnManager instance;

    // Kiểm tra xem scene được load từ SceneTriggerInteraction hay không
    private static bool _loadFromSceneChange;

    // Tham chiếu đến GameObject player
    private GameObject _player;

    // Tham chiếu đến CharacterController của player
    private CharacterController _characterController;

    // Tham chiếu đến BoxCollider (có thể là vị trí spawn)
    private BoxCollider _collider;

    // Lưu trữ vị trí spawn khi chuyển scene
    private SceneTriggerInteraction.SceneChangeSpawnAt _sceneToSpawnTo;

    private void Awake()
    {
        // Thiết lập instance
        if (instance == null)
        {
            instance = this;
        }

        // Tìm kiếm GameObject player
        _player = GameObject.FindGameObjectWithTag("Player");

        // Lấy CharacterController của player
        _characterController = _player.GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // Đăng ký sự kiện scene loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện scene loaded
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SwapSceneFromSceneTrig(SceneField myScene, SceneTriggerInteraction.SceneChangeSpawnAt sceneChangeSpawnAt)
    {
        // Đánh dấu chuyển scene từ SceneTriggerInteraction
        _loadFromSceneChange = true;

        // Bắt đầu coroutine fade out và chuyển scene
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, sceneChangeSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, 
        SceneTriggerInteraction.SceneChangeSpawnAt sceneChangeSpawnAt = SceneTriggerInteraction.SceneChangeSpawnAt.None)
    {
        // Bắt đầu fade out scene
        SceneFadeManager.instance.StartFadeOut();

        // Đợi đến khi fade out hoàn thành
        while (SceneFadeManager.instance.IsFadingOut)
        {
            yield return null;
        }

        // Lưu trữ vị trí spawn
        _sceneToSpawnTo = sceneChangeSpawnAt;

        // Load scene mới
        SceneManager.LoadScene(myScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Bắt đầu fade in scene
        SceneFadeManager.instance.StartFadeIn();

        // Kiểm tra nếu chuyển scene từ SceneTriggerInteraction
        if (_loadFromSceneChange)
        {
            // Tìm vị trí spawn dựa trên SceneChangeSpawnAt
            FindPosition(_sceneToSpawnTo);

            // Reset cờ chuyển scene
            _loadFromSceneChange = false;
        }
    }

    private void FindPosition(SceneTriggerInteraction.SceneChangeSpawnAt sceneSpawnNumber)
    {
        // Tìm tất cả các object SceneTriggerInteraction
        SceneTriggerInteraction[] scene = FindObjectsOfType<SceneTriggerInteraction>();

        // Duyệt qua các object SceneTriggerInteraction
        for (int i = 0; i < scene.Length; i++)
        {
            // Kiểm tra xem có khớp với vị trí spawn lưu trữ không
            if (scene[i].currentScenePosition == sceneSpawnNumber)
            {
                // Lấy BoxCollider của object
                _collider = scene[i].gameObject.GetComponent<BoxCollider>();

                // Teleport player đến vị trí spawn
                Tele_Char();
                break; // Thoát khỏi vòng lặp khi tìm thấy vị trí spawn
            }
        }
    }

    private void Tele_Char()
    {
        // Tắt CharacterController (có thể để tránh kẹt player)
        _characterController.enabled = false;

        // Debug vị trí spawn
        Debug.Log(_collider.transform.position);

        // Teleport player đến vị trí và hướng của BoxCollider
        _player.transform.position = _collider.transform.position;
        _player.transform.rotation = _collider.transform.rotation;

        // Thiết lập cờ đổi hướng (có thể liên quan đến script khác)
        DoiHuong._dhChangeScene = true;

        // Bật lại CharacterController
        _characterController.enabled = true;
    }
}

