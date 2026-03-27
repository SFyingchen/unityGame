using System.IO;
using UnityEngine;

namespace Lingtianlu
{
    public class SaveSystem : MonoBehaviour
    {
        [SerializeField] private string fileName = "savegame.json";

        private string SavePath => Path.Combine(Application.persistentDataPath, fileName);

        public void Save(SaveData data)
        {
            if (data == null)
            {
                return;
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public SaveData Load()
        {
            if (!File.Exists(SavePath))
            {
                return new SaveData();
            }

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
        }
    }
}
