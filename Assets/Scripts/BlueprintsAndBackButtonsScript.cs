using UnityEngine;
using UnityEngine.UI;

public class BlueprintsAndBackButtonsScript : MonoBehaviour
{
    //Setting game objects for image of blueprints and the button to get back
    //to the level after pressing the blueprints button
    [SerializeField] private GameObject blueprintsImage;
    [SerializeField] private Button backButton;

    public GameObject BlueprintsImage { get => blueprintsImage; 
        set => blueprintsImage = value; }
    public Button BackButton1 { get => backButton; set => backButton = value; }

    /// <summary>
    /// Function that allows the back button to destroy the blueprints and 
    /// itself so the player can see the gameplay agian
    /// </summary>
    public void BackButton()
    {
        Destroy(blueprintsImage);
        Destroy(backButton);
    }

    /// <summary>
    /// Function that allows the blueprints button to spawn the blueprints
    /// image and the back button
    /// </summary>
    public void BlueprintsButton()
    {
        Instantiate(blueprintsImage, new Vector3(0, 0, 0), 
            Quaternion.identity);
        Instantiate(backButton, new Vector3(1094, 570, 0), 
            Quaternion.Euler(0, 0, 0));
    }
}
