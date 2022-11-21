using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    private List<int> playerTaskList = new List<int>();
    private List<int> playerSequenceList = new List<int>();

    public List<AudioClip> buttonSoundList = new List<AudioClip>();
    public List<List<Color32>> buttonColors = new List<List<Color32>>();
    public List<Button> clickableButtons;

    public AudioClip loseSound;
    public AudioClip startSound;
    public AudioSource audioSource;
    public CanvasGroup buttons;
    public GameObject startButton;
    [SerializeField]
    float delay = 0.7f;
    bool alive = true;
    int scorePoints = 0;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI gameOver;
    void Awake()
    {
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: " + scorePoints;
        buttonColors.Add(new List<Color32> { new Color32(255, 100, 100, 255), new Color32(255, 0, 0, 255) });// add red
        buttonColors.Add(new List<Color32> { new Color32(255, 187, 109, 255), new Color32(255, 140, 0, 255) });// add orange
        buttonColors.Add(new List<Color32> { new Color32(162, 255, 124, 255), new Color32(0, 255, 0, 255) });// add green
        buttonColors.Add(new List<Color32> { new Color32(57, 111, 255, 255), new Color32(0, 0, 255, 255) });// add blue
        for (int i = 0; i < 4; i++)
        {
            clickableButtons[i].GetComponent<Image>().color = buttonColors[i][0];
        }
    }
    public void AddToPlayerSequenceList(int buttonId)
    {
        StartCoroutine(HighlightButton(buttonId));
        playerSequenceList.Add(buttonId);

        for (int i = 0; i < playerSequenceList.Count; i++)
        {
            if (playerTaskList[i] == playerSequenceList[i])
            {
                continue;
            }
            else
            {
                StartCoroutine(PlayerLost());
                return;
            }
        }
        if (!alive)
        {
            return;
        }
        if (playerSequenceList.Count == playerTaskList.Count)
        {
            scorePoints++;
            scoreText.text = "Score: " + scorePoints;
            if (scorePoints % 4 == 0 && scorePoints < 17)
            {
                delay = delay - 0.1f;
            }
            StartCoroutine(StartNextRound());
        }
    }
    public void StartGame()
    {
        alive = true;
        scoreText.text = "Score: " + scorePoints;
        //enable gameover text
        gameOver.enabled = false;

        StartCoroutine(StartNextRound());
        startButton.SetActive(false);
    }

    IEnumerator PlayerLost()
    {
        gameOver.enabled = true;
        audioSource.PlayOneShot(loseSound);
        playerSequenceList.Clear();
        playerTaskList.Clear();
        delay = 0.8f;
        scorePoints = 0;
        alive = false;
        yield return new WaitForSeconds(3f);
        startButton.SetActive(true);
    }
    IEnumerator HighlightButton(int buttonId)
    {
        clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][1];
        audioSource.PlayOneShot(buttonSoundList[buttonId]);
        yield return new WaitForSeconds(0.6f);
        clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][0];
    }
    IEnumerator StartNextRound()
    {
        playerSequenceList.Clear();
        buttons.interactable = false;
        yield return new WaitForSeconds(0.7f);
        playerTaskList.Add(Random.Range(0, 4));
        foreach (int index in playerTaskList)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(HighlightButton(index));
        }
        yield return new WaitForSeconds(0.4f);
        buttons.interactable = true;
        yield return null;
    }
}
