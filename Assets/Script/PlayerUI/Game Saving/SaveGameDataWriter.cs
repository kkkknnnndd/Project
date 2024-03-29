using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveGameDataWriter : MonoBehaviour
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    //Trước khi tạo save file mới, cần kiểm tra xem nếu 1 trong những character slots đã tồn tại (max 10 character slots)
    public bool CheckToSeeIfFileExists()
    {
        if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //Dùng xóa Characeter Save Files
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }
    //Dùng tạo tệp lưu khi bắt đầu game mới
    public void WriteCharacterDataToSaveFile(CharacterSaveData characterData)
    {
        //Tạo path để save file (1 vị trí trên máy tính)
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);
        Debug.Log("Move to this point");

        try
        {
            //tạo thư mục mà tệp sẽ được ghi vào nếu nó chưa tồn tại
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

            //Serialize the C# game data object into JSON
            string dataToStore = JsonUtility.ToJson(characterData, true);

            //Write the file to our System
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR WHILE TRYING TO SAVE CHARACTER DATA, GAME COULD NOT BE SAVED " + ex);
        }
    }
    //Dùng tải 1 save file khi tải game trước đó
    public CharacterSaveData LoadCharacterDataFromJson()
    {
        CharacterSaveData characterData = null;

        //Tạo path để load file
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //Deserialize the data from JSON back to UNITY
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }

        return characterData;
    }
}
