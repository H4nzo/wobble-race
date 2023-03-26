using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/*GGQVliYnBIREIycWRjT2hTZAXNHUV9ITmlaVzlSNVo5SkVsQko5NHB1MmhESlpKRnFEelYzUTgtejA0MFdDUm9iajRTcHRoazdWWnBWRmlmNlBiM055MEtJc1Vib3JUTkpaS1dmM1R3RThzLWVYRkdzWTRiZAER5ZAENKX2R3SUtSTzZAwZAwZDZD
*/

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _profileImage;
    [SerializeField] private Sprite _noProfileSprite;
   // [SerializeField] private GameObject _postLoginActions;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject LoginBTN, LogOutBTN;

    private void Start()
    {
        if (FacebookAndPlayFabManager.Instance.IsLoggedOnFacebook)
        {
            _nameText.text = FacebookAndPlayFabManager.Instance.FacebookUserName;
            _profileImage.sprite = FacebookAndPlayFabManager.Instance.FacebookUserPictureSprite;
            // _postLoginActions.SetActive(true);
        }
    }

    public void LoginWithFacebook()
    {
        if (string.IsNullOrEmpty(FacebookAndPlayFabManager.Instance.PlayFabTitleId))
        {
            Debug.LogError("PlayFabTitleId is null.");
            return;
        }

        if (FacebookAndPlayFabManager.Instance.IsLoggedOnFacebook)
            return;

        FacebookAndPlayFabManager.Instance.LogOnFacebook(successCallback: res =>
        {
            StartCoroutine(GetUserNameRoutine());
            StartCoroutine(GetUserPictureRoutine());
            StartCoroutine(WaitForPlayFabLogin());
            

        });
    }

    // Shows the player's Facebook name as soon as it's available.
    private IEnumerator GetUserNameRoutine()
    {
        yield return new WaitUntil(() => !string.IsNullOrEmpty(FacebookAndPlayFabManager.Instance.FacebookUserName));

        _nameText.text = FacebookAndPlayFabManager.Instance.FacebookUserName;
    }

    // Shows the player's Facebook picture as soon as it's available.
    private IEnumerator GetUserPictureRoutine()
    {
        yield return new WaitUntil(() => FacebookAndPlayFabManager.Instance.FacebookUserPictureSprite != null);

        _profileImage.sprite = FacebookAndPlayFabManager.Instance.FacebookUserPictureSprite;
    }

    // Enable a set of buttons as soon as the player gets logged on PlayFab.
    private IEnumerator WaitForPlayFabLogin()
    {
        yield return new WaitUntil(() => FacebookAndPlayFabManager.Instance.IsLoggedOnPlayFab);

        // _postLoginActions.SetActive(true);
        LoginBTN.SetActive(false);
        LogOutBTN.SetActive(true);
    }

    public void Logout()
    {
        FacebookAndPlayFabManager.Instance.LogOutFacebook();
        _profileImage.sprite = _noProfileSprite;
        _nameText.text = "Offline";
        // _postLoginActions.SetActive(false);
        LoginBTN.SetActive(!false);
        LogOutBTN.SetActive(!true);
    }

    public void PostScoreOnPlayFab()
    {
        if (string.IsNullOrEmpty(_scoreText.text))
            return;

        var score = PlayerPrefs.GetInt("HIGH_SCORE");
        FacebookAndPlayFabManager.Instance.UpdateStat(Constants.LeaderboardName, score);
    }

    public void SeeLeaderboard()
    {
        PostScoreOnPlayFab();
        SceneManager.LoadScene("02_LeaderboardMenu");
    }
}