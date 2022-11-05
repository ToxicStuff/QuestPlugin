using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine.Networking;

namespace QuestPlugin
{
    public class Class1 : MelonPlugin
    {
        public static Dictionary<string, string> FilePaths = new Dictionary<string, string>();
        public static string ModsDirectory { get; internal set; }
        public static string PluginsDirectory { get; internal set; }
        public static string ManagedPath { get; internal set; }
        public static string AssemblyGeneratorManaged { get; internal set; }

        static Class1()
        {
            //https://github.com/SirCoolness/MelonLoader/blob/7f64d42f80230e6284002a0324a62961898699b8/MelonLoader/Melons/Handler.cs#L36
            PluginsDirectory = Path.Combine(MelonUtils.GameDirectory, "Plugins");
            ModsDirectory = Path.Combine(MelonUtils.GameDirectory, "Mods");
            if (!Directory.Exists(ModsDirectory))
                Directory.CreateDirectory(ModsDirectory);
            ManagedPath = Path.Combine(string.Copy(MelonUtils.GetApplicationPath()), "melonloader", "etc", "managed");
        }
        public override void OnApplicationEarlyStart()
        {
            FilePaths.Add($"{ManagedPath}/Newtonsoft.Json.dll", "https://cdn.discordapp.com/attachments/746499694875639879/1038569726307209336/Newtonsoft.Json.dll"); //Fix Newtonsoft.Json Issue
            FilePaths.Add($"{ModsDirectory}/MyEpicTestMod.dll", "https://github.com/gompoc/MyEpicTestMod/releases/download/v1.0.0/MyEpicTestMod.dll"); //Its Needed to let VRChat working
            FilePaths.Add($"{ModsDirectory}/HWIDPatch.dll", "https://github.com/knah/ML-UniversalMods/releases/download/updates-2021-07-25/HWIDPatch.dll"); //HWID Spoofer
            FilePaths.Add($"{ModsDirectory}/QuestPlayspaceMover.dll", "https://github.com/Solexid/QuestPlayspaceMover/releases/download/r1/QuestPlayspaceMover.dll"); //PlaySpace Mover

            foreach (var valuePair in FilePaths)
            {
                MelonLogger.Msg($"Cheking for: {valuePair.Key}");
                if (!File.Exists($"{valuePair.Key}"))
                {
                    MelonLogger.Warning($"Missing file: {valuePair.Key}");
                    UnityWebRequest www = UnityWebRequest.Get(valuePair.Value);
                    www.SendWebRequest();
                    while (!www.isDone)
                    {
                    }
                    byte[] array = www.downloadHandler.data;
                    if (array == null || array.Length == 0)
                    {
                        MelonLogger.Error($"Failed: {valuePair.Value} is null");
                        return;
                    }
                    File.WriteAllBytes(valuePair.Key, array);
                    MelonLogger.Msg($"Downloaded {valuePair.Key}");
                }
            }
        }
    }
}
