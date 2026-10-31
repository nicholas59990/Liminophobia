using UnityEngine;
using TMPro;

public class ObjectiveHUD : MonoBehaviour
{

    // Lets other scripts reach this one easily
    public static ObjectiveHUD Instance;

    // The two text objects on the screen
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI completedText;

    // The current objective (empty = no objective)
    private string currentObjective = "";

    // Finished objectives
    private string completedList = "";

    private void Awake()
    {
        Instance = this;
        UpdateScreen();
    }

    // Call this to show a new objective
    public void SetObjective(string text)
    {
        // Ignore it if it is already showing
        if (text == currentObjective)
        {
            return;
        }

        // Ignore it if the player already finished it
        if (completedList.Contains("- " + text + "\n"))
        {
            return;
        }

        currentObjective = text;
        UpdateScreen();
    }

    // Call this when the objective is done
    public void CompleteObjective()
    {
        // Do nothing if no objective
        if (currentObjective == "")
        {
            return;
        }

        // Move it to the completed list
        completedList = completedList + "- " + currentObjective + "\n";
        currentObjective = "";
        UpdateScreen();
    }

    // Writes everything on the screen
    public void UpdateScreen()
    {
        string title = "Objective Tracker";

        if (currentObjective == "")
        {
            // No objective so only title
            objectiveText.text = title;
        }
        else
        {
            // Title on line one, task on line two
            objectiveText.text = title + "\n- " + currentObjective;
        }

        // The completed list (hidden in the scene)
        if (completedList == "")
        {
            completedText.text = "";
        }
        else 
        {
            completedText.text = "Completed:\n" + completedList;
        }
    }
}
