using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField]public float timeRemainingInMinutes; // Ejemplo: 1 minuto y 30 segundos
    [SerializeField]private string animParameters;
    [SerializeField]private TMP_Text displayText; // O public Text timerText; para UI antigua
    private GameManager gameManagerRef;
    public bool isCounting;

    private void Start() {
        gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
        timeRemainingInMinutes *= 60;
        isCounting = true;
    }

    void Update()
    {
        if (isCounting){
            if (timeRemainingInMinutes > 0)
            {
                timeRemainingInMinutes -= Time.deltaTime;
                DisplayTime(timeRemainingInMinutes);
            }
            else
            {
                timeRemainingInMinutes = 0;
                // Aquí puedes llamar a un evento de "Tiempo terminado"
            }
        }
        
        else
        {
            displayText.text = animParameters+":)";
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Evitamos que el tiempo sea negativo en la UI
        if (timeToDisplay >= 0)
        {
            displayText.text = animParameters+":(";

            // Calculamos minutos y segundos
            int minutes = Mathf.FloorToInt(timeToDisplay / 60); 
            int seconds = Mathf.FloorToInt(timeToDisplay % 60);

            // Formateamos el string con ":00" para que siempre tenga dos dígitos
            displayText.text = animParameters + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else {
            displayText.text = animParameters+":c";
        }
        
    }


}