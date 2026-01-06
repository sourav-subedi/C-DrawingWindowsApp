using BOOSE;

namespace MYBooseApp
{
    public class AppArray : BOOSE.Array
    {
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