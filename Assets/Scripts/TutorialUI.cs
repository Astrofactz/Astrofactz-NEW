/*******************************************************************************
// File Name :      TutorialUI.cs
// Author :         Anthony Pollos 
// Creation Date :  13 April 2022
// 
// Description :    Manages the tutorial UI and it's messages.
*******************************************************************************/
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Tooltip("The first message to be shown")]
    public GameObject text1;

    [Tooltip("The second message to be shown")]
    public GameObject text2;

    [Tooltip("The third message to be shown")]
    public GameObject text3;


    [Tooltip("Used to count which message is being presented")]
    public int messages = 0;


    /// <summary>
    /// Starts the Tutorial UI with the first message
    /// </summary>
    private void Awake()
    {
        text1.SetActive(true);
        text2.SetActive(false);
        text3.SetActive(false);
    }


    /// <summary>
    /// This will cycle through the messages at the start of each mission
    /// </summary>
    public void TutorialMessage()
    {
        ++messages;

        if (messages == 0)
        {
            text1.SetActive(true);
            text2.SetActive(false);
            text3.SetActive(false);
        }
        if (messages == 1)
        {
            text1.SetActive(false);
            text2.SetActive(true);
            text3.SetActive(false);
        }
        if (messages == 2)
        {
            text1.SetActive(false);
            text2.SetActive(false);
            text3.SetActive(true);
        }
        if (messages == 3)
        {
            text1.SetActive(false);
            text2.SetActive(false);
            text3.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
