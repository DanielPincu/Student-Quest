using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePoint : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource upSound;
    [SerializeField] private string levelToLoad; // Set the scene name directly in the Inspector

    private bool isActive = false;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive)
            return;

        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Play");
            upSound.Play();
            GameManager.instance.StopTimer();
            UIController.instance.ShowEndGame();
            isActive = true;

            // Start the level transition coroutine
            StartCoroutine(TransitionToNextLevel(10f));
        }
    }

    private IEnumerator TransitionToNextLevel(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Check if levelToLoad is specified and load it
        if (!string.IsNullOrEmpty(levelToLoad))
        {
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            Debug.LogWarning("No level specified to load.");
        }
    }
}
