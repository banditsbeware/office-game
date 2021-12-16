using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void LawnWithToupeeMan() {
        SceneManager.LoadScene("LawnWithToupeeMan");
    }
}
