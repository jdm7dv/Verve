using StringProcessor;

namespace GuiApp
{
    class UppercaseString : IStringProcessor
    {
        #region IStringProcessor Members

        public string Process(string input)
        {
            return input.ToUpper();
        }

        #endregion

        public override string ToString()
        {
            return "Uppercase the String";
        }
    }
}
