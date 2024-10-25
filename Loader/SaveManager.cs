using System.IO;

namespace LabRat
{
    public static class SaveManager
    {
        private static readonly string _saveFile = "savefile.txt";
        private static readonly string _saveDirectory = "LabRat";
        private static readonly string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _saveDirectory);
        private static readonly string _saveFilePath = Path.Combine(_directoryPath, _saveFile);

        public static void SaveGame(int levelsCompleted)
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
            File.WriteAllText(_saveFilePath, levelsCompleted.ToString());
        }

        public static int LoadGame()
        {
            int levels = 0;
            try
            {
                string data = File.ReadAllText(_saveFilePath);
                levels = int.Parse(data);
            }
            catch
            {
                SaveGame(0);
            }
            return levels;
        }
    }
}
