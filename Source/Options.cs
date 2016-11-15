using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Surrounded.Source
{
    public class Options
    {
        // The name of the options file.
        private string fileName;
        
        // Options.
        public bool Fullscreen = false;
        public uint Width = 960;
        public uint Height = 540;
        public uint AntialiasingLevel = 8;
        public bool VerticalSync = false;
        public uint FramerateLimit = 60;

        // Class constructor.
        public Options(string fileName)
        {
            this.fileName = fileName;
        }

        // Sets the current filename, usually used when loading the class via JSON.
        public void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }
        
        // Saves options to specified file.
        public void Save()
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, this.fileName + ".json"), JsonConvert.SerializeObject(this));
        }

        // Loads a settings file directly from the file system.
        public static Options Load(string fileName = "options")
        {
            // Check if the file exists.
            Options options = new Options(fileName);
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, fileName + ".json")))
            {
                // Load the text data from file.
                StreamReader fileReader = File.OpenText(Path.Combine(Environment.CurrentDirectory, fileName + ".json"));
                string text = fileReader.ReadToEnd();
                options = JsonConvert.DeserializeObject<Options>(text);
                options.SetFileName(fileName);
                fileReader.Close();
            }
            else
            {
                // Create the options file.
                File.Create(Path.Combine(Environment.CurrentDirectory, fileName + ".json")).Close();
                options.Save();
            }
            return options;
        }
    }
}
