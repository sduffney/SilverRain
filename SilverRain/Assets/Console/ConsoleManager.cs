using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ConsoleManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject consolePanel;
    public TMP_InputField inputField;
    public TextMeshProUGUI outputField;
    public KeyCode toggleKey = KeyCode.BackQuote;

    [Header("Console Settings")]
    public bool startHidden = true;
    public bool pauseWhileOpen = true;
    public bool isEnabled = true;


    private Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>();
    private bool isOpen = false;
    private float previousTimeScale = 1f;

    private ScrollRect scrollRect;
    private PlayerInput playerInput;


    private void Awake()
    {
        if (!isEnabled) return;
        RegisterDefaultCommands();
        //scrollRect = outputField.GetComponentInParent<ScrollRect>();
        playerInput = GameObject.FindWithTag("Player")?.GetComponent<PlayerInput>();

        if (consolePanel != null)
        {
            consolePanel.SetActive(!startHidden);
            isOpen = !startHidden;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleConsole();
            return;
        }

        if (isOpen && Input.GetKeyDown(KeyCode.Return))
        {
            if (inputField != null && !string.IsNullOrWhiteSpace(inputField.text))
            {
                string line = inputField.text.Trim();
                ExecuteInput(line);
                inputField.text = "";
                EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
                inputField.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
    }

    public void ToggleConsole()
    {
        isOpen = !isOpen;
        if (consolePanel != null)
        { 
            consolePanel.SetActive(isOpen);
            if (scrollRect == null) scrollRect = outputField.GetComponentInParent<ScrollRect>();
        }

        if (pauseWhileOpen && playerInput)
        {
            if (isOpen)
            {
                playerInput.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
            else
            {
                playerInput.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = previousTimeScale;
            }
        }

        if (isOpen)
        {
            EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            inputField.OnPointerClick(new PointerEventData(EventSystem.current));
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void ExecuteInput(string inputLine)
    {
        AppendOutput($"> {inputLine}");

        // Simple analysis: split by space, the first item is a command
        string[] parts = SplitArguments(inputLine);
        if (parts.Length == 0) return;

        string cmd = parts[0];
        string[] args = new string[parts.Length - 1];
        Array.Copy(parts, 1, args, 0, args.Length);

        if (commands.TryGetValue(cmd, out var action))
        {
            try
            {
                action.Invoke(args);
            }
            catch (Exception ex)
            {
                AppendOutput($"Command Execution Exception: {ex.Message}");
            }
        }
        else
        {
            AppendOutput($"Unknown Command: {cmd}. Use > help to see the available commands.");
        }
    }


    #region Command Registration & Defaults
    private void Register(string name, Action<string[]> callback, string description = "")
    {
        name = name.ToLowerInvariant();
        if (!commands.ContainsKey(name))
        {
            commands.Add(name, callback);
        }
        else
        {
            commands[name] = callback;
        }
    }

    private void RegisterDefaultCommands()
    {
        Register("help", args =>
        {
            AppendOutput("Available Commands:");
            AppendOutput("help - Show Help");
            AppendOutput("clear - Empty the console");
            AppendOutput("god - Toggle invincibility");
            AppendOutput("sethealth <value> - Setting the player's blood level");
            AppendOutput("teleport <x> <y> <z> - Teleport the player to the coordinates");
        });

        Register("clear", args =>
        {
            outputField.text = "";
        });

        Register("god", args =>
        {
            var player = GameObject.FindWithTag("Player");
            if (player == null) { AppendOutput("Not Found Player"); return; }
            var ph = player.GetComponent<PlayerHealth>();
            if (ph == null) { AppendOutput("Player does not implement the PlayerHealth component"); return; }
            ph.isInvincible = !ph.isInvincible;
            AppendOutput($"Invincible has switched to {ph.isInvincible}");
        });

        Register("sethealth", args =>
        {
            if (args.Length < 1) { AppendOutput("Usage: sethealth <value>"); return; }
            if (!float.TryParse(args[0], out float v)) { AppendOutput("Invalid Figure"); return; }
            var player = GameObject.FindWithTag("Player");
            if (player == null) { AppendOutput("Not Found Player"); return; }
            var ph = player.GetComponent<PlayerHealth>();
            if (ph == null) { AppendOutput("Player does not implement the PlayerHealth component"); return; }
            ph.SetHealth(v);
            AppendOutput($"Health has been set to {ph.currentHealth}");
        });

        Register("teleport", args =>
        {
            if (args.Length < 3) { AppendOutput("Uasge: teleport <x> <y> <z>"); return; }
            if (!float.TryParse(args[0], out float x) ||
                !float.TryParse(args[1], out float y) ||
                !float.TryParse(args[2], out float z))
            {
                AppendOutput("Invalid coordinate numbers");
                return;
            }
            var player = GameObject.FindWithTag("Player");
            if (player == null) { AppendOutput("Not Found Player"); return; }
            player.transform.position = new Vector3(x, y, z);
            AppendOutput($"The player has been transported to {x}, {y}, {z}");
        });

    }
    #endregion

    #region Utilities
    public void AppendOutput(string text)
    {
        if (outputField == null) return;
        outputField.text += text + "\n";
        Canvas.ForceUpdateCanvases();
        //scrollRect.verticalNormalizedPosition = 0f;
    }

    private string[] SplitArguments(string input)
    {
        var parts = new List<string>();
        bool inQuotes = false;
        var cur = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }
            if (char.IsWhiteSpace(c) && !inQuotes)
            {
                if (cur.Length > 0)
                {
                    parts.Add(cur.ToString());
                    cur.Length = 0;
                }
            }
            else
            {
                cur.Append(c);
            }
        }
        if (cur.Length > 0) parts.Add(cur.ToString());
        return parts.ToArray();
    }
    #endregion

    #region Public API for adding commands at runtime
    /// <summary>
    /// New commands can be registered externally with this
    /// <example>
    /// <para/>To register a command named "mycommand", and set its callback to call a function:
    /// <para/>>mycommand 3.5
    /// <code>
    /// <para/>void Function1(float num)
    /// <para/>void Function2()
    /// <para/>RegisterCommand("mycommand", args => { 
    ///     if (!float.TryParse(args[0], out float t)) { AppendOutput("Invalid Figure"); return; }
    ///     Function1(t);
    ///     Function2();
    ///     AppendOutput("MyCommand executed");
    /// });
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="name">the command name</param>
    /// <param name="callback">the callback to invoke when the command is executed</param>

    public void RegisterCommand(string name, Action<string[]> callback)
    {
        Register(name, callback);
    }
    #endregion
}
