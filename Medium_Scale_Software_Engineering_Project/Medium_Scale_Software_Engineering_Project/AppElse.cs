using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Else command for the MYBooseApp environment.
    /// Extends the base <see cref="Else"/> class without using a static counter.
    /// </summary>
    public class AppElse : Else
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppElse"/> class.
        /// Each instance is independent and does not rely on any static counters.
        /// </summary>
        public AppElse()
        {
            // No static counter reset needed
        }

        /// <summary>
        /// Executes the Else command.
        /// Relies on base class functionality, no static counters involved.
        /// </summary>
        public override void Execute()
        {
            base.Execute();
        }
    }
}
