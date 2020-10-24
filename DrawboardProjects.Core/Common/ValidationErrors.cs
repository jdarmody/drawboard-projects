using System.Collections.Generic;
using System.Text;

namespace DrawboardProjects.Core.Common
{
    /// <summary>
    /// Use to add simple validation to your Model or View Model classes.
    /// </summary>
    /// <remarks>You can bind to <see cref="IsValid"/> and <see cref="this[string]"/> in your View files.</remarks>
    public class ValidationErrors : BindableBase
    {
        private readonly Dictionary<string, string> validationErrors = new Dictionary<string, string>();

        /// <summary>
        /// Returns True if there are currently no validation errors.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.validationErrors.Count < 1;
            }
        }
        /// <summary>
        /// Use to store and retrieve validation error messages of fields in your class.
        /// </summary>
        /// <param name="fieldName">Name of the field that has a validation error message</param>
        /// <returns>The validation error message for the given <paramref name="fieldName"/>.
        /// Returns <see cref="System.String.Empty"/> if there is no error message.</returns>
        public string this[string fieldName]
        {
            get
            {
                return this.validationErrors.ContainsKey(fieldName) ?
                    this.validationErrors[fieldName] : string.Empty;
            }
            set
            {
                if (this.validationErrors.ContainsKey(fieldName))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        this.validationErrors.Remove(fieldName);
                    }
                    else
                    {
                        this.validationErrors[fieldName] = value;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        this.validationErrors.Add(fieldName, value);
                    }
                }
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Combines all the validation error messages into a multi-line string.
        /// </summary>
        public override string ToString()
        {
            if (validationErrors.Values.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string error in validationErrors.Values)
                {
                    sb.AppendLine(error);
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Use to clear all error messages.
        /// </summary>
        public void Clear()
        {
            var keyList = new string[this.validationErrors.Count];
            this.validationErrors.Keys.CopyTo(keyList, 0);

            foreach (var key in keyList)
            {
                this[key] = string.Empty;
            }
        }
    }
}
