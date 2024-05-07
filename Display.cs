using BepInEx;
using BepInEx.Configuration;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CGVD
{
    [BepInPlugin("CGVD", "CGVD", "1.0.0")]
    public class Display : BaseUnityPlugin
    {
        private ConfigEntry<string> Video_Path;
        private ConfigEntry<int> Video_Width, Video_Height;

        public void Play_Video()
        {
            RenderTexture rt = new RenderTexture(Video_Width.Value, Video_Height.Value, 16, RenderTextureFormat.ARGB32);
            rt.Create();

            GameObject Panel = GameObject.Find("Everything/Cube/Canvas").transform.GetChild(1).gameObject;
            DestroyImmediate(Panel.transform.GetComponent<Image>());

            RawImage image = Panel.AddComponent<RawImage>();
            image.texture = rt;
            
            var videoPlayer = Panel.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.url = Video_Path.Value;
            videoPlayer.isLooping = true;
            videoPlayer.targetTexture = rt;
            videoPlayer.Play();
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SceneHelper.CurrentScene == "Endless" && Video_Path.Value != "")
            {
                Transform Panel = GameObject.Find("Everything/Cube/Canvas").transform.GetChild(1);
                foreach (Transform child in Panel.transform)
                {
                    child.gameObject.SetActive(false);
                }

                Play_Video();

                Logger.LogInfo("Video Display Loaded.");
            }
        }

        void Awake()
        {
            Video_Path = Config.Bind("Video Config",
                                     "Video Path",
                                     "",
                                     "Full path to the video file without quotation marks. (Leave blank to disable)");

            Video_Width = Config.Bind("Video Config",
                                      "Video Width",
                                      0,
                                      "Width of the video.");

            Video_Height = Config.Bind("Video Config",
                                       "Video Height",
                                       0,
                                       "Height of the video.");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }
}
