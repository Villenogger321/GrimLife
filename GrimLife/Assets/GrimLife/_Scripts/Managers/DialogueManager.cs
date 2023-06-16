using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    
    [SerializeField, Range(1, 200)] float textSpeed;

    bool listeningForInput;
    bool skip;

    public static DialogueManager Instance;
    void Awake()
    {
        Instance = this;
    }

    public async void StartDialogue(string[] _dialogue)
    {
        dialogueText.gameObject.SetActive(true);
        listeningForInput = true;

        foreach (string dialogue in _dialogue)
        {
            await WriteText(dialogue);

            while (skip == false)
                await Task.Yield();
            skip = false;
        }

        dialogueText.gameObject.SetActive(false);
        listeningForInput = false;
    }

    async Task WriteText(string _text)
    {
        int characterIndex = 0;
        StringBuilder sb = new StringBuilder();

        while (characterIndex < _text.Length && skip == false)
        {
            sb.Append(_text[characterIndex++]);
            dialogueText.text = sb.ToString();
            await Task.Delay((int)(1000/textSpeed));
        }
        dialogueText.text = _text;
        skip = false;
    }
    void OnSkipText()
    {
        if (listeningForInput)
        {
            skip = true;
        }
    }
}
