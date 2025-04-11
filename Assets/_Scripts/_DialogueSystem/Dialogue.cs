using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue {
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    
    public Canvas dialogueCanvas;
}