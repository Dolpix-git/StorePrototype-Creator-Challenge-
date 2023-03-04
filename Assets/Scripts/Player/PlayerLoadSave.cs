using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class PlayerLoadSave{
    public static void SavePlayer() {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.val";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        Debug.Log($"Money:{Player.Instance.PlayerData.playerMoney}   Length:{Player.Instance.PlayerData.playerInventory.Length}");

        if (Player.Instance.PlayerData == null) {
            formatter.Serialize(fileStream, new PlayerData());
        } else {
            formatter.Serialize(fileStream, Player.Instance.PlayerData);
        }
        Debug.Log("Saved Player Data");
        fileStream.Close();
    }
    public static PlayerData LoadPlayer() {
        string path = Application.persistentDataPath + "/player.val";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            PlayerData data = (PlayerData)formatter.Deserialize(fileStream);
            fileStream.Close();
            Debug.Log("Loaded save file");
            return data;
        } else {
            Debug.LogError("Attempted to load a save file! Currently no save file in " + path);
            return null;
        }
    }
}
