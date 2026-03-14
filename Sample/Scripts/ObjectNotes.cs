using UnityEngine;
using Sirenix.OdinInspector;

[HideMonoScript]
public class ObjectNotes : MonoBehaviour
{
    [TitleGroup("Object Notepad", "Note", Alignment = TitleAlignments.Centered)]
    [HideLabel]
    [MultiLineProperty(10)]
    [GUIColor(1f, 1f, 0.8f)]
    public string Note = "type here...";

 }