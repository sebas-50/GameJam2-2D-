using UnityEngine;

public class SwitchPanel : MonoBehaviour
{
    public void SwitchToPanel(GameObject panelToSwitch)
    {
        panelToSwitch.SetActive(true);
        gameObject.SetActive(false);
    }

}
