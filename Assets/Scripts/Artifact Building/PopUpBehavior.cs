using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpBehavior : MonoBehaviour
{
    ///<summary>
    /// Graphic to reward player for connect a piece
    ///</summary>
    public GameObject snapGraphic;

    /// <summary>
    /// Stella graphic for the CorrectSnapUI scnrren
    /// </summary>
    public GameObject stella;

    /// <summary>
    /// Alistar graphic for the CorrectSnapUI scnrren
    /// </summary>
    public GameObject alistar;

    /// <summary>
    /// Time it takes for the reward popup to disappear
    /// </summary>
    public float seconds = 1f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Decides what character will popup to reward the player upon getting a
    /// correct snapped piece
    /// </summary>
    IEnumerator CorrectSnapUI()
    {

        float characterChoice = Random.Range(1, 3);

        if (characterChoice == 1)
        {
            stella.SetActive(false);
            alistar.SetActive(true);
        }
        else if (characterChoice == 2)
        {
            stella.SetActive(true);
            alistar.SetActive(false);
        }

        yield return new WaitForSeconds(seconds);
        snapGraphic.SetActive(false);
    }
}
