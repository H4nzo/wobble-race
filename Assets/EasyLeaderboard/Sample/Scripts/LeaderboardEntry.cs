using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    public Image userPictureImage;
    public Image userPositionBox;
    public Text userPositionText;
    public Text userNameText;
    public Text userScoreText;

    [Header("First Places colors")]
    public Color golden;
    public Color silver;
    public Color bronze;

    public void SetUserPictureSprite(Sprite userPictureSprite)
    {
        userPictureImage.sprite = userPictureSprite;
    }

    public void SetUserPosition(int userPosition)
    {
        userPositionText.text = userPosition.ToString();

        switch (userPosition)
        {
            case 1:
                userPositionBox.color = golden;
                break;
            case 2:
                userPositionBox.color = silver;
                break;
            case 3:
                userPositionBox.color = bronze;
                break;
        }
    }

    public void SetUserName(string userName)
    {
        userNameText.text = userName;
    }

    public void SetUserScore(string userScore)
    {
        userScoreText.text = userScore;
    }

    private void SetUserPositionBoxColor(Color color)
    {
        userPositionBox.color = color;
    }
}
