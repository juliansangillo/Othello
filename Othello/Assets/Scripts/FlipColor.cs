using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlipColor : MonoBehaviour
{

    public TextMeshProUGUI title;
    public Toggle blackTog;
    public Text blackTxt;
    public Toggle whiteTog;
    public Text whiteTxt;
    public Button play;
    public Text playTxt;
    public Button exit;
    public Text exitTxt;

    Color black, white;

    ColorBlock blackBlock, whiteBlock, playBlock, exitBlock;

    void Start() {

        black = new Color(0, 0, 0);
        white = new Color(255, 255, 255);
        blackBlock = blackTog.colors;
        whiteBlock = whiteTog.colors;
        playBlock = play.colors;
        exitBlock = exit.colors;

    }

    public void flipToBlack() {

        title.color = black;
        blackBlock.normalColor = black;
        blackTog.colors = blackBlock;
        blackTxt.color = white;
        whiteBlock.normalColor = black;
        whiteTog.colors = whiteBlock;
        whiteTxt.color = white;
        playBlock.normalColor = black;
        play.colors = playBlock;
        playTxt.color = white;
        exitBlock.normalColor = black;
        exit.colors = exitBlock;
        exitTxt.color = white;

        Settings.blackSetting = true;

    }

    public void flipToWhite() {

        title.color = white;
        blackBlock.normalColor = white;
        blackTog.colors = blackBlock;
        blackTxt.color = black;
        whiteBlock.normalColor = white;
        whiteTog.colors = whiteBlock;
        whiteTxt.color = black;
        playBlock.normalColor = white;
        play.colors = playBlock;
        playTxt.color = black;
        exitBlock.normalColor = white;
        exit.colors = exitBlock;
        exitTxt.color = black;

        Settings.blackSetting = false;

    }

}
