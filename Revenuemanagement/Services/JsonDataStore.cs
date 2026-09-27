using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Revenuemanagement.Models;

namespace Revenuemanagement.Services
{
    /// <summary>
    /// Сохранение и загрузка всех данных в локальный JSON-файл (без БД).
    /// </summary>
    public class JsonDataStore
    {
        private readonly string _filePath;
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            DateFormatString = "yyyy-MM-ddTHH:mm:ss",
            NullValueHandling = NullValueHandling.Ignore
        };

        public JsonDataStore(string fileName = "finance_data.json")
        {
            // Файл рядом с exe — удобно для резервных копий
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = Path.Combine(folder, fileName);
        }

        public string FilePath => _filePath;

        public FinanceData Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new FinanceData();

                string json = File.ReadAllText(_filePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(json))
                    return new FinanceData();

                var data = JsonConvert.DeserializeObject<FinanceData>(json, Settings);
                return data ?? new FinanceData();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Не удалось прочитать файл данных: " + _filePath + Environment.NewLine + ex.Message, ex);
            }
        }

        public void Save(FinanceData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                string json = JsonConvert.SerializeObject(data, Settings);
                File.WriteAllText(_filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Не удалось сохранить файл данных: " + _filePath + Environment.NewLine + ex.Message, ex);
            }
        }
    }
}
