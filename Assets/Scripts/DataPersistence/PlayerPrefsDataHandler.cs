using UnityEngine;
using System;
using System.IO;

public class PlayerPrefsDataHandler
{
    private string dataKey = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    public PlayerPrefsDataHandler(string dataKey, bool useEncryption)
    {
        this.dataKey = dataKey;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        GameData loadedData = null;
        if (PlayerPrefs.HasKey(dataKey))
        {
            try
            {
                string dataToLoad = PlayerPrefs.GetString(dataKey);
                Debug.Log("Load: " + dataToLoad);

                if (useEncryption)
                {
                    //dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to load data from PlayerPrefs with key " + dataKey + "\n" + e);
            }

        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        try
        {
            string dataToStore = JsonUtility.ToJson(data);

            if (useEncryption)
            {
                //dataToStore = EncryptDecrypt(dataToStore);
            }

            PlayerPrefs.SetString(dataKey, dataToStore);
            PlayerPrefs.Save();  // Đảm bảo rằng dữ liệu được lưu ngay lập tức
            Debug.Log("Save: " + dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to PlayerPrefs with key " + dataKey + "\n" + e);
        }
    }

    // the below is a simply implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}

