using System;
namespace NumAmountToStringConverter
{
    public class NumAmountToStringException : Exception
    {
        public NumAmountToStringException()
            : base()
        { }
        public NumAmountToStringException(string message)
            : base(message)
        { }
    }
}