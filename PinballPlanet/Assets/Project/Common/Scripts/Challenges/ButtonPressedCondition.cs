using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A condition that is met by comparing the score.
/// </summary>
class ButtonPressedCondition : Condition
{
    // Button(s) to press.
    private string _buttonToPressName = "";
    private List<Button> _buttonsToPress = new List<Button>();

    // Constructor.
    public ButtonPressedCondition(string buttonToPressName)
    {
        // Store object to hit.
        _buttonToPressName = buttonToPressName;
    }

    // Constructor.
    public ButtonPressedCondition()
    { }

    // Internal function used to check condition met.
    protected override bool IsInternallyMet()
    {
        // Find buttons if none are found.
        if (_buttonsToPress.Count < 1)
            FindButtons();

        // Check if any button was pressed.
        foreach (Button button in _buttonsToPress)
        {
            if (button.pressed)
            {
                // Keep pressed set to true as the button's default behaviour is to set it to false when .pressed is called.
                button.pressed = true;
                return true;
            }
        }

        return false;
    }

    // Initialize from a string parameters dictionary. 
    public override void InitializeFromParameters(System.Collections.Generic.Dictionary<string, string> parameters)
    {
        // Name
        string key = "Name";
        if (TryParseParameter(key, out _buttonToPressName, "", ref parameters))
            parameters.Remove(key);

        // Base initialize.
        base.InitializeFromParameters(parameters);
    }

    // Called when new level was loaded.
    public override void OnLevelWasLoaded()
    {
        FindButtons();

        base.OnLevelWasLoaded();
    }

    // Looks for buttons with right name.
    private void FindButtons()
    {
        // Clear list.
        _buttonsToPress.Clear();

        // Find buttons with name.
        // This will also add prefabs not in scene, not really intended but causes no problems.
        foreach (object button in Resources.FindObjectsOfTypeAll(typeof(Button)))
        {
            Button butt = button as Button;
            if (butt.name == _buttonToPressName)
            {
                _buttonsToPress.Add(butt);
            }
        }
    }
}
