using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class EndGamePoint : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource upSound;

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

        // Load the next level (in this case, "Demo")
        SceneManager.LoadScene("Demo");
    }
}
