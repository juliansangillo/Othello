using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void toGame() {

        SceneManager.LoadScene(1);

    }

    public void toMenu() {

        SceneManager.LoadScene(0);

    }

    public void quit() {

        Application.Quit();

    }

}
