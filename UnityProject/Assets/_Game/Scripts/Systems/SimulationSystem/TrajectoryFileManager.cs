using System.IO;
using UnityEngine;

namespace _Game.Systems.SimulationSystem
{
    public static class TrajectoryFileManager
    {
        private const string DirectoryName = "Trajectories";

        private static string GetDirectoryPath()
        {
            string path = Path.Combine(Application.persistentDataPath, DirectoryName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static void Save(string filename, PolarTrajectoryData data)
        {
            string json = JsonUtility.ToJson(data, true);
            string path = Path.Combine(GetDirectoryPath(), filename);
            File.WriteAllText(path, json);
            Debug.Log($"[TrajectoryFileManager] Saved to: {path}");
        }

        public static PolarTrajectoryData Load(string filename)
        {
            string path = Path.Combine(GetDirectoryPath(), filename);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<PolarTrajectoryData>(json);
            }

            Debug.LogWarning($"[TrajectoryFileManager] File not found: {path}");
            return null;
        }

        public static void Delete(string filename)
        {
            string path = Path.Combine(GetDirectoryPath(), filename);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[TrajectoryFileManager] Deleted: {path}");
            }
        }
    }
}