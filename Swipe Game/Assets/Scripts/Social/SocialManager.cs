using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialManager : MonoBehaviour
{
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWEET_LANGUAGE = "en";

    public static void ShareToTwitter(string textToDisplay)
    {
        Application.OpenURL(TWITTER_ADDRESS +
                    "?text=" + WWW.EscapeURL(textToDisplay) +
                    "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
    }

    public static void Share()
    {
        /*TODO*/
    }
}
