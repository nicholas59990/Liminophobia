using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // Other scripts (like PlayerMovement) check this to freeze the player
    public static bool IsPlaying { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button[] choiceButtons;

    // Seconds between each letter
    [SerializeField] private float typingSpeed = 0.04f;

    // The Audio Source on this object
    private AudioSource typingSource;

    private Story story;
    private int startFrame;

    // True while the letters are still appearing
    private bool isTyping;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        Instance = this;
        IsPlaying = false;
        dialoguePanel.SetActive(false);

        // Find the Audio Source on this object
        typingSource = GetComponent<AudioSource>();
        if (typingSource != null)
        {
            typingSource.loop = true;
            typingSource.playOnAwake = false;
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        if (IsPlaying) return;

        story = new Story(inkJSON.text);
        IsPlaying = true;
        startFrame = Time.frameCount;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void Update()
    {
        if (!IsPlaying) return;

        // Ignore the same key press that started the dialogue
        if (Time.frameCount == startFrame) return;

        bool pressed = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        if (!pressed) return;

        if (isTyping)
        {
            // Pressing a key while typing shows the whole line at once
            FinishTyping();
        }
        else if (story.currentChoices.Count == 0)
        {
            // No choices on screen, so go to next line
            ContinueStory();
        }
    }

    private void ContinueStory()
    {
        if (story.canContinue)
        {
            string line = story.Continue().Trim();

            // Hide the choices until the line is finished
            HideChoices();

            typingCoroutine = StartCoroutine(TypeLine(line));
        }
        else
        {
            EndDialogue();
        }
    }

    // Shows the line one letter at a time
    private IEnumerator TypeLine(string line)
    {
        isTyping = true;

        // Set the text but show 0 letters
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.ForceMeshUpdate();

        int totalLetters = dialogueText.textInfo.characterCount;

        // Start the keyboard sound
        if (typingSource != null)
        {
            typingSource.Play();
        }

        // Show one more letter each time
        for (int i = 1; i <= totalLetters; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }

        EndTyping();
    }

    // Shows the full line right away
    private void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        EndTyping();
    }

    // Called when the whole line is visible
    private void EndTyping()
    {
        isTyping = false;
        dialogueText.maxVisibleCharacters = 99999;

        // Stop the keyboard sound
        if (typingSource != null)
        {
            typingSource.Stop();
        }

        ShowChoices();
    }

    private void HideChoices()
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }
    }

    private void ShowChoices()
    {
        int count = story.currentChoices.Count;

        // Free the mouse so the player can click choices
        Cursor.lockState = count > 0 ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = count > 0;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < count)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = story.currentChoices[i].text;

                int index = i; // copy for the click event
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => MakeChoice(index));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void MakeChoice(int index)
    {
        story.ChooseChoiceIndex(index);
        ContinueStory();
    }

    private void EndDialogue()
    {

        // Stop typing and the sound
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        isTyping = false;

        if (typingSource != null)
        {
            typingSource.Stop();
        }

        // Read the objective the story set
        string newObjective = story.variablesState["objective"].ToString();

        // Only show it if there is one
        if (newObjective != "" && ObjectiveHUD.Instance != null)
        {
            ObjectiveHUD.Instance.SetObjective(newObjective);
        }

        IsPlaying = false;
        dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}