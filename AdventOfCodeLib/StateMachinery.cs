namespace AdventOfCodeLib;

public record Transition(string From, char On, string To, Action<int, Transition>? OnEnter = null);

public record StateMachinery(
    List<string> States,
    List<char> PossibleInputs,
    List<Transition> Transitions,
    string StartState,
    List<string> EndStates)
{
    public bool Run(string input)
    {
        return Accepts(0, input, StartState);
    }

    //??????##....###


    // (ok, .) => (ok)
    // (ok, #) => (broken)
    // (broken, #) => (broken)
    // (broken, .) => (ok)
    // (ok, ?) => (ok, broken)
    // (broken, ?) => (ok, broken)
    private bool Accepts(int index, string currentInput, string currentState)
    {
        if (currentInput.Length > 0)
        {
            var transitions = GetTransitions(currentInput[0], currentState);
            foreach (var t in transitions)
            {
                t.OnEnter?.Invoke(index, t);
                if (Accepts(index + 1, currentInput.Substring(1), t.To))
                {
                    return true;
                }
            }

            return false;
        }

        if (EndStates.Contains(currentState))
        {
            return true;
        }

        return false;
    }

    private IList<Transition> GetTransitions(char currentInput, string currentState)
    {
        return Transitions
            .Where(t => t.From == currentState
                        && t.On == currentInput
            ).ToList();
    }
}