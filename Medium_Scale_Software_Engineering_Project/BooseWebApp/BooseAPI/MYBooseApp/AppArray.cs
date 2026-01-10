using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom array command for the BOOSE scripting environment.
    /// Extends the base <see cref="BOOSE.Array"/> class to provide additional 
    /// parameter validation specific to this application.
    /// </summary>
    public class AppArray : BOOSE.Array
    {
        /// <summary>
        /// Checks the parameters provided for the array declaration.
        /// Ensures that the number of parameters is either 3 or 4, otherwise
        /// throws a <see cref="CommandException"/>.
        /// </summary>
        /// <param name="parameterList">The list of parameters to validate.</param>
        /// <exception cref="CommandException">
        /// Thrown when the array declaration syntax is invalid.
        /// </exception>
        public override void CheckParameters(string[] parameterList)
        {
            base.Parameters = base.ParameterList.Trim().Split(' ');
            if (base.Parameters.Length != 3 && base.Parameters.Length != 4)
            {
                throw new CommandException("Invalid array declaration syntax.");
            }
        }
    }
}
