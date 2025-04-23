using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private DialogueTrigger openTrigger;
    
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _dialogueText;
    private Canvas _dialogueCanvas;
    
    private Queue<string> _sentences;
    
    private Coroutine _typing = null;
    private string _currentSentence = "";

    private void Start()
    {
        _sentences = new Queue<string>();
        openTrigger.TriggerDialogue();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        GameManager.Instance.LockMovement();
        GameManager.Instance.PauseTime();
        _sentences.Clear();
        
        if (dialogue.nameText != null)
        {
            _nameText = dialogue.nameText;
            _nameText.text = dialogue.name;
        }
        _dialogueText = dialogue.dialogueText;
        _dialogueCanvas = dialogue.dialogueCanvas;
        
        _dialogueCanvas.gameObject.SetActive(true);
        
        foreach (string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }

        ShowNextSentence();
    }

    public void ShowNextSentence()
    {
        if (_typing != null)
        {
            StopCoroutine(_typing);
            _typing = null;
            _dialogueText.text = _currentSentence;
            return;
        }
        if (_sentences.Count <= 0)
        {
            EndDialogue();
            return;
        }

        _currentSentence = _sentences.Dequeue();
        
        _dialogueText.text = "";

        _typing = StartCoroutine(PlayText(_currentSentence, _dialogueText));
    }
    
    IEnumerator PlayText(string sentence, TextMeshProUGUI txt)
    {
        foreach (char c in sentence) 
        {
            txt.text += c;
            yield return new WaitForSeconds (0.025f);
        }
        _typing = null;
    }

    private void EndDialogue()
    {
        _dialogueCanvas.gameObject.SetActive(false);
        GameManager.Instance.UnlockMovement();
        GameManager.Instance.ResumeTime();
    }
}