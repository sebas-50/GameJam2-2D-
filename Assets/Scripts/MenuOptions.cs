using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuOptions : MonoBehaviour
{
    public float transitionTime = 3f;
    private float timer;
    public AudioClip audioOnClick;
    [SerializeField]private GameObject[] availablePanels; 


    private void Start() {
        foreach(GameObject i in availablePanels)
        {
            i.SetActive(false);
        }
        availablePanels[0].SetActive(true);
    }

//====================================================================================


    public void StartGame()
    {
        StartCoroutine(LoadSceneCoroutine(1));
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadSceneCoroutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void ReturnToMenu()
    {
        StartCoroutine(LoadSceneCoroutine(0));
        Debug.Log("it got here");
    }
    

    private IEnumerator LoadSceneCoroutine(int sceneToLoad)
    {   
        //StartCoroutine(TurnMusicDown());   
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(sceneToLoad);      
    }


    public void QuitGame()
    {
        StartCoroutine(QuitGameCoroutine());
    }
    
    private IEnumerator QuitGameCoroutine()
    {   
        //StartCoroutine(TurnMusicDown());   
        yield return new WaitForSecondsRealtime(transitionTime);
        Application.Quit();       
    }

    


    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
/*
    public void PlayButonSound()
    {
        AudioManager.Instance.PlaySFX(audioOnClick); 
    }

*/

/*
    IEnumerator TurnMusicDown()
    {

        AudioManager.Instance.PlaySFX(1f, audioOnClick); 

        if (AudioManager.Instance.CurrentMusicVolume() <= 0f) yield return new WaitForSeconds(transitionTime);

        else
        {
            float musicVolume;
            float currentMaxVolume = AudioManager.Instance.CurrentMusicVolume();

            while (AudioManager.Instance.CurrentMusicVolume() > 0f)
            {
                
                timer += Time.deltaTime;
                musicVolume = Mathf.Lerp(currentMaxVolume, 0f, timer / transitionTime);
                AudioManager.Instance.ChangeMusicVolume(musicVolume);
                yield return null;
            }
        }  

    }

    */
}
