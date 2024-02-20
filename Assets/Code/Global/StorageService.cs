using UnityEngine;

namespace Game.Global
{
    public static class StorageService
    {
        public static void Set(string objectValue, object data)
        {
            string jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(objectValue, jsonData);
        }

        public static T Get<T>(string objectValue)
        {
            string jsonData = PlayerPrefs.GetString(objectValue);
            return JsonUtility.FromJson<T>(jsonData);
        }
    }
}
