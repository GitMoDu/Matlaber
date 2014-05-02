using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matlaber
{
    public class class_types
    {

        public struct SimpleResult
        {
            public bool success;
            public string message;
        }

        public struct CodeResult
        {
            public bool success;
            public string code;
            public string comment;
        }

        public struct m_variable
        {
            public string name;
            public string type;
            public string value;
            public string IO_type;
            public string comment;
            public bool retain;
        }

        public struct m_CodeLine
        {
            public string code;
            public string comment;
        }


        public struct m_file_data
        {
            public class_types.m_CodeLine title;
            public LinkedList<string> var_block_comments;

            public LinkedList<class_types.m_variable> variables;
            public LinkedList<class_types.m_CodeLine> codelines;
        }

        public struct Instruction_and_Argument
        {
            public string instruction;
            public string argument;
        }

        public struct read_function_output
        {
            public int lines_forward;
            public bool success;
            public string message;
            public m_file_data m_file;

        }


    }
}
