using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator crossfadeTransition;
    public Animator canvasCamera;
    public Animator CanvasOverlay;
    public Animator GameField;
    public LevelJunkoDescriptor levelDescriptor;
    public AudioSource audioSource;
    public float timeToDelay;

    // Start is called before the first frame update
    public void PrepareToStart()
    {
        StartCoroutine(StartLevel());
    }

    public void ChangeLevel(int index)
    {
        StartCoroutine(NextLevel(index));
    }

    IEnumerator StartLevel()
    {
        GameField.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(timeToDelay);
        audioSource.Play();
        levelDescriptor.LevelStart();
    }

    IEnumerator NextLevel(int index)
    {
        yield return new WaitForSecondsRealtime(timeToDelay);
        CanvasOverlay.SetTrigger("Level_End");
        canvasCamera.SetTrigger("Level_End");
        crossfadeTransition.SetTrigger("Level_End");
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("GameOver");
    }
}
