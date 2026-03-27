using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CatCounter : MonoBehaviour
{
    [SerializeField]List<GameObject> cats = new List<GameObject>();
    [SerializeField]private string animParameters;
    [SerializeField]private TMP_Text displayText; // O public Text timerText; para UI antigua
    private GameManager gameManagerRef;

    private int totalCats;
    private int currentCats;
    public bool isComplete;

    void Start()
    {
        gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
        foreach (GameObject cat in GameObject.FindGameObjectsWithTag("Cat"))
        {
            cats.Add(cat);
        }
        totalCats = cats.Count;
        displayText.text = animParameters + "0/" + cats.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("Display Cats Test")]
    public void UpdateDisplay()
    {
        currentCats++;
        displayText.text = animParameters + currentCats + "/" + cats.Count;
        if (currentCats == totalCats) isComplete = true;
    }

    


}
