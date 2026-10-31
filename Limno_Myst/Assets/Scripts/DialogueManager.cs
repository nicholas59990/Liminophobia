using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // Other scripts (like PlayerMovement) check this to freeze the player
    public static bool IsPlaying { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button[] choiceButtons;

    private Story story;
    private int startFrame;

    private void Awake()
    {
        Instance = this;
        IsPlaying = false;
        dialoguePanel.SetActive(false);
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

        // Only advance with a key when there are no choices on screen
        if (story.currentChoices.Count == 0 &&
            (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            ContinueStory();
        }
    }

    private void ContinueStory()
    {
        if (story.canContinue)
        {
            dialogueText.text = story.Continue().Trim();
            ShowChoices();
        }
        else
        {
            EndDialogue();
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
        IsPlaying = false;
        dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}