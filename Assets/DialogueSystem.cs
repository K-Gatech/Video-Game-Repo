using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text NameUI;
    public TMP_Text DialogueUI;

    public GameObject NextUI;

    bool isRunning;
    bool canNext;
    bool dialogueSequenceOngoing;
    
    string objective;
    
    Text objectiveUI;

    CanvasGroup cg;

    class Dialogue
    {
        public string dialogue;
        public string name;
        public float time;

        public Dialogue(string dialogue, string name, float time)
        {
            this.dialogue = dialogue;
            this.name = name;
            this.time = time;
        }
    }

    private Queue<Dialogue> qDialogue;

    // Start is called before the first frame update
    void Start()
    {
        qDialogue = new Queue<Dialogue>();
        isRunning = false;
        canNext = false;
        cg = GetComponent<CanvasGroup>();
        
        objectiveUI = GameObject.Find("Objective Text").GetComponent<Text>();

        cg.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning && canNext)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                NextDialogue();
            }
        }
    }

    public DialogueSystem AddDialog(string name, string dialogue, float time)
    {
        if (isRunning)
            return this;

        qDialogue.Enqueue(new Dialogue(dialogue, name, time));
        return this;
    }
    
    public DialogueSystem AddObjective(string objective) 
    {
        this.objective = objective;
        
        return this;
    }

    public void Show()
    {
        if (isRunning)
            return;

        isRunning = true;
        cg.alpha = 1;
        objectiveUI.text = objective;
        StartCoroutine(show());
    }

    IEnumerator show()
    {
        HideNext();

        Dialogue d = qDialogue.Dequeue();
        NameUI.text = d.name;
        DialogueUI.text = d.dialogue;

        yield return new WaitForSeconds(d.time);

        ShowNext();
    }

    public void HideNext()
    {
        canNext = false;
        NextUI.SetActive(false);
    }

    void ShowNext()
    {
        canNext = true;
        NextUI.SetActive(true);
    }

    void NextDialogue()
    {
        if (qDialogue.Count == 0)
        {
            isRunning = false;
            cg.alpha = 0;
        } else
        {
            StartCoroutine(show());
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsSequenceDone()
    {
        return (qDialogue.Count == 0);
    }

}
