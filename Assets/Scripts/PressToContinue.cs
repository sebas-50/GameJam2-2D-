using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;


public class PressToContinue : MonoBehaviour
{
    private PlayerInput playerInput;
    public float transitionTime = 3f;
    private void Start() {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update() {
        if (playerInput.actions["Interact"].IsPressed())
        {
            StartCoroutine(LoadSceneCoroutine(2));
        }
    }

        private IEnumerator LoadSceneCoroutine(int sceneToLoad)
    {   
        Debug.Log("GotToCoroutine");
        //StartCoroutine(TurnMusicDown());   
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(sceneToLoad);      
    }
}
