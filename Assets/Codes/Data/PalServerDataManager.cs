using System;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Data.Common;
using System.Text;

public class PalServerDataManager
{
    private static string palServerSettingPath => Path.GetDirectoryName(Application.dataPath) + "/Config/";
    private static string palServerSettingName = "palServerSettings";

    public static void Save(PalServerData data)
    {
        if (!Directory.Exists(palServerSettingPath))
        {
            Directory.CreateDirectory(palServerSettingPath);
        }

        string json = JsonConvert.SerializeObject(data);
        string filePath = palServerSettingPath + palServerSettingName + ".json";
        WriteFile(filePath, json);
        SaveConfig(data);
    }

    public static void SaveConfig(PalServerData data)
    {
        StringBuilder settingResult = new StringBuilder("[/Script/Pal.PalGameWorldSettings]\nOptionSettings=(");
        int count = data.palServerValues.Count;
        int currentIndex = 0;
        data.palServerValues.ForEach((palServerValue) =>
        {
            settingResult.Append(palServerValue.name);
            settingResult.Append("=");
            if (palServerValue.type == PalServerValue.ValueType.String)
            {
                settingResult.Append("\"");
                settingResult.Append(palServerValue.Value);
                settingResult.Append("\"");
            }
            else
            {
                settingResult.Append(palServerValue.Value);
            }

            if (currentIndex < count - 1)
            {
                settingResult.Append(", ");
            }

            currentIndex++;
        });
        settingResult.Append(")");
        string test = settingResult.ToString();
        try
        {
            string filePath = data.PalServerConfigPath;
            File.WriteAllText(filePath, settingResult.ToString());
            Debug.Log("파일 저장 성공: " + filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("파일 저장 실패: " + ex.Message);
        }
    }
    private static void WriteFile(string path, string content)
    {
        File.WriteAllText(path, content);
    }

    public static PalServerData Load()
    {
        string filePath = palServerSettingPath + palServerSettingName + ".json";

        if (!File.Exists(filePath))
        {
            Debug.Log("No such file exists");
            return null;
        }

        string file = File.ReadAllText(filePath);
        PalServerData data = JsonConvert.DeserializeObject<PalServerData>(file);
        return data;
    }
}
