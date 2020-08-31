public class TimerUpdateUIEventArgs
{
    public float CurrentTime { get; }
    public float AllTime { get; }

    public TimerUpdateUIEventArgs(float current, float all)
    {
        CurrentTime = current;
        AllTime = all;
    }
}
