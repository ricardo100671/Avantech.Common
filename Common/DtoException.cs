using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Avantech.Common
{
    [Serializable]
    public class DtoException<TDto> : Exception
        where TDto : class
    {
        public DtoException()
        {
            Data = new List<KeyValuePair<string, Exception>>();
        }

        public List<KeyValuePair<string, Exception>> Data { get; set; }

        /// <summary>
        /// Adds an exception for the specified Dto member.
        /// </summary>
        /// <param name="memberAccesor">The Dto member accesor.</param>
        /// <param name="exception">The exception to be added.</param>
        public void Add(Expression<Func<TDto, object>> memberAccesor, Exception exception)
        {
            Data.Add(new KeyValuePair<string, Exception>(
                memberAccesor.GetPropertyInfo().Name,
                exception
            ));
        }

        /// <summary>
        /// Determines whether there are any exception for the specified Dto member.
        /// </summary>
        /// <param name="memberAccesor">The Dto member accesor.</param>
        /// <returns>
        ///   <c>true</c> if one or more exceptions are found for the specified Dto member; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsExceptionsFor(Expression<Func<TDto, object>> memberAccesor)
        {
            return Data.Any(i => i.Key == memberAccesor.GetPropertyInfo().Name);
        }

        /// <summary>
        /// Determines whether there are exceptions of type "TException" for the specified Dto member.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to be searched.</typeparam>
        /// <param name="memberAccesor">The Dto member accesor.</param>
        /// <returns>
        ///   <c>true</c> if one or more exceptions of type "TException" are found for the specified Dto member; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsExceptionsFor<TException>(Expression<Func<TDto, object>> memberAccesor)
            where TException : Exception
        {
            return Data.Any(i =>
                i.Key == memberAccesor.GetPropertyInfo().Name
                && i.Value is TException
            );
        }

        /// <summary>
        /// Removes all exceptions of type "TException".
        /// </summary>
        /// <typeparam name="TException">The type of the exception to be removed.</typeparam>
        public void Remove<TException>()
            where TException : Exception
        {
            Data
                .Where(i => i.Value is TException
                )
                .ToList()
                .ForEach(i =>
                    Data.Remove(i)
                );
        }

        /// <summary>
        /// Removes all exceptions for the specified Dto memeber.
        /// </summary>
        /// <param name="memberAccesor">The Dto member accesor.</param>
        public void RemoveFor(Expression<Func<TDto, object>> memberAccesor)
        {
            Data
                .Where(i => i.Key == memberAccesor.GetPropertyInfo().Name)
                .ToList()
                .ForEach(i =>
                    Data.Remove(i)
                );
        }

        /// <summary>
        /// Removes all exceptions, of type "TException", for the specified Dto memeber.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to be removed.</typeparam>
        /// <param name="memberAccesor">The Dto member accesor.</param>
        public void RemoveFor<TException>(Expression<Func<TDto, object>> memberAccesor)
            where TException : Exception
        {
            Data
                .Where(i =>
                    i.Key == memberAccesor.GetPropertyInfo().Name
                    && i.Value is TException
                )
                .ToList()
                .ForEach(i =>
                    Data.Remove(i)
                );
        }

        /// <summary>
        /// Gets an enumeration <see cref="IEnumerable{Exception}"/> of all exceptions.
        /// </summary>
        /// <value>
        /// The exceptions.
        /// </value>
        public List<Exception> Exceptions
        {
            get
            {
                return Data
                    .Select(i =>
                        i.Value
                    )
                    .ToList();
            }
        }

        public List<TException> ExceptionsFor<TException>(Expression<Func<TDto, object>> memberAccesor)
            where TException : Exception
        {
            return Data
              .Where(i =>
                  i.Key == memberAccesor.GetPropertyInfo().Name
                  && i.Value is TException
              )
              .Select(i => i.Value)
              .Cast<TException>()
              .ToList();
        }


        /// <summary>
        /// Gets an enumeration <see cref="IEnumerable{Exception}"/> of all exceptions for the specified Dto member.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{Exception}"/>.
        /// </value>
        /// <param name="memberAccesor">The member accesor.</param>
        /// <returns></returns>
        public IEnumerable<Exception> this[Expression<Func<TDto, object>> memberAccesor]
        {
            get
            {
                return Data
                    .Where(i =>
                        i.Key == memberAccesor.GetPropertyInfo().Name
                    )
                    .Select(i => i.Value)
                    .ToList();
            }
        }

        /// <summary>
        /// Removes all exceptions.
        /// </summary>
        public void Clear()
        {
            Data.Clear();
        }

        /// <summary>
        /// Gets the total number of exceptions.
        /// </summary>
        /// <value>
        /// The count of exceptions.
        /// </value>
        public int Count
        {
            get { return Data.Count; }
        }

        /// <summary>
        /// Gets the total number of execptions for the specified Dto Member.
        /// </summary>
        /// <param name="memberAccesor">The Dto member accesor.</param>
        /// <returns></returns>
        public int CountFor(Expression<Func<TDto, object>> memberAccesor)
        {
            return Data.Count(i =>
                i.Key == memberAccesor.GetPropertyInfo().Name
            );
        }

        /// <summary>
        /// Gets all the member names for which there are exception.
        /// </summary>
        /// <value>
        /// The Dto member names.
        /// </value>
        public IEnumerable<string> MemberNames
        {
            get
            {
                return
                    Data.Select(i => i.Key)
                    .Distinct()
                    .ToList();
            }
        }

        /// <summary>
        /// An aggregated message of all exception message.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                var message = new List<string>();

                Data.ForEach(kvp =>
                    message.Add(kvp.Key + ":" + kvp.Value.Message)
                );

                return message.JoinWith(" ");
            }
        }
    }
}
