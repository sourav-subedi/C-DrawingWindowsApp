using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Method command for the MYBooseApp environment.
    /// Extends the base <see cref="Method"/> class without using any static counters.
    /// </summary>
    public class AppMethod : Method
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppMethod"/> class.
        /// All static counter restrictions have been removed.
        /// </summary>
        public AppMethod()
        {
            // No static counter reset required
        }

        /// <summary>
        /// Compiles the method command by calling the base Compile method.
        /// You can add custom initialization here if needed.
        /// </summary>
        public override void Compile()
        {
            base.Compile();
            // Any additional setup for AppMethod can go here
        }

        /// <summary>
        /// Executes the method command by calling the base Execute method.
        /// Custom logic can be added here if needed.
        /// </summary>
        public override void Execute()
        {
            base.Execute();
            // Additional execution behavior can be implemented here
        }
    }
}
