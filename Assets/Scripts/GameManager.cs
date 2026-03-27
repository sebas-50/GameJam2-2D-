using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]Timer timerRef;
    [SerializeField] CatCounter catCounterRef;
    [SerializeField]float transitionTime = 3f;
    [SerializeField]public float timeInMinutes = 10f; // Ejemplo: 1 minuto y 30 segundos
    private MenuOptions menuOptionsRef;

    
    void Start()
    {
        timerRef.timeRemainingInMinutes = timeInMinutes;
        menuOptionsRef = GameObject.Find("Game UI").GetComponent<MenuOptions>();
    }

    private void Update() {
        if (timerRef.timeRemainingInMinutes == 0) GameOver();
    }

    void GameOver()
    {
        Debug.Log("You Lost :(");
        StartCoroutine(GameOverCoroutine());
    }

    void YouWon()
    {
        menuOptionsRef.PauseGame();
        menuOptionsRef.DisplayVictory();
        Debug.Log("You Won!");
    }

    private IEnumerator GameOverCoroutine()
    {   
        //StartCoroutine(TurnMusicDown());   
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(3);      
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Cat"))
        {
            catCounterRef.UpdateDisplay();
            other.gameObject.SetActive(false); // u otra lógica para disponer del gameobject del gato
            if (catCounterRef.isComplete)
            {
                timerRef.isCounting = false;
                YouWon();
            }
        }
    }

}
