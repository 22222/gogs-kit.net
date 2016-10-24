using GogsKit.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit
{
    /// <summary>
    /// The base interface for a GOGS API result.
    /// </summary>
    public interface IJsonEntity
    {
        /// <summary>
        /// Returns a JSON representation of this result.
        /// </summary>
        /// <returns>the JSON for this result</returns>
        string ToJson();
    }

    /// <summary>
    /// A base class for GOGS API results.
    /// </summary>
    public abstract class JsonEntityBase : IJsonEntity
    {
        /// <summary>
        /// Returns a JSON representation of this result.
        /// </summary>
        /// <returns>the JSON for this result</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Returns a JSON representation of this result (the same as <see cref="ToJson"/>).
        /// </summary>
        /// <returns>the JSON for this result</returns>
        public override string ToString()
        {
            return ToJson();
        }
    }

    /// <summary>
    /// Helper methods for a GOGS API result.
    /// </summary>
    public static class JsonEntity
    {
        /// <summary>
        /// Attempts to parse the specified JSON into an array of result objects.
        /// </summary>
        /// <typeparam name="T">the type of result object</typeparam>
        /// <param name="json">the JSON string to parse</param>
        /// <param name="result">the results or null></param>
        /// <returns>true if the parse was successful</returns>
        public static bool TryParseJsonArray<T>(string json, out IReadOnlyCollection<T> result) where T : class, IJsonEntity
        {
            try
            {
                result = ParseJsonArray<T>(json);
            }
            catch (GogsKitResultParseException)
            {
                result = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Parses the specified JSON into an array of result objects.
        /// </summary>
        /// <typeparam name="T">the type of result object</typeparam>
        /// <param name="json">the JSON string to parse</param>
        /// <returns>the results or null</returns>
        /// <exception cref="GogsKitResultParseException">if the parse fails</exception>
        public static IReadOnlyCollection<T> ParseJsonArray<T>(string json) where T : class, IJsonEntity
        {
            if (json == null)
            {
                throw new GogsKitResultParseException($"{nameof(json)} is null");
            }

            try
            {
                return JsonConvert.DeserializeObject<T[]>(json);
            }
            catch (JsonException ex)
            {
                throw new GogsKitResultParseException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Parses the specified JSON into an array of result objects.
        /// </summary>
        /// <param name="json">the JSON string to parse</param>
        /// <param name="resultType">the type of result object</param>
        /// <returns>the results or null</returns>
        /// <exception cref="GogsKitResultParseException">if the parse fails</exception>
        public static IEnumerable<IJsonEntity> ParseJsonArray(string json, Type resultType)
        {
            if (json == null)
            {
                throw new GogsKitResultParseException($"{nameof(json)} is null");
            }

            var arrayType = resultType.MakeArrayType();
            try
            {
                var enumerable = JsonConvert.DeserializeObject(json, arrayType) as IEnumerable;
                return enumerable?.Cast<IJsonEntity>();
            }
            catch (JsonException ex)
            {
                throw new GogsKitResultParseException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Attempts to parse the specified JSON into a result object.
        /// </summary>
        /// <typeparam name="T">the type of result object</typeparam>
        /// <param name="json">the JSON string to parse</param>
        /// <param name="result">the result or null></param>
        /// <returns>true if the parse was successful</returns>
        public static bool TryParseJson<T>(string json, out T result) where T : class, IJsonEntity
        {
            try
            {
                result = ParseJson<T>(json);
            }
            catch (GogsKitResultParseException)
            {
                result = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Parses the specified JSON into a result object.
        /// </summary>
        /// <typeparam name="T">the type of result object</typeparam>
        /// <param name="json">the JSON string to parse</param>
        /// <returns>the result or null</returns>
        /// <exception cref="GogsKitResultParseException">if the parse fails</exception>
        public static T ParseJson<T>(string json) where T : class, IJsonEntity
        {
            if (json == null)
            {
                throw new GogsKitResultParseException($"{nameof(json)} is null");
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException ex)
            {
                throw new GogsKitResultParseException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Parses the specified JSON into a result object.
        /// </summary>
        /// <param name="json">the JSON string to parse</param>
        /// <param name="resultType">the type of result object</param>
        /// <returns>the result or null</returns>
        /// <exception cref="GogsKitResultParseException">if the parse fails</exception>
        public static IJsonEntity ParseJson(string json, Type resultType)
        {
            if (json == null)
            {
                throw new GogsKitResultParseException($"{nameof(json)} is null");
            }
            try
            {
                return JsonConvert.DeserializeObject(json, resultType) as IJsonEntity;
            }
            catch (JsonException ex)
            {
                throw new GogsKitResultParseException(ex.Message, ex);
            }
        }
    }
}
