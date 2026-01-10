using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Evaluation command for the MYBooseApp environment.
    /// Extends the base <see cref="Evaluation"/> class to allow expression evaluation.
    /// </summary>
    public class AppEvaluation : Evaluation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppEvaluation"/> class.
        /// </summary>
        public AppEvaluation()
        {
        }

        /// <summary>
        /// Compiles the evaluation expression using the base implementation.
        /// </summary>
        public override void Compile()
        {
            base.Compile();
        }

        /// <summary>
        /// Executes the evaluation expression using the base implementation.
        /// </summary>
        public override void Execute()
        {
            base.Execute();
        }
    }
}
