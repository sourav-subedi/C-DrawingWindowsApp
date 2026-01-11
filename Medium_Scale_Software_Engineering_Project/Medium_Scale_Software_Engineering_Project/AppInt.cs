using BOOSE;

/// <summary>
/// Represents an integer variable in BOOSE with support for declaration and assignment.
/// Supports "int x", "int x = 50", and "x = expression" forms.
/// </summary>
public class AppInt : Evaluation, ICommand
{
    /// <summary>
    /// Stores the integer value of this variable.
    /// </summary>
    private int intValue;

    /// <summary>
    /// Default constructor for use by <see cref="AppCommandFactory"/>.
    /// </summary>
    public AppInt() { }

    /// <summary>
    /// Gets or sets the integer value stored in this command.
    /// </summary>
    public override int Value
    {
        get => intValue;
        set => intValue = value;
    }

    /// <summary>
    /// Initializes the variable by parsing the name and optional assignment expression.
    /// </summary>
    /// <param name="Program">The current <see cref="StoredProgram"/> instance.</param>
    /// <param name="Params">The declaration string, e.g., "x = 50" or "x".</param>
    public new void Set(StoredProgram Program, string Params)
    {
        base.Set(Program, Params);
        string trimmed = Params.Trim();

        if (trimmed.Contains("="))
        {
            int eq = trimmed.IndexOf('=');
            VarName = trimmed.Substring(0, eq).Trim();
            Expression = trimmed.Substring(eq + 1).Trim();
        }
        else
        {
            VarName = trimmed;
            Expression = "0"; // Default initialization if no value provided
        }
    }

    /// <summary>
    /// Compilation phase: Registers the variable in the program and evaluates the initial expression.
    /// </summary>
    /// <exception cref="BOOSEException">Thrown if variable or program state is invalid.</exception>
    /// <exception cref="CommandException">Thrown if the initial expression cannot be evaluated as an integer.</exception>
    public override void Compile()
    {
        if (Program == null || string.IsNullOrEmpty(VarName))
            throw new BOOSEException("Invalid AppInt state");

        if (!Program.VariableExists(VarName))
        {
            Program.AddVariable(this);

            if (!string.IsNullOrEmpty(Expression) && Expression != "0")
            {
                try
                {
                    base.Execute(); // Evaluate the expression
                    string result = evaluatedExpression;

                    if (int.TryParse(result, out intValue))
                    {
                        Program.UpdateVariable(VarName, intValue);
                    }
                    else
                    {
                        throw new CommandException($"Cannot convert '{result}' to int in declaration");
                    }
                }
                catch (StoredProgramException ex)
                {
                    throw new CommandException(
                        $"Invalid expression in int declaration '{Expression}': {ex.Message}"
                    );
                }
            }
            else
            {
                intValue = 0;
            }
        }
    }

    /// <summary>
    /// Executes the assignment of a value to the variable at runtime.
    /// Evaluates the expression and updates the program variable.
    /// </summary>
    /// <exception cref="CommandException">Thrown if expression cannot be evaluated as an integer.</exception>
    public override void Execute()
    {
        if (string.IsNullOrEmpty(Expression) || Expression == "0")
            return;

        try
        {
            base.Execute(); // Evaluate expression
            string result = evaluatedExpression;

            if (!int.TryParse(result, out intValue))
                throw new CommandException($"Cannot convert '{result}' to int");

            if (Program.VariableExists(VarName))
                Program.UpdateVariable(VarName, intValue);
        }
        catch (StoredProgramException ex)
        {
            throw new CommandException(
                $"Invalid expression in assignment to '{VarName}': {ex.Message}"
            );
        }
    }

    /// <summary>
    /// No parameter validation is required for this command.
    /// </summary>
    /// <param name="parameterList">Array of parameters (not used).</param>
    public override void CheckParameters(string[] parameterList) { }
}
