using TMPro;

public class DialogueHandler : OneInstanceSingleton<DialogueHandler>
{
    private DialoguePayload _dialogue;
    private int _currentDialogueIndex;
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI ContentText;

    public void TurnOn(DialoguePayload dialogue)
    {
        _dialogue = dialogue;
        _currentDialogueIndex = 0;
        ShowDialogue(_currentDialogueIndex);
        gameObject.SetActive(true);
    }

    public void ShowDialogue(int index)
    {
        if (index >= _dialogue.payloads.Count || index < 0)
        {
            TurnOff();
            return;
        }
        else
        {
            if (!string.IsNullOrEmpty(_dialogue.payloads[index].header))
                HeaderText.text = _dialogue.payloads[index].header;
            HeaderText.alignment = _dialogue.payloads[index].headerTextAlignment;
            ContentText.text = _dialogue.payloads[index].content;
        }
    }

    public void NextDialogue()
    {
        ShowDialogue(++_currentDialogueIndex);
    }

    public void TurnOff()
    {
        _currentDialogueIndex = 0;
        gameObject.SetActive(false);
    }
}
