namespace BlazorCore.Data.Models;

public class StepperModel
{
    public bool IsCompleted { get; set; }
    public bool CanSkip { get; set; }
    public string StepName { get; }

    public StepperModel(string name, bool canSkip)
    {
        StepName = name;

        CanSkip = canSkip;
    }
}