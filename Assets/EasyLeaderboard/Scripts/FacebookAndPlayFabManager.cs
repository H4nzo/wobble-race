using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookAndPlayFabManager : MonoSingleton<FacebookAndPlayFabManager>
{
    private string[] _permissions = { "public_profile", "email", "user_friends" };
    private const int PictureWidth = 140;
    private const int PictureHeight = 140;

    [Header("PlayFab Info")]
    [Tooltip("ID of your application on PlayFab")]
    [SerializeField]
    private string _playFabTitleId;

    [Header("Facebook Share Info")]
    [Tooltip("Application URL")]
    [SerializeField]
    private string _contentUrl;

    [Tooltip("Application name")]
    [SerializeField]
    private string _contentTitle;

    [TextArea]
    [Tooltip("Message to your Facebook friends")]
    [SerializeField]
    private string _contentDescription;

    [Tooltip("Application image URL")]
    [SerializeField]
    private string _pictureUrl;

    public string PlayFabTitleId { get { return _playFabTitleId; } }
    public string PlayFabUserId { get; private set; }
    public bool IsLoggedOnPlayFab { get; private set; }

    public bool IsLoggedOnFacebook { get; private set; }
    public string FacebookUserId { get; private set; }
    public string FacebookUserName { get; private set; }
    public Sprite FacebookUserPictureSprite { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        Debug.Log("EasyLeaderboard.Awake => FB.IsInitialized: " + FB.IsInitialized);

        if (FB.IsInitialized)
            return;

        FB.Init(() => FB.ActivateApp());
    }

    private void Start()
    {
        PlayFabSettings.TitleId = _playFabTitleId;
    }

    /// <summary>
    /// Logs the player into Facebook.
    /// </summary>
    /// <param name="successCallback">Action to be executed when the process is done correctly.</param>
    /// <param name="errorCallback">Action to be executed when the process fails.</param>
    public void LogOnFacebook(Action<ILoginResult> successCallback = null, Action<ILoginResult> errorCallback = null)
    {
        Debug.Log("EasyLeaderboard.LogOnFacebook => FB.IsInitialized: " + FB.IsInitialized);
        Debug.Log("EasyLeaderboard.LogOnFacebook => FB.IsLoggedIn: " + FB.IsLoggedIn);

        if (FB.IsLoggedIn)
        {
            SetLoggedInfo();

            if (successCallback != null)
                successCallback(null);

            return;
        }

        FB.LogInWithReadPermissions(_permissions,
            (res =>
            {
                if (!ValidateResult(res))
                {
                    if (errorCallback != null)
                        errorCallback(res);

                    return;
                }

                FacebookUserId = res.ResultDictionary["user_id"].ToString();

                Debug.Log("EasyLeaderboard.LogOnFacebook => FB.IsLoggedIn: " + FB.IsLoggedIn);

                SetLoggedInfo();

                if (successCallback != null)
                    successCallback(res);
            }));
    }

    private void SetLoggedInfo()
    {
        IsLoggedOnFacebook = true;

        GetFacebookUserName("me", res =>
        {
            FacebookUserName = res.ResultDictionary["name"].ToString();
        });

        LogOnPlayFab();
    }

    /// <summary>
    /// Gets the player's Facebook profile name.
    /// </summary>
    /// <param name="id">Unique identifier of a Facebook profile.</param>
    /// <param name="successCallback">Action to be executed when the process is done correctly.</param>
    /// <param name="errorCallback">Action to be executed when the process fails.</param>
    public void GetFacebookUserName(string id, Action<IGraphResult> successCallback = null, Action<IGraphResult> errorCallback = null)
    {
        FB.API("/" + id, HttpMethod.GET,
            (res =>
            {
                if (!ValidateResult(res))
                {
                    if (errorCallback != null)
                        errorCallback(res);

                    return;
                }

                Debug.Log(string.Format("FacebookAndPlayFabManager.GetFacebookUserName => Success! (name: {0})",
                    res.ResultDictionary["name"]));

                if (successCallback != null)
                    successCallback(res);
            }));
    }

    /// <summary>
    /// Gets the player's Facebook profile picture.
    /// </summary>
    /// <param name="id">Unique identifier of a Facebook profile.</param>
    /// <param name="width">Width the returning image is supposed to have.</param>
    /// <param name="height">Height the returning image is supposed to have.</param>
    /// <param name="successCallback">Action to be executed when the process is done correctly.</param>
    /// <param name="errorCallback">Action to be executed when the process fails.</param>
    public void GetFacebookUserPicture(string id, int width, int height, Action<IGraphResult> successCallback = null, Action<IGraphResult> errorCallback = null)
    {
        string query = string.Format("/{0}/picture?type=square&height={1}&width={2}", id, height, width);

        FB.API(query, HttpMethod.GET,
            (res =>
            {
                if (!ValidateResult(res) || res.Texture == null)
                {
                    if (errorCallback != null)
                        errorCallback(res);

                    return;
                }

                if (successCallback != null)
                    successCallback(res);

                Debug.Log("EasyLeaderboard.GetFacebookUserPicture => Success!");
            }));
    }

    public void GetFacebookUserPictureFromUrl(string id, int width, int height, Action<IGraphResult> successCallback = null, Action<IGraphResult> errorCallback = null)
    {
        string query = string.Format("/{0}/picture?type=square&height={1}&width={2}&redirect=false", id, height, width);

        FB.API(query, HttpMethod.GET,
            (res =>
            {
                if (!ValidateResult(res))
                {
                    if (errorCallback != null)
                        errorCallback(res);

                    return;
                }

                if (successCallback != null)
                    successCallback(res);

                Debug.Log("EasyLeaderboard.GetFacebookUserPictureUrl => Success!");
            }));
    }

    public IEnumerator GetTextureFromGraphResult(IGraphResult result, Action<Texture2D> callback)
    {
        var data = result.ResultDictionary["data"] as IDictionary<string, object>;
        var url = data["url"].ToString();

        WWW request = new WWW(url);

        yield return new WaitUntil(() => request.isDone);

        if (callback != null)
            callback(request.texture);
    }

    /// <summary>
    /// Logs the player out of Facebook.
    /// </summary>
    public void LogOutFacebook()
    {
        FB.LogOut();

        IsLoggedOnFacebook = false;
        IsLoggedOnPlayFab = false;
        FacebookUserName = string.Empty;
        FacebookUserPictureSprite = null;
    }

    private void LogOnPlayFab()
    {
        if (IsLoggedOnPlayFab)
            return;

        var loginWithFacebookRequest = new LoginWithFacebookRequest
        {
            TitleId = PlayFabSettings.TitleId,
            AccessToken = AccessToken.CurrentAccessToken.TokenString,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithFacebook(loginWithFacebookRequest, PlayFabLoginSuccessCallback, PlayFabErrorCallback);
    }

    private void PlayFabLoginSuccessCallback(PlayFab.ClientModels.LoginResult result)
    {
        Debug.Log("EasyLeaderboard.LogOnPlayFab => Success!");

        IsLoggedOnPlayFab = true;
        PlayFabUserId = result.PlayFabId;

        GetFacebookUserPicture("me", PictureHeight, PictureWidth, res =>
        {
            FacebookUserPictureSprite = Sprite.Create(res.Texture, new Rect(0, 0, PictureHeight, PictureWidth), Vector2.zero);
        });

        // ATTENTION:
        // If you're having trouble getting the profile picture please comment the call above and uncomment the following.

        //GetFacebookUserPictureFromUrl("me", PictureWidth, PictureHeight, res =>
        //{
        //    StartCoroutine(GetTextureFromGraphResult(res, tex =>
        //    {
        //        FacebookUserPictureSprite = Sprite.Create(tex, new Rect(0, 0, PictureWidth, PictureHeight), Vector2.zero);
        //    }));
        //});

        UpdatePlayFabDisplayUserName();
    }

    private void PlayFabErrorCallback(PlayFabError error)
    {
        if (error == null || string.IsNullOrEmpty(error.ErrorMessage))
        {
            Debug.LogError("EasyLeaderboard.PlayFabErrorCallback => [No error message from PlayFab]");
            return;
        }

        string message = string.Format("EasyLeaderboard.PlayFabErrorCallback => {0}\r\n", error.ErrorMessage);

        if (error.ErrorDetails != null)
        {
            foreach (var item in error.ErrorDetails)
                message += string.Format("{0} => {1}\r\n", item.Key, string.Join(" | ", item.Value.ToArray()));
        }

        Debug.LogError(message);
    }

    /// <summary>
    /// Updates the value of a given PlayFab statistic.
    /// </summary>
    /// <param name="statisticName">Name of the PlayFab statistic to be updated.</param>
    /// <param name="value">Value the statistic must receive.</param>
    /// <param name="successCallback">Action to be executed when the process is done correctly.</param>
    public void UpdateStat(string statisticName, int value, Action<UpdatePlayerStatisticsResult> successCallback = null)
    {
        if (!IsLoggedOnPlayFab)
        {
            Debug.LogError("EasyLeaderboard.UpdateStat => Not logged on PlayFab!");
            return;
        }

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = statisticName,
                    Value = value
                }
            }
        };

        successCallback += (result) =>
        {
            Debug.Log(string.Format("EasyLeaderboard.UpdateStat => Success! (Statistic '{0}', value {1})",
                statisticName, value));
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, successCallback, PlayFabErrorCallback);
    }

    private void UpdatePlayFabDisplayUserName()
    {
        if (!IsLoggedOnPlayFab)
        {
            Debug.LogError("EasyLeaderboard.UpdatePlayFabDisplayUserName => Not logged on PlayFab!");
            return;
        }

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = FacebookUserId
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, null, PlayFabErrorCallback);
    }

    /// <summary>
    /// Gets values from a given PlayFab statistic.
    /// </summary>
    /// <param name="statisticName">Name of the PlayFab statistic to be retrieved.</param>
    /// <param name="friendsOnly">Only retrieve info from Facebook friends?</param>
    /// <param name="maxResultsCount">Maximum number of records to retrieve.</param>
    /// <param name="successCallback">Action to be executed when the process is done correctly.</param>
    /// <param name="startPosition">Starting point to retrieve the statistic values.</param>
    public void GetLeaderboard(string statisticName, bool friendsOnly, int maxResultsCount, Action<GetLeaderboardResult> successCallback, int startPosition = 0)
    {
        if (!IsLoggedOnPlayFab)
        {
            Debug.LogError("EasyLeaderboard.GetLeaderboard => Not logged on PlayFab!");
            return;
        }

        successCallback += res =>
        {
            Debug.Log(string.Format("EasyLeaderboard.GetLeaderboard => Success! (Statistic '{0}', {1}, {2} items found)",
                statisticName, friendsOnly ? "Friends" : "Global", res.Leaderboard.Count));
        };

        if (friendsOnly)
        {
            var request = new GetFriendLeaderboardRequest
            {
                StatisticName = statisticName,
                MaxResultsCount = maxResultsCount,
                IncludeFacebookFriends = true,
                StartPosition = startPosition
            };

            PlayFabClientAPI.GetFriendLeaderboard(request, successCallback, PlayFabErrorCallback);
        }
        else
        {
            var request = new GetLeaderboardRequest
            {
                StatisticName = statisticName,
                MaxResultsCount = maxResultsCount,
                StartPosition = startPosition
            };

            PlayFabClientAPI.GetLeaderboard(request, successCallback, PlayFabErrorCallback);
        }
    }

    /// <summary>
    /// Shares a post on Facebook.
    /// </summary>
    /// <param name="successCallback">Action to be executed when the process is done correctly.</param>
    /// <param name="errorCallback">Action to be executed when the process fails.</param>
    public void ShareOnFacebook(Action<IShareResult> successCallback = null, Action<IShareResult> errorCallback = null)
    {
        if (!IsLoggedOnFacebook)
        {
            Debug.Log("EasyLeaderboard.ShareOnFacebook => Not logged on Facebook!");
            return;
        }

        FB.ShareLink(new Uri(_contentUrl), _contentTitle, _contentDescription, new Uri(_pictureUrl),
            res =>
            {
                if (!ValidateResult(res))
                {
                    if (errorCallback != null)
                        errorCallback(res);

                    return;
                }

                if (successCallback != null)
                    successCallback(res);
            });
    }

    private bool ValidateResult(IResult result)
    {
        if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
            return true;

        Debug.LogError(string.Format("{0} is invalid (Cancelled={1}, Error={2}, JSON={3})",
            result.GetType(), result.Cancelled, result.Error, Facebook.MiniJSON.Json.Serialize(result.ResultDictionary)));

        return false;
    }
}
