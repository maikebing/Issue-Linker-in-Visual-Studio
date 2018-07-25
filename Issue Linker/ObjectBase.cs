using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue_Linker
{
    abstract class ObjectBase
    {
        private string type = string.Empty;
        private List<string> labels = new List<string>();
        private string status = string.Empty;
        private string resolution = string.Empty;
        /// <summary>
        /// Empty constructor
        /// </summary>
        public ObjectBase()
        {
            //empty constructor
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type">The type of the object (issue,pull,bug, etc)</param>
        /// <param name="labels">List of labels</param>
        /// <param name="status">Open/Close</param>
        public ObjectBase(string type, List<string> labels, string status, string resolution)
        {
            Type = type;
            Labels = labels;
            Status = status;
            Resolution = resolution;
        }


        #region Proprieties
        public string Type { get => type; set => type = value; }
        public List<string> Labels { get => labels; set => labels = value; }
        public string Status { get => status; set => status = value; }
        public string Resolution { get => resolution; set => resolution = value; }
        #endregion

        #region Abstract Methods
        abstract public void CreateVisuals();

        #endregion

    }
}
