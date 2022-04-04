using UnityEngine;
using UnityEngine.UI;

public class BlueprintsAndBackButtonsScript : MonoBehaviour
{
    //Setting game objects for image of blueprints and the button to get back
    //to the level after pressing the blueprints button
    [SerializeField] private GameObject blueprintsImage;
    [SerializeField] private Button backButton;
    private bool blueprintView;
    private bool firstPress;
    private bool secondPress;

    private void Start()
    {
        blueprintView = false;
        firstPress = false;
        secondPress = false;
        blueprintsImage.GetComponent<Image>().color 
            = new Color(225, 225, 225, 0);
    }
    public GameObject BlueprintsImage { get => blueprintsImage; 
        set => blueprintsImage = value; }
    public Button BackButton1 { get => backButton; set => backButton = value; }

    /// <summary>
    /// Function that allows the blueprints button to spawn the blueprints
    /// image and the back button
    /// </summary>
    public void BlueprintsButton()
    {
        if(blueprintView == true && secondPress == true)
        {
            blueprintsImage.GetComponent<Image>().color
            = new Color(225, 225, 225, 225);
            blueprintView = false;
            print("blueprint appear");
        }
        /*else if(blueprintView == true && secondPress == false)
        {
            blueprintsImage.GetComponent<Image>().color
            = new Color(225, 225, 225, 0);
            blueprintView = false;
            secondPress = true;
        }*/
        else if(blueprintView == false && firstPress == false)
        {
            blueprintsImage.GetComponent<Image>().color
            = new Color(225, 225, 225, 225);
            firstPress = true;
            blueprintView = true;
        }
        /*else if(blueprintView == false && firstPress == true)
        {
            blueprintsImage.GetComponent<Image>().color
            = new Color(225, 225, 225, 0);
            blueprintView = true;
            firstPress = true;
            print("blueprint disappear");
        }*/
    }

    public void InvisibleButton()
    {
        if (blueprintView == true && secondPress == false)
        {
            blueprintsImage.GetComponent<Image>().color
            = new Color(225, 225, 225, 0);
            blueprintView = false;
            secondPress = true;
        }
        else if (blueprintView == false && firstPress == true)
        {
            blueprintsImage.GetComponent<Image>().color
            = new Color(225, 225, 225, 0);
            blueprintView = true;
            firstPress = true;
            print("blueprint disappear");
        }
    }
}
