using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteraction : MonoBehaviour
{
    [SerializeField] private SceneField _sceneNow;
    [SerializeField] private GameObject Loading_Text;
    private bool _loadingTime = false;
    private PlayerStats _playerStats;
    private void Awake()
    {
        GameObject tempchar = GameObject.FindGameObjectWithTag("Player");
        _playerStats = tempchar.GetComponent<PlayerStats>();
        if (!_loadingTime)
        {
            Loading_Text.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _loadingTime)
        {
            WorldSaveGameManager.instance.currentCharacterSaveData._sceneNow = _sceneNow;
            WorldSaveGameManager.instance.saveGame = true;
        }
        if (_playerStats._isLoadingyposition)
        {
            WorldSaveGameManager.instance.loadGame = true;
            _playerStats._isLoadingyposition = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _loadingTime = true;
            if (_loadingTime)
            {
                Loading_Text.SetActive(true);
            }
            else
            {
                Loading_Text.SetActive(false);
            }  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _loadingTime = false;
        Loading_Text.SetActive(false);
    }
}
