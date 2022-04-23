using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using JsonFx.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Multiplatform.Podcasts
{
    public class APITest : MonoBehaviour
    {
        [SerializeField]
        AudioClipController audio;

        [SerializeField]
        PodcastSelectionUI podcastSelection, episodeSelection;

        string listenURL = "https://listen-api.listennotes.com/api/v2";
        string transistorURL = "https://api.transistor.fm/v1";

        string listenAPIKey = "187bfbd5eb724368912389484469b59d";
        string transistorAPIKey = "jY9f3ybHAGkBHdYVd4uhng";

        [SerializeField]
        int showId = 0;

        Dictionary<string, string> podcastIds;
        Dictionary<string, string> episodeIds;

        private void Awake()
        {
            GetPodcasts();
        }

        #region Transistor
        string QueryServer(string _extension = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(transistorURL + _extension);

            request.Headers["x-api-key"] = transistorAPIKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.NotFound)
            {
                Debug.Log("Error querying server");
            }

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();

            return jsonResponse;
        }
        public void GetShowAnalytics()
        {
            string data = "/analytics/1&include[]=show&fields[show][]=title";

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(transistorURL + data);
            httpRequest.Method = "POST";

            httpRequest.Headers["x-api-key"] = transistorAPIKey;
            httpRequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();

                Debug.Log("Result: " + result);
            }

            Debug.Log("Status Code: " + httpResponse.StatusCode);
        }
        public void GetShows()
        {
            string data = "/shows/pagination[page]=1&pagination[per]=5&fields[show][]=title&fields[show][]=description";

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(transistorURL + data);
            httpRequest.Method = "POST";

            httpRequest.Headers["x-api-key"] = transistorAPIKey;
            httpRequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();

                Debug.Log("Result: " + result);
            }

            Debug.Log("Status Code: " + httpResponse.StatusCode);
        }
        public void GetEpisodes()
        {
            string data = $"/episodes/show_id={showId}&pagination[page]=1&pagination[per]=5&fields[episode][]=title&fields[episode][]=summary";

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(transistorURL + data);
            httpRequest.Method = "POST";

            httpRequest.Headers["x-api-key"] = transistorAPIKey;
            httpRequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();

                Debug.Log("Result: " + result);
            }

            Debug.Log(httpResponse.StatusCode);
        }

        #endregion

        #region Listen
        void GetPodcasts(int _genreId = 127)
        {
            podcastIds = new Dictionary<string, string>();

            string data = $"/best_podcasts?genre_id={127}&page=2®ion=us&sort=listen_score&safe_mode=0";

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(listenURL + data);
            httpRequest.Headers["X-ListenAPI-Key"] = "187bfbd5eb724368912389484469b59d";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string resultJSON = streamReader.ReadToEnd();

                JsonReader jsonReader = new JsonReader();
                dynamic results = jsonReader.Read(resultJSON);

                int numberOfPodcasts = results["podcasts"].Length;
                for (int i = 0; i < numberOfPodcasts; ++i)
                {
                    dynamic podcast = results["podcasts"][i];

                    podcastIds.Add(podcast["title"].Trim(), podcast["id"]);
                }

                podcastSelection.SetSelection(podcastIds, DownloadPodcast);
            }

            Debug.Log("GetPodcasts: " + httpResponse.StatusCode);
        }
        void DownloadPodcast(string _title)
        {
            string podcastId = podcastIds[_title];

            episodeIds = new Dictionary<string, string>();

            string data = $"/podcasts/{podcastId}?next_episode_pub_date=0&sort=recent_first";

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(listenURL + data);
            httpRequest.Headers["X-ListenAPI-Key"] = "187bfbd5eb724368912389484469b59d";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();

                JsonReader jsonReader = new JsonReader();
                dynamic results = jsonReader.Read(result);
                int numberOfEpisodes = results["episodes"].Length;
                for (int i = 0; i < numberOfEpisodes; ++i)
                {
                    dynamic episode = results["episodes"][i];

                    if (!episodeIds.ContainsKey(episode["title"]))
                    {
                        episodeIds.Add(episode["title"], episode["id"]);
                    }
                }

                episodeSelection.SetSelection(episodeIds, DownloadAudio);
            }

            Debug.Log("GetEpisode: " + httpResponse.StatusCode);
        }
        public void Search()
        {
            Debug.Log("Searching");

            string data = $"/episodes/a61cd3ba8f3a42acbd4f11ed908b6c8a?show_transcript=0";

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(listenURL + data);
            httpRequest.Headers["X-ListenAPI-Key"] = listenAPIKey;

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            string result = "";
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                //Podcast podcast = JsonUtility.FromJson(result, typeof(Podcast)) as Podcast;
                //
                //Debug.Log(podcast.title);

                //Debug.Log("Asset Found");

                if (!File.Exists("Assets/Result.txt"))
                    using (StreamWriter writer = File.CreateText("Assets/Result.txt"))
                    {
                        writer.WriteLine(result);
                    }

                DownloadAudio("a61cd3ba8f3a42acbd4f11ed908b6c8a");
            }
        }
        void DownloadAudio(string _id)
        {
            Debug.Log("Downloading: " + _id);

            string url = $"https://www.listennotes.com/e/p/{episodeIds[_id]}/";

            StartCoroutine(DoDownload(url));
        }
        IEnumerator DoDownload(string _url)
        {
            using (UnityWebRequest unityRequest = UnityWebRequestMultimedia.GetAudioClip(_url, AudioType.MPEG))
            {
                yield return unityRequest.SendWebRequest();

                DownloadHandlerAudioClip download = unityRequest.downloadHandler as DownloadHandlerAudioClip;
                download.streamAudio = true;
                SetAudio(download.audioClip);
            }
        }
        void SetAudio(AudioClip _clip)
        {
            audio.SetClip(_clip);
        }
        #endregion
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(APITest))]
    public class APITestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Search"))
            {
                (target as APITest).Search();
            }

            //if (GUILayout.Button("Ping Shows"))
            //{
            //    (target as APITest).GetShows();
            //}
            //
            //if (GUILayout.Button("Ping Episodes"))
            //{
            //    (target as APITest).GetEpisodes();
            //}
        }
    }
#endif
}