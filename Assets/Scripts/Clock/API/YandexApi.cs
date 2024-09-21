using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Clock.API
{
    public class YandexApi : ITimeApi
    {
        private const string URL = "https://yandex.com/time/sync.json";
        private TimeSpan _serverTime;

        public IEnumerator GetTime(Action<TimeResponse> action)
        {
            while (true)
            {
                using (var webRequest = UnityWebRequest.Get(URL))
                {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogError("Error: " + webRequest.error);
                    }
                    else
                    {
                        var jsonResponse = webRequest.downloadHandler.text;

                        var timeResponse = JsonUtility.FromJson<TimeResponse>(jsonResponse);

                        action(timeResponse);
                    }
                }

                yield return new WaitForSeconds(3600f);
            }
        }
    }
}