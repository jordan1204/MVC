using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebSuccessWebSite.Models
{
    public class MainMenuModel
    {
        public MainMenuModel()
        {
            SubFunctions = new List<SubFunctionModel>();
        }

        public string FunctionName { get; set; }
        public int ParentId { get; set; }
        public bool IsRoot { get; set; }
        public string Url { get; set; }

        public List<SubFunctionModel> SubFunctions { get; set; }

        public partial class SubFunctionModel
        {
            public string FunctionName { get; set; }
            public int ParentId { get; set; }
            public string Url { get; set; }
        }
    }
}