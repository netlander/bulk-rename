using UnityEngine;
using UnityEditor;
using System.Linq;

public class BulkRename : ScriptableWizard
{

    public string NewName = "renamed-object";
    public string Delimiter = "-";
    public int StartNumber = 0;
    public int Increment = 1;
    public int NumberOfDigits = 2;

    [MenuItem("Custom/Bulk Rename...")]
    static void InstantiateWizard()
    {
        ScriptableWizard.DisplayWizard("Bulk Rename", typeof(BulkRename), "Rename");
    }

    void OnEnable()
    {
        UpdateWizardHelp();
    }

    void OnSelectionChange()
    {
        UpdateWizardHelp();
    }

    void OnWizardCreate()
    {
        if (Selection.objects.Length == 0)
        {
            return;
        }

        int PostFix = StartNumber;
        string PostFixString = "";

        Object[] theArray = new Object[getLargestIndex(Selection.objects) + 1];

        foreach (Object O in Selection.objects)
        {
            theArray[((GameObject) O).transform.GetSiblingIndex()] = O;
        }

        theArray = theArray.Where(c => c != null).ToArray();

        foreach (Object O in theArray)
        {
            PostFixString = PostFix.ToString();

            if (PostFix.ToString().Length < NumberOfDigits)
            {
                int discrepency = NumberOfDigits - PostFix.ToString().Length;

                for (int i = 0; i < discrepency; i++)
                {
                    PostFixString = "0" + PostFixString;
                }
            }

            if (Delimiter != "")
            {
                O.name = NewName + Delimiter + PostFixString;
            }
            else
            {
                O.name = NewName + PostFixString;
            }

            PostFix += Increment;
        }
    }

    private int getLargestIndex(Object[] selection)
    {
        int theIndex = 0;

        foreach (Object O in selection)
        {
            if (theIndex < ((GameObject) O).transform.GetSiblingIndex())
            {
                theIndex = ((GameObject) O).transform.GetSiblingIndex();
            }
        }
        return theIndex;
    }

    void UpdateWizardHelp()
    {
        if (Selection.objects != null)
        {
            helpString = "Number of Currently selected objects: " + Selection.objects.Length;
        }
    }

}