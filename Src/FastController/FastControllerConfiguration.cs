using FastController.Formattor.Input;
using FastController.Formattor.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastController
{
    public class FastControllerConfiguration
    {
        public string[] Methods { get; }
        public string ResponseContextType { get; }
        public List<IInputFormattor> InputFormattors { get; }
        public List<IOutPutFormattor> OutPutFormattors { get; }

        public static FastControllerConfiguration Default { get; set; } = new FastControllerConfiguration("application/json",
            new List<IInputFormattor> {
                new ParamInputFormattor(),
                new JsonInputFormattor()
            },
            new List<IOutPutFormattor>
            {
                new JsonOutputFormattor()
            },
            "Post", "Get");

        public FastControllerConfiguration(string responseContextType, List<IInputFormattor> inputFormattors, List<IOutPutFormattor> outPutFormattors)
        {
            ResponseContextType = responseContextType;
            InputFormattors = inputFormattors;
            OutPutFormattors = outPutFormattors;
        }

        public FastControllerConfiguration(string responseContextType, List<IInputFormattor> inputFormattors, List<IOutPutFormattor> outPutFormattors, params string[] methods)
        {
            Methods = methods;
            ResponseContextType = responseContextType;
            InputFormattors = inputFormattors;
            OutPutFormattors = outPutFormattors;
        }
    }
}
