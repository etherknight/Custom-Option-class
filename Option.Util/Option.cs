using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace Options.Util
{
    public struct Option<TObject>
    {
        private readonly TObject value;

        private readonly bool hasValue;

        private Option(TObject value)
        {
            this.value = value;
            this.hasValue = true;
        }

        /// <summary>
        /// Override for assignment operation.
        /// </summary>
        /// <example>
        /// <code>
        /// Option<string> result = "Example";
        /// </code>
        /// </example>
        /// <param name="value"></param>
        public static implicit operator Option<TObject>(TObject value)
        {
            if (value == null)
            {
                return new Option<TObject>();
            }

            return new Option<TObject>(value);
        }

        /// <summary>
        /// Allows chaining of multiple calls together. A failure at 
        /// any point in a chain of Then() calls will drop through to the 
        /// Finally( none ) block.
        /// </summary>
        /// /// <example>
        /// <code>
        /// Option<string> result = "Example";
        /// 
        /// result.Then(str => $"{str} and then")
        ///       .Then(str => $"{str_ and another then")
        ///       .Finally
        ///       (
        ///          some: (finalStr) => Console.WriteLine(finalStr),
        ///          none: () => throw new Exception("Really broken");
        ///       )
        /// </code>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="next"></param>
        /// <returns></returns>
        public Option<TOut> Then<TOut>(Func<TObject, TOut> next)
        {
            if (false == hasValue)
            {
                return new Option<TOut>();
            }

            return next(value);
        }

        /// <summary>
        /// Checks the final result of the option.
        /// </summary>
        /// <example>
        /// <code>
        ///  Option<string> example = "Example";
        ///  example.Finally
        ///  (
        ///    some: (value) => Console.WriteLine(value),
        ///    none: ( )     => Console.WriteLine("Nothing here");
        ///  )
        /// </code>
        /// </example>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="some"></param>
        /// <param name="none"></param>
        public TResult Finally<TResult>(Func<TObject, TResult> some, Func<TResult> none)
        {
            if (hasValue)
            {
                return some(value);
            }

            return none();
        }

        /// <summary>
        /// Checks the final result of the option.
        /// </summary>
        /// <example>
        /// <code>
        ///  Option<string> example = "Example";
        ///  example.Finally
        ///  (
        ///    some: (value) => Console.WriteLine(value),
        ///    none: ( )     => Console.WriteLine("Nothing here");
        ///  )
        /// </code>
        /// </example>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="some"></param>
        /// <param name="none"></param>
        /// <returns></returns>
        public void Finally(Action<TObject> some, Action none)
        {
            if (hasValue)
            {
                some(value);
            }
            else
            {
                none();
            }
        }

        /// <summary>
        /// Safe request to access the value of the Option.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if value is provided.</returns>
        public bool TryGetValue(out TObject value)
        {
            if (hasValue)
            {
                value = this.value;
                return true;
            }

            value = default(TObject);
            return false;
        }

        /// <summary>
        /// Get the value of the Option or throw an exception.
        /// </summary>
        /// <typeparam name="TException">Exception to throw</typeparam>
        /// <returns>Value of the Option</returns>
        public TObject ValueOrThrow<TException>()
            where TException: Exception, new()
        {
            if (false == hasValue)
            {
                throw new TException();
            }

            return value;
        }

        /// <summary>
        /// Get the value of the Option or throw an exception.
        /// </summary>
        /// <param name="message">Optional error message for exception</param>
        /// <exception cref="OptionValueException">Thrown if there is no value.</exception>
        /// <returns>Value of the Option</returns>
        public TObject ValueOrThrow(string message = null)
        {
            if (false == hasValue)
            {
                throw new OptionValueException(message);
            }

            return value;
        }
    }

    public sealed class OptionValueException : Exception
    {
        public OptionValueException()
        {
        }

        public OptionValueException(string message) : base(message)
        {
        }
    }
}