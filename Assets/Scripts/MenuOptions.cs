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
        StartCoroutine(StarGameCoroutine());
    }

    private IEnumerator StarGameCoroutine()
    {   
        //StartCoroutine(TurnMusicDown());
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(1);        
    }


    public void QuitGame()
    {
        StartCoroutine(QuitGameCoroutine());
    }
    
    private IEnumerator QuitGameCoroutine()
    {   
        //StartCoroutine(TurnMusicDown());   
        yield return new WaitForSeconds(transitionTime);
        Application.Quit();       
    }

    public void RestartLevel()
    {
        StartCoroutine(RestarLevelCoroutine());
    }

     private IEnumerator RestarLevelCoroutine()
    {   
        //StartCoroutine(TurnMusicDown());   
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
