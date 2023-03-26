using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using Facebook.Unity;

using UnityEngine.UI;
using TMPro;
using System;



public class CustomLogin : MonoBehaviour

{

    // public GameObject Panel_Add;

    public TextMeshProUGUI FB_userName;

    public Image FB_useerDp;





    public GameObject friendstxtprefab;

    public GameObject GetFriendsPos;

    private static readonly string EVENT_PARAM_SCORE = "score";

    private static readonly string EVENT_NAME_GAME_PLAYED = "game_played";

    private void Awake()

    {

        // FB.Init(SetInit, onHidenUnity);

        // Panel_Add.SetActive(false);





        if (!FB.IsInitialized)

        {

            FB.Init(() =>

            {

                if (FB.IsInitialized)

                    FB.ActivateApp();

                else

                    Debug.LogError("Couldn't initialize");

            },

            isGameShown =>

            {

                if (!isGameShown)

                    Time.timeScale = 0;

                else

                    Time.timeScale = 1;

            });

        }

        else

            FB.ActivateApp();

    }

    void SetInit()

    {

        if (FB.IsLoggedIn)

        {

            Debug.Log("Facebook is Login!");

        }

        else

        {

            Debug.Log("Facebook is not Logged in!");

        }

        DealWithFbMenus(FB.IsLoggedIn);

    }



    void onHidenUnity(bool isGameShown)

    {

        if (!isGameShown)

        {

            Time.timeScale = 0;

        }

        else

        {

            Time.timeScale = 1;

        }

    }

    public void FBLogin()

    {

        List<string> permissions = new List<string>();

        permissions.Add("public_profile");

        permissions.Add("user_friends");

        FB.LogInWithReadPermissions(permissions, AuthCallBack);

    }



    public void CallLogout()

    {

        StartCoroutine("FBLogout");

    }

    IEnumerator FBLogout()

    {

        FB.LogOut();

        while (FB.IsLoggedIn)

        {

            print("Logging Out");

            yield return null;

        }

        print("Logout Successful");

        FB_useerDp.sprite = null;

        FB_userName.text = "";

    }





    public void GetFriendsPlayingThisGame()

    {

        string query = "/me/friends";

        FB.API(query, HttpMethod.GET, result =>

        {

            Debug.Log("the raw" + result.RawResult);

            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);

            var friendsList = (List<object>)dictionary["data"];









            foreach (var dict in friendsList)

            {



                GameObject go = Instantiate(friendstxtprefab);

                go.GetComponent<Text>().text = ((Dictionary<string, object>)dict)["name"].ToString();

                go.transform.SetParent(GetFriendsPos.transform, false);



                //  FriendsText[1].text += ((Dictionary<string, object>)dict)["name"];

            }







        });



    }

    public void FacebookSharefeed()

    {

        string url = "https://developers.facebook.com/docs/unity/reference/current/FB.ShareLink";

        FB.ShareLink(

            new Uri(url),

            "Checkout unity3d teacher channel",

            "I just watched " + "22" + " times of this channel",

            null,

            ShareCallback);



    }

    private static void ShareCallback(IShareResult result)

    {

        Debug.Log("ShareCallback");

        SpentCoins(2, "sharelink");

        if (result.Error != null)

        {

            Debug.LogError(result.Error);

            return;

        }

        Debug.Log(result.RawResult);

    }

    // Start is called before the first frame update

    void AuthCallBack(IResult result)

    {

        if (result.Error != null)

        {

            Debug.Log(result.Error);

        }

        else

        {

            if (FB.IsLoggedIn)

            {

                Debug.Log("Facebook is Login!");

                // Panel_Add.SetActive(true);

            }

            else

            {

                Debug.Log("Facebook is not Logged in!");

            }

            DealWithFbMenus(FB.IsLoggedIn);

        }

    }



    void DealWithFbMenus(bool isLoggedIn)

    {

        if (isLoggedIn)

        {

            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);

            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);

        }

        else

        {



        }

    }

    void DisplayUsername(IResult result)

    {

        if (result.Error == null)

        {

            string name = "" + result.ResultDictionary["first_name"];

            FB_userName.text = name;



            Debug.Log("" + name);

        }

        else

        {

            Debug.Log(result.Error);

        }

    }



    void DisplayProfilePic(IGraphResult result)

    {

        if (result.Texture != null)

        {

            Debug.Log("Profile Pic");

            FB_useerDp.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());

        }

        else

        {

            Debug.Log(result.Error);

        }

    }

    public static void SpentCoins(int coins, string item)

    {

        // setup parameters

        var param = new Dictionary<string, object>();

        param[AppEventParameterName.ContentID] = item;

        // log event

        FB.LogAppEvent(AppEventName.SpentCredits, (float)coins, param);

    }



}