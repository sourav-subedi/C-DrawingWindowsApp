using BOOSE;

public class AppInt : Evaluation, ICommand
{
    public AppInt() { }
    public AppInt(StoredProgram program)
    {
        this.Program = program ?? throw new ArgumentNullException(nameof(program));
    }

    public override int Value
    {
        get => base.value;
        set => base.value = value;
    }

    public override void Compile()
    {
        if (Program == null)
            throw new NullReferenceException("Program reference is null in AppInt.");

        base.Compile();

        if (!Program.VariableExists(VarName))
        {
            Program.AddVariable(this);

            if (!string.IsNullOrEmpty(Expression) && Expression != "0")
                Execute();
            else
                Value = 0;
        }
    }

    public override void Execute()
    {
        if (Program == null)
            throw new NullReferenceException("Program reference is null in AppInt.");

        base.Execute();

        if (!int.TryParse(evaluatedExpression, out int result))
        {
            if (double.TryParse(evaluatedExpression, out _))
                throw new StoredProgramException("Cannot assign double to integer variable.");
            else
                throw new StoredProgramException($"Cannot assign '{evaluatedExpression}' to integer variable.");
        }

        Value = result;

        if (Program.VariableExists(VarName))
            Program.UpdateVariable(VarName, Value);
    }

    public override void CheckParameters(string[] parameterList) { }
}
