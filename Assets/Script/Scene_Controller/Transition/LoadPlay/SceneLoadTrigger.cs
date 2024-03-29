using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    // Danh sách các scene sẽ được load
    [SerializeField] private SceneField[] _scenesToLoad;
    // Danh sách các scene sẽ được unload
    [SerializeField] private SceneField[] _scenesToUnLoad;
    // Tham chiếu đến GameObject player
    private GameObject _player;
    private void Awake()
    {
        // Tìm kiếm GameObject player
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu player va chạm với trigger
        if (other.gameObject == _player)
        {
            // Load các scene
            LoadScenes();

            // Unload các scene
            UnLoadScenes();
        }
    }
    private void LoadScenes()
    {
        // Duyệt qua danh sách scene cần load
        for (int i = 0; i < _scenesToLoad.Length; i++)
        {
            // Biến kiểm tra xem scene đã được load hay chưa
            bool isSceneLoad = false;

            // Duyệt qua các scene đã load
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                // Lấy scene thứ j đã load
                Scene loadedScene = SceneManager.GetSceneAt(j);

                // Kiểm tra nếu tên scene trùng với scene cần load
                if (loadedScene.name == _scenesToLoad[i].SceneName)
                {
                    isSceneLoad = true; // Đánh dấu scene đã được load
                    break; // Thoát khỏi vòng lặp trong
                }
            }

            // Nếu scene chưa được load
            if (!isSceneLoad)
            {
                // Load scene theo chế độ Additive (không unload scene đang hoạt động)
                SceneManager.LoadSceneAsync(_scenesToLoad[i], LoadSceneMode.Additive);
            }
        }
    }
    private void UnLoadScenes()
    {
        // Duyệt qua danh sách scene cần unload
        for (int i = 0; i < _scenesToUnLoad.Length; i++)
        {
            // Duyệt qua các scene đã load
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                // Lấy scene thứ j đã load
                Scene loadedScene = SceneManager.GetSceneAt(j);

                // Kiểm tra nếu tên scene trùng với scene cần unload
                if (loadedScene.name == _scenesToUnLoad[i].SceneName)
                {
                    // Unload scene
                    SceneManager.UnloadSceneAsync(_scenesToUnLoad[i]);
                }
            }
        }
    }
}

