using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Fifteen.Services
{
    class FIleIOService
    {
        private readonly string PATH;

        public FIleIOService(string path)
        {
            PATH = path;
        }
        
        public BindingList<Records> LoadData()
        {
            var fileExist = File.Exists(PATH);

            if (!fileExist)
            {
                File.CreateText(PATH).Dispose();
                return new BindingList<Records>();
            }

            using (var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Records>>(fileText);
            }
        }

        public void SaveData(BindingList<Records> recordsList)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(recordsList);
                writer.Write(output);
            }
        }
    }
}
