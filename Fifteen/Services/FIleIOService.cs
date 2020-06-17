using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace Fifteen.Services
{
    class FIleIOService
    {
        private readonly string _path;

        public FIleIOService(string path)
        {
            _path = path;
        }
        
        public BindingList<Records> LoadData()
        {
            var fileExist = File.Exists(_path);

            if (!fileExist)
            {
                File.CreateText(_path).Dispose();
                return new BindingList<Records>();
            }

            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Records>>(fileText);
            }
        }

        public void SaveData(BindingList<Records> recordsList)
        {
            using (StreamWriter writer = File.CreateText(_path))
            {
                string output = JsonConvert.SerializeObject(recordsList);
                writer.Write(output);
            }
        }
    }
}
