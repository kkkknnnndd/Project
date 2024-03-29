using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    #region WSGMVariable
    public static WorldSaveGameManager instance;
    public PlayerStats player;
    SaveGameDataWriter saveGameDataWriter;

    [Header("Current Character Data")]
    //Character Slots
    public CharacterSaveData currentCharacterSaveData;
    [SerializeField] private string fileName;
    [SerializeField] private string filePath;

    [Header("SAVE/LOAD")]
    [SerializeField] public bool saveGame;
    [SerializeField] public bool loadGame;

    [Header("Persistent Data")]
    [SerializeField] private SceneField persistentGamePlay;
    private List<AsyncOperation> sceneToLoad = new List<AsyncOperation>();
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        saveGameDataWriter = gameObject.AddComponent<SaveGameDataWriter>();
    }
    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }
        
        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    //NEW GAME
    public void SaveGame()
    {
        saveGameDataWriter.saveDataDirectoryPath = filePath;
        saveGameDataWriter.saveFileName = fileName;

        player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);

        //Write the current character data to a json file and save it on device
        saveGameDataWriter.WriteCharacterDataToSaveFile(currentCharacterSaveData);

        Debug.Log("SAVING GAME...");
        Debug.Log("FILE SAVED AS: " + fileName);
    }

    public void LoadGame()
    {
        //Decide Load File based on character save slot
        saveGameDataWriter.saveDataDirectoryPath = filePath;
        saveGameDataWriter.saveFileName = fileName;
        currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

        StartCoroutine(LoadWorldSceneAsynchronously());
    }

    private IEnumerator LoadWorldSceneAsynchronously()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerStats>();
        }

        //sceneToLoad.Add(SceneManager.LoadSceneAsync(persistentGamePlay));
        sceneToLoad.Add(SceneManager.LoadSceneAsync(currentCharacterSaveData._sceneNow));
        for (int i = 0; i < sceneToLoad.Count; i++)
        {
            if (!sceneToLoad[i].isDone)
            {
                yield return null;
            }
        }
        player.LoadCharacterDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);
    }
}
