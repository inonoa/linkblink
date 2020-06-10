using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class Tweeter : MonoBehaviour
{

    [SerializeField, Tooltip("`[score]`でスコアが入るよ, `[seq]`でシーケンス名が入るよ"), Multiline(10)]
    string tweetText;
    string url;

#if UNITY_WEBGL
    [DllImport("__Internal")] private static extern void OpenNewWindow(string url);
#endif

    public void Tweet(int score, string sequenceName)
    {
        string actualTweetText = 
            tweetText.Replace(
                "[score]",
                score.ToString()
            ).Replace(
                "[seq]",
                sequenceName
            );

        url = "https://twitter.com/intent/tweet?text=" 
            + UnityWebRequest.EscapeURL(actualTweetText);
        
        OpenTweetWindow();
    }

    void OpenTweetWindow(){

#if UNITY_WEBGL && !UNITY_EDITOR
        OpenNewWindow(url);
#else
        Application.OpenURL(url);
#endif
    }
}
