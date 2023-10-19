using UnityEngine;
using System;
using System.IO;

namespace TapGame
{
    public class FileDataHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";
        private bool _useEncryption = false;
        private readonly string _encryptionCodeWord = "HomeMadeStudio";

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName + ".txt";
        }
        
        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName + ".txt";
            this._useEncryption = useEncryption;
        }

        public T Load<T>()
        {
            // Use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            T loadData = default;

            if (File.Exists(fullPath))
            {
                try
                {
                    // Load the serialized data from the file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // Optionally encrypt the data
                    if (_useEncryption)
                    {
                        dataToLoad = EcryptDecrypt(dataToLoad);
                    }

                    // Deserialize the data from Json back into the C# object
                    loadData = JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data to file: " + fullPath + "\n" + e);
                }
            }

            return loadData;
        }

        public void Save<T>(T data)
        {
            // Use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            try
            {
                // Create the directory the file will be written to of it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);

                // Serialize the C# game data object into Json
                string dataToStore = JsonUtility.ToJson(data, true);

                // Optionally encrypt the data
                if (_useEncryption)
                {
                    dataToStore = EcryptDecrypt(dataToStore);
                }

                // Write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        // The below is a simple implementation of XOR encryption
        private string EcryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
            }

            return modifiedData;
        }

        public void Delete()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.Log("No " + fullPath);
            }
            else
            {
                File.Delete(fullPath);
                Debug.Log("Delete Succes");
            }
        }
    }
}