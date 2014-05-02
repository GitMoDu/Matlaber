using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography;
using System.Security;



namespace Matlaber
{
    public partial class Form1 : Form
    {
        


        static string DefaultOutputFolder="\\Converted";
        public bool HasSelectedOutput = false;
        public bool GOTO_flag = false;
        public bool COUNTER_UP_flag = false;
        public bool COUNTER_DOWN_flag = false;
        public bool TIMER_ON_flag = false;
        public Form1()
        {
            InitializeComponent();
            textBoxInput.Text = Directory.GetCurrentDirectory();
            textBoxOutput.Text = textBoxInput.Text + DefaultOutputFolder;
            LoadImages();
            ValidateAssemblies();//TODO:revise for more files
            FillCheckBox();
            button1.Enabled = false;
        }

        private void UpdateInputDirectory(string InputDirectory)
        {
            textBoxInput.Text = InputDirectory;
        }

        private void UpdateOutputDirectory(string OutputDirectory)
        {
            if (OutputDirectory.Length > 0)
            {
                //root clause: avoids something like c:\\DefaultOutputFolder
                string outputfolder;
                //char slash='\';
                if (textBoxInput.Text.Substring(textBoxInput.Text.Length - 1, 1) == '\\'.ToString())
                    outputfolder = DefaultOutputFolder.Substring(1, DefaultOutputFolder.Length - 1);
                else
                    outputfolder = DefaultOutputFolder;

                if (!HasSelectedOutput)
                {
                    if (OutputDirectory.Length == 0)
                        textBoxOutput.Text = textBoxInput.Text + outputfolder;
                    else
                        textBoxOutput.Text = OutputDirectory;
                }
                else
                {
                    if (OutputDirectory.Length != 0)
                        textBoxOutput.Text = OutputDirectory;
                }
            }
                
        }

        private int FillCheckBox()
        {
            string usable_directory = textBoxInput.Text;
            if (Directory.Exists(usable_directory))
            {
                while (true)
                {
                    if (ReverseString(usable_directory).Substring(0, 1) != "\\")
                        break;
                    else
                        usable_directory = usable_directory.Substring(0, usable_directory.LastIndexOf("\\"));
                }

                //string[] filePaths = Directory.GetFiles(usable_directory, "*.il");
                string[] filePaths = Directory.GetFiles(usable_directory);

                string file;

                FileListCheckedBox.Items.Clear();
                if (filePaths.Count() > 0)
                {
                    for (int i = 0; i < filePaths.Count(); i++)
                    {
                        file = filePaths.ElementAt(i).Substring(usable_directory.Count() + 1, filePaths.ElementAt(i).Length - usable_directory.Count() - 1);
                        FileListCheckedBox.Items.Add(file);
                    }
                    return filePaths.Count();
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            class_types.SimpleResult result;
            string result_string, path,outpath;
            int ConvertedCount=0;
            float progress = 0;

            if (FileListCheckedBox.CheckedItems.Count > 0)
            {
                //ListResultBox.Items.Clear();
                for (int i = 0; i <FileListCheckedBox.CheckedItems.Count; i++)
                {
                    progress = ((float)(i+1) / (float)(FileListCheckedBox.CheckedItems.Count)) * (float)100;
                    progressBar1.Value = (int)progress;

                    path = textBoxInput.Text+"\\"+FileListCheckedBox.CheckedItems[i].ToString();
                    outpath = textBoxOutput.Text + "\\";//filename is title of program

                    result = ConvertIEC(path, outpath);//convert all files to memory

                    if (result.success)
                    {
                        result_string = " was successfully converted. " + result.message;
                        ConvertedCount++;
                    }
                    else
                        result_string = " was not converted. " + result.message;
                   
                    ListResultBox.Items.Add(FileListCheckedBox.CheckedItems[i].ToString() + result_string);
                    

                }
                //mensagens de resultado
                if (ConvertedCount == 0)
                    MessageBox.Show("Errors occurred while attempting to convert. No files were converted. Check the log.", "No files have been converted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (FileListCheckedBox.CheckedItems.Count > ConvertedCount)
                    {
                        MessageBox.Show("Errors occurred while trying to convert some files. Check the log.", "Some files were not converted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (FileListCheckedBox.CheckedItems.Count == ConvertedCount)
                            MessageBox.Show("All files were successfully converted.", "All files were converted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Errors occurred while trying to convert. No files were converted. Check the log.", "No files have been converted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
                MessageBox.Show("No files selected.", "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
            
        }

        private void ButtonInputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();  // show the dialog.
            if (result == DialogResult.OK) // test result.
            {
                try
                {
                    UpdateInputDirectory(folderBrowserDialog1.SelectedPath);
                    UpdateOutputDirectory("");
                    FillCheckBox();
                }
                catch (IOException)
                {
                    MessageBox.Show("Invalid folder: " + folderBrowserDialog1.SelectedPath, "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (FileListCheckedBox.CheckedItems.Count > 0)
                button1.Enabled = true;
            else
                button1.Enabled = false;

        }

        private void ButtonOutputFolder_Click(object sender, EventArgs e)
        {
            HasSelectedOutput = true;

            DialogResult result = folderBrowserDialog1.ShowDialog();  // show the dialog.
            if (result == DialogResult.OK) // test result.
            {
                try
                {
                    UpdateOutputDirectory(folderBrowserDialog1.SelectedPath);
                }
                catch (IOException)
                {
                    MessageBox.Show("Invalid folder: \n" + folderBrowserDialog1.SelectedPath, "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            //TODO: Erase last "\" if found

            if (textBoxInput.Text.Length > 0)
            {
                UpdateOutputDirectory(textBoxInput.Text + DefaultOutputFolder);
                FillCheckBox();
                if (FileListCheckedBox.CheckedItems.Count > 0)
                    button1.Enabled = true;
                else
                    button1.Enabled = false;
            }
            
        }

        string PathToDirectory(string path)
        {
            string reverse=ReverseString(path);
                int lastslash= reverse.IndexOf("\\");
                return path.Substring(0,path.Length-lastslash);
                    
        }

        static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        bool IsFileEmpty(string path)
        {
            if (!File.Exists(path))
                return true;

            StreamReader Reader = File.OpenText(path);
            while (Reader.ReadLine() != null)
            {
                Reader.Close();
                return false;
            }
            Reader.Close();
            return true;

        }

        int NumberOfLines(string path)
        {
            if (File.Exists(path))
            {
                string line;
                int LineCount = 0;
                StreamReader Reader = File.OpenText(path);
                StreamReader file = null;
                try
                {
                    file = new StreamReader(path);
                    while ((line = file.ReadLine()) != null)
                    {
                        LineCount++;
                    }
                    file.Close();
                    
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
                return LineCount;
            } 
            else
                return 0;


        }

        LinkedList<string> GetFileFromAsssembly(string file, string category)
        {
            string line;
            LinkedList<string> filecode = new LinkedList<string>();

            Assembly myAssembly;
            StreamReader filestream;
            

            try
            {
                myAssembly = Assembly.GetExecutingAssembly();
                filestream = new StreamReader(myAssembly.GetManifestResourceStream("Matlaber." + category + "." + file));
                while ((line = filestream.ReadLine()) != null)
                {
                    filecode.AddLast(line);
                }
                filestream.Close();
                return filecode;

            }
            catch
            {
                MessageBox.Show("Program corrupted, exiting.", "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
                return null;
            }



        }

        LinkedList<string> ReadFromText(string path)
        {
            string line;
            LinkedList<string> InCode = new LinkedList<string>();

            if (File.Exists(path))
            {
                    StreamReader file = null;
                    try
                    {

                        file = new StreamReader(path);
                        while ((line = file.ReadLine()) != null)
                        {
                            InCode.AddLast(ReplaceBadChars(line.ToUpperInvariant()));
                        }
                        file.Close();
                        return InCode;
                    }
                    catch
                    {
                        return null;
                    }

            }
            else
                return null;
        }

        class_types.CodeResult RemoveComment(string line)//remove comment function
        {
            class_types.CodeResult result=new class_types.CodeResult();
            result.code = line;
            result.comment = "";

            //comments "(* comment *)" exception
            if (!(line == ""))
            {
                if (line.Contains("(*"))
                {
                    if (line.Substring(2).Contains("*)"))
                    {
                        //--------if there any comments, it will write them to the output and remove them from the reading line-------
                        if (line.Substring(line.IndexOf("(*", 0), line.Length - line.IndexOf("(*", 0)).Contains("*)"))
                        {
                            result.comment = line.Substring(line.IndexOf("(*", 0) + 2, line.IndexOf("*)", 0) - line.IndexOf("(*", 0)-2);
                            //result.comment = line.Substring(line.IndexOf("(*", 0) + 2, line.Length - line.IndexOf("(*", 0) - 4);
                            result.code = line.Remove(line.IndexOf("(*", 0), line.Length - line.IndexOf("(*", 0));

                            //clean last spaces
                            result.code = RemoveEndSpaces(result.code);

                            result.success = true;
                        }
                    }
                    else
                    {
                        result.code = RemoveEndSpaces(line.Remove(line.IndexOf("(*", 0), line.Length - line.IndexOf("(*", 0)));
                        result.success = false;
                    }
                }
                else
                {
                    result.success = true;
                }
            }
            else
            {
                result.success = false;
            }

            return result;

        }

        string RemoveSpaces(string in_string)
        {
            int index = 0;
            if (in_string.Length < 1)
                return "";

            while (index < in_string.Length)
            {
                if (in_string.Substring(index, 1) == " ")
                {
                    string A, B;
                    A = in_string.Substring(0, index);
                    B = in_string.Substring(index + 1, in_string.Length - index - 1);
                    in_string = A + B;

                }//remove space
                else
                    index++;
            }
            return in_string;
        }

        string ReplaceBadChars(string in_string)
        {
            StringBuilder no_bad_chars = new StringBuilder(in_string);

            char[] work_string = in_string.ToCharArray();

            for (int i = 0; i < in_string.Length; i++)
            {
                if (work_string[i] == 65533 || work_string[i] == 9)
                    no_bad_chars[i] = ' ';
            }

            string result = no_bad_chars.ToString();

            return result;
        }

        class_types.Instruction_and_Argument SeparateInstFromArg(string line_full)
        {

            //TODO precheck for needed spaces

            int wrong_spaces = 0;
            int index = 0;
            class_types.Instruction_and_Argument result;
            result.argument = "";
            result.instruction = "";
            string line;
            if (line_full.Length < 1)
                return result;

            line = ReplaceBadChars(line_full);

            while (line.Substring(wrong_spaces, 1) == " " && wrong_spaces < line.Length - 1)//clean up wrong spaces
                wrong_spaces++;
            line = line.Substring(wrong_spaces, line.Length - wrong_spaces);
            index = line.IndexOf(" ");
            if (index > 0)
            {
                result.instruction = line.Substring(0, index);
                line = line.Substring(index, line.Length - index);
                wrong_spaces = 0;
                while (line.Substring(wrong_spaces, 1) == " " && wrong_spaces < line.Length - 1)
                    wrong_spaces++;
                line = line.Substring(wrong_spaces, line.Length - wrong_spaces);
                index = line.IndexOf(" ");

                result.argument = line;
            }
            else
            {
                result.instruction = line;
                result.argument = "";
            }


            return result;
        }

        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        string MD5SUM(byte[] FileOrText) //Output: String<-> Input: Byte[] //
        {
            return BitConverter.ToString(new
                MD5CryptoServiceProvider().ComputeHash(FileOrText)).Replace("-", "").ToLower();
        }

        public void ValidateAssemblies()
        {
            byte[] bytearray;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Matlaber.GOTO.goto.m");
            bytearray = ReadFully(myStream);
            myStream.Close();
            string md5_goto = MD5SUM(bytearray);
            myStream = myAssembly.GetManifestResourceStream("Matlaber.POU.COUNTER_UP.m");
            bytearray = ReadFully(myStream);
            myStream.Close();
            string md5_CTU = MD5SUM(bytearray);
            myStream = myAssembly.GetManifestResourceStream("Matlaber.POU.COUNTER_DOWN.m");
            bytearray = ReadFully(myStream);
            myStream.Close();
            string md5_CTD = MD5SUM(bytearray);
            myStream = myAssembly.GetManifestResourceStream("Matlaber.POU.TIMER_ON.m");
            bytearray = ReadFully(myStream);
            myStream.Close();
            string md5_TON = MD5SUM(bytearray);

            myStream = myAssembly.GetManifestResourceStream("Matlaber.Resources.logounl.png");
            bytearray = ReadFully(myStream);
            myStream.Close();
            string md5_unl = MD5SUM(bytearray);
            myStream = myAssembly.GetManifestResourceStream("Matlaber.Resources.Matlaber.ico");
            bytearray = ReadFully(myStream);
            myStream.Close();
            string md5_ico = MD5SUM(bytearray);
            myStream = myAssembly.GetManifestResourceStream("Matlaber.Resources.logofct.png");
            bytearray = ReadFully(myStream);
            myStream.Close();
            string md5_fct = MD5SUM(bytearray);

            if (md5_TON != "76f2f5e1a38d1a83da35611129ef3590" || md5_CTD != "aedd8e7620facb15dbcdf8b5d0013eea" || md5_CTU != "ed99af5fa280dd6815f9f6b5dc8abf98" || md5_unl != "15e6fb149464c324946e39a35ef1e37b" || md5_fct != "c545c0f7add43132c6815c43f0e12d11" || md5_ico != "7ccb4719239149e6f0d97a371ce82917" || md5_goto != "df896bf0d55e9fbec3f57f843e97e976")
            {
                MessageBox.Show("Program corrupted, exiting.", "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        public void LoadImages()
        {
            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Matlaber.Resources.logofct.png");
            Bitmap logofct = new Bitmap(myStream);
            myStream.Close();
            myStream = myAssembly.GetManifestResourceStream("Matlaber.Resources.logounl.png");
            Bitmap logounl = new Bitmap(myStream);
            myStream.Close();

            myStream = myAssembly.GetManifestResourceStream("Matlaber.Resources.Matlaber.ico");

            myStream.Close();

            pictureBox2.Image = logofct;
            pictureBox1.Image = logounl;


        }

        private void FileListCheckedBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FileListCheckedBox.CheckedItems.Count > 0)
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBoxOutput_TextChanged(object sender, EventArgs e)
        {

        }

        string RemoveEndSpaces(string input)
        {
            if (input.Length < 1)
                return input;
            string output = ReverseString(input);

            while (true)
            {
                if (output[0] == ' ')
                {
                    output = output.Substring(1);
                }
                else
                    break;
            }
            output = ReverseString(output);
            return output;
        }

        string ReplaceIllegalChars(string in_string)
        {
            StringBuilder no_line_illegals = new StringBuilder(RemoveEndSpaces(in_string));
            LinkedList<char> work_string = new LinkedList<char>();


            for (int i = 0; i < no_line_illegals.Length; i++)
            {

                if (!(work_string.Count == 0 && (no_line_illegals[i] == ' ') || no_line_illegals[i] == '%'))
                {
                    if (no_line_illegals[i] == '%' || no_line_illegals[i] == '.' || no_line_illegals[i] == ',' || no_line_illegals[i] == ' ') //illegal (not accepted by matlab) chars in names
                        work_string.AddLast('_');
                    else
                    {
                        if (no_line_illegals[i] == ';')
                            break;
                        else
                        {
                            work_string.AddLast(no_line_illegals[i]);
                        }
                    }
                }

            }

            Array result_array = work_string.ToArray();

            string result = "";
            for (int i = 0; i < work_string.Count; i++)
            {
                result += work_string.ElementAt(i);
            }


            return result;
        }

        public LinkedList<class_types.m_variable> SortVariables(class_types.m_file_data m_file)
        {
            //-----variables need sorting, so they are declared and aligned in Matlab---------

            LinkedList<class_types.m_variable> result = new LinkedList<class_types.m_variable>();

            int length = m_file.variables.Count();

            class_types.m_variable[] variables_to_sort = new class_types.m_variable[length];

            variables_to_sort = m_file.variables.ToArray();


            while (true)
            {
                int wrong = 0;
                for (int i = 0; i < (length - 1); i++)
                {

                    int comparison;
                    comparison = String.Compare(variables_to_sort[i].name, variables_to_sort[i + 1].name);


                    if (comparison == 1)
                    {
                        class_types.m_variable bubble_temp;
                        bubble_temp = variables_to_sort[i + 1];
                        variables_to_sort[i + 1] = variables_to_sort[i];
                        variables_to_sort[i] = bubble_temp;
                        wrong++;
                    }

                }
                if (wrong == 0)
                    break;
            }


            for (int i = 0; i < variables_to_sort.Length; i++)
                result.AddLast(variables_to_sort[i]);

            return result;
        }

        public class_types.CodeResult ForwardToFBVar(string input_var, class_types.m_file_data input_m_file)
        {//repoint variables do status vector

            class_types.CodeResult result = new class_types.CodeResult();
            string var = "";
            string FB_ID = "";
            string name = "";

            if (input_var.Contains("."))
            {

                for (int i = 0; i < input_m_file.variables.Count; i++)//find variable
                {
                    if (input_var.Substring(0, input_var.IndexOf(".")) + "_FBID" == input_m_file.variables.ElementAt(i).name)
                    {
                        FB_ID = input_m_file.variables.ElementAt(i).value;//retrieve FB_ID
                        name = input_m_file.variables.ElementAt(i).name.Substring(0, input_m_file.variables.ElementAt(i).name.IndexOf("_FBID"));
                        var = ReplaceIllegalChars(input_var.Substring(input_var.IndexOf(".") + 1));
                        break;
                    }
                }
                if (name != "" && FB_ID != "")
                {
                    switch (FB_ID)
                    {
                        case "'TIMER_ON'":
                            switch (var)
                            {
                                case "IN":
                                    result.code = name + "(2)";
                                    result.comment += name + ".IN";
                                    result.success = true;
                                    break;
                                case "ET":
                                    result.code = name + "(3)";
                                    result.comment += name + ".T";
                                    result.success = true;
                                    break;
                                case "PT":
                                    result.code = name + "(4)";
                                    result.comment += name + ".Q";
                                    result.success = true;
                                    break;
                                case "Q":
                                    result.code = name + "(6)";
                                    result.comment += name + ".Q";
                                    result.success = true;
                                    break;
                            }
                            break;
                        case "'COUNTER_UP'":
                            switch (var)
                            {
                                case "CU":
                                    result.code = name + "(2)";
                                    result.comment += name + ".CU";
                                    result.success = true;
                                    break;
                                case "CV":
                                    result.code = name + "(3)";
                                    result.comment += name + ".CV";
                                    result.success = true;
                                    break;
                                case "PV":
                                    result.code = name + "(4)";
                                    result.comment += name + ".PV";
                                    result.success = true;
                                    break;
                                case "RESET":
                                    result.code = name + "(5)";
                                    result.comment += name + ".RESET";
                                    result.success = true;
                                    break;
                                case "Q":
                                    result.code = name + "(6)";
                                    result.comment += name + ".Q";
                                    result.success = true;
                                    break;
                            }
                            break;
                        case "'COUNTER_DOWN'":
                            switch (var)
                            {
                                case "CD":
                                    result.code = name + "(2)";
                                    result.comment += name + ".CD";
                                    result.success = true;
                                    break;
                                case "CV":
                                    result.code = name + "(3)";
                                    result.comment += name + ".CV";
                                    result.success = true;
                                    break;
                                case "PV":
                                    result.code = name + "(4)";
                                    result.comment += name + ".PV";
                                    result.success = true;
                                    break;
                                case "LOAD":
                                    result.code = name + "(5)";
                                    result.comment += name + ".LOAD";
                                    result.success = true;
                                    break;
                                case "Q":
                                    result.code = name + "(6)";
                                    result.comment += name + ".Q";
                                    result.success = true;
                                    break;

                            }
                            break;
                    }
                }
                else
                    result.success = false; 
            }
            else
                result.success = false;

            return result;
        }

        public LinkedList<class_types.CodeResult> ReadParametersFromIdentifier(string parameters,LinkedList<string> var_list, class_types.m_file_data input_m_file)
        {
            class_types.CodeResult clear_argument=new class_types.CodeResult();
            LinkedList<class_types.CodeResult> result = new LinkedList<class_types.CodeResult>();
            string temp_var_name;

            for (int i=0; i<var_list.Count; i++)
            {
                int start_index=-1;
                int length=-1;
                try{start_index=parameters.IndexOf(var_list.ElementAt(i));}catch{};
                if(start_index>-1) 
                {
                    start_index += var_list.ElementAt(i).Length;
                    temp_var_name="";
                    clear_argument.code = "";
                    clear_argument.comment = "";
                    int comma_index=-1;
                    int parenthesis_index=-1;
                    if (parameters.Length>start_index+1)
                    {
                        try{comma_index=parameters.Substring(start_index).IndexOf(",");}catch{};
                        try{parenthesis_index=parameters.Substring(start_index).IndexOf(")");}catch{};

                        if (comma_index!=-1||parenthesis_index!=-1)
                        {
                            if (comma_index==-1)
                                length = parenthesis_index;
                            if (parenthesis_index == -1)
                                length = comma_index;

                            if (length==-1&&comma_index <= parenthesis_index)
                                length = comma_index;
                            else
                                length = parenthesis_index;



                            temp_var_name = parameters.Substring(start_index,length);
                            //start and end index determined
                            //check for special variables
                            clear_argument = ForwardToFBVar(temp_var_name, input_m_file);
                            if (!clear_argument.success)
                            {
                                clear_argument.code=ReplaceIllegalChars(temp_var_name);
                                clear_argument.comment="";
                                clear_argument.success = true;
                            }

                        }
                        else
                            clear_argument.success=false; 

                    }
                    result.AddLast(clear_argument);
                }
                else
                    clear_argument.success = false; 
                

            }
            return result;
        }

        class_types.SimpleResult ConvertIEC(string path, string outpath)
        {
            class_types.SimpleResult resultVar;
            class_types.m_file_data m_file = new class_types.m_file_data();
            resultVar.success = true;
            resultVar.message = "";
            int Line_A = 0; //Line_A is a helper for lines currently being worked
            int original_code_index = 0; //the deleted lines from the code are added here, hence it represents the line index of the
            //original code, for error reporting messages
            int status_set = 0; //TODO include description of states
            class_types.read_function_output Grind;

            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            //TODO replace manual constructor in class
            m_file = new class_types.m_file_data();
            m_file.var_block_comments = new LinkedList<string>();
            m_file.var_block_comments.Clear();
            m_file.codelines = new LinkedList<class_types.m_CodeLine>();
            m_file.variables = new LinkedList<class_types.m_variable>();

            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------


            LinkedList<string> InCode = new LinkedList<string>();
            InCode = ReadFromText(path);

            if (InCode == null)
            {
                resultVar.success = false;
                resultVar.message = "Code not found.";
                return resultVar;
            }

            //if it has a program title, read it, otherwise skip it
            //se tem título de programa, tá fixe, senão passa ao próximo.

            while (status_set != -1)
            {
                if (InCode.Count > 0) //check for EOF or empty file
                {
                    if (InCode.First() != "") //check for empty line
                    {
                        switch (status_set)
                        {
                            case 0: //read program title
                                Grind = ReadProgramTitle(InCode.First(), m_file, original_code_index);
                                m_file = Grind.m_file;
                                if (Grind.success)
                                {
                                    status_set = 1; //title read, onto variable declarations
                                    Line_A = Grind.lines_forward;
                                    original_code_index += Grind.lines_forward;
                                    if (m_file.title.code == "")//if title was read but empty
                                    {
                                        m_file.title.code = "NoTitle";
                                        resultVar.message += "Warning: No title found. " + Grind.message;
                                    }

                                    //clean source code from processed lines
                                    while (Line_A > 0)
                                    {
                                        InCode.RemoveFirst();
                                        Line_A--;
                                        original_code_index++;
                                    }

                                }
                                else //if title can't be read ----------------------check workings
                                {
                                    resultVar.success = false;
                                    resultVar.message += "Error attempting to read title in line " + original_code_index + ". " + Grind.message;
                                    return resultVar;
                                }
                                outpath += m_file.title.code + ".m"; //filename is program title

                                break;
                            case 1://read all variable declarations
                                if (RemoveComment(InCode.First()).code == "")
                                {

                                    original_code_index++;
                                    class_types.m_CodeLine commentonly = new class_types.m_CodeLine();
                                    
                                    commentonly.code="";
                                    commentonly.comment=RemoveComment(InCode.First()).comment;

                                    m_file.codelines.AddLast(commentonly);
                                    InCode.RemoveFirst();

                                }
                                else
                                {
                                    if (InCode.First().Substring(0, 3) == "VAR")
                                    {
                                        Grind = ReadVariables(InCode, m_file, original_code_index);//read a block of a set of input/output/internal variables
                                        m_file = Grind.m_file;
                                        if (!Grind.success)
                                        {
                                            resultVar.success = false;
                                            resultVar.message += "Error reading variables. " + Grind.message;
                                            return resultVar;
                                        }
                                        else
                                        {
                                            resultVar.message += Grind.message;
                                        }
                                    }
                                    else //no VAR_XXXX instruction
                                    {//TODO revise
                                        if (InCode.First() != "") //if line isn't empty
                                            status_set = 2;//all variables are read
                                        //do not remove current line, let next case deal with it
                                    }

                                }




                                break;
                            case 2: //instructions time

                                Grind = ReadCode(InCode, m_file, original_code_index);//read a block of code
                                m_file = Grind.m_file;
                                if (!Grind.success)
                                {
                                    resultVar.success = false;
                                    resultVar.message += "Error reading code at line " + original_code_index + ". " + Grind.message;
                                    return resultVar;
                                }
                                else
                                {
                                    resultVar.message += Grind.message;
                                    original_code_index = Grind.lines_forward;
                                    status_set = 10; //All code read
                                }


                                break;

                            case 10: //write to file
                                //write all conversions to files from memory
                                class_types.SimpleResult result;
                                if (!(result = writeToFile(outpath, m_file, resultVar.message)).success)
                                {
                                    resultVar.success = false;
                                    resultVar.message += "Error occurred while writing file " + m_file.title.code + " to  " + outpath + ". " + result.message;
                                    return resultVar;
                                }
                                else
                                {
                                    resultVar.success = true;
                                    resultVar.message += result.message;
                                    status_set = -1;
                                }
                                break;

                        }
                    }
                    else //in case line is empty
                    {
                        InCode.RemoveFirst();
                        original_code_index++;
                    }
                }
                else //in case of EOF
                {
                    status_set = 10;
                    if (m_file.variables.Count == 0 && m_file.codelines.Count == 0) //if code is empty
                    {
                        resultVar.success = false;
                        resultVar.message = "No code found. ";
                        return resultVar;
                    }

                    //write all conversions to files from memory
                    class_types.SimpleResult result;
                    if (!(result = writeToFile(outpath, m_file, resultVar.message)).success)
                    {
                        resultVar.success = false;
                        resultVar.message += "Error occurred while writing file" + m_file.title + " to  " + outpath + ". " + result.message;
                        return resultVar;
                    }
                    else
                    {
                        resultVar.success = true;
                        resultVar.message += result.message;
                        status_set = -1;
                    }
                    break;
                }


            }



            return resultVar;
        }

        class_types.read_function_output ReadProgramTitle(string code, class_types.m_file_data m_file_input, int current_line) 
        {
            class_types.read_function_output result;
            class_types.CodeResult TempCodeComment;
            int wrong_spaces = 0;
            int name_length = 0;

            result.lines_forward = 1;
            result.success = false;
            result.message = "";
            result.m_file = m_file_input;

            //comments "(* comment *)" removal and adding
            TempCodeComment = RemoveComment(code);

            if (TempCodeComment.success == false)//check for malformed comments
            {
                result.success = false;
                result.message += "Error in comments at line " + (current_line + 1) + ". ";
                return result;
            }

            result.m_file.title.comment = TempCodeComment.comment;
            code = TempCodeComment.code;

            while (code.Substring(wrong_spaces, 1) == " " && wrong_spaces < code.Length)//clean up wrong spaces
                wrong_spaces++;

            if (wrong_spaces > 0)
                result.message += "Warning: There extra spaces in line " + (current_line + 1) + ". Matlaber was able to continue, but you should fix this. ";

            if (code.Length < 9)
            {
                result.success = false;
                result.message = "Error: Invalid code. ";
            }
            else
            {
                if (code.Substring(wrong_spaces, 8) == "PROGRAM ")
                {
                    if (code.Substring(9 + wrong_spaces, code.Length - 9 - wrong_spaces).Contains(" "))
                        name_length = code.IndexOf(" ", 9) - 8 - wrong_spaces;
                    else
                        name_length = code.Length - 8 - wrong_spaces;

                    result.m_file.title.code = code.Substring(8 + wrong_spaces, name_length);
                    result.success = true;

                }
                else //in case PROGRAM instruction is not found
                {
                    result.success = false;
                    result.message = "Error: PROGRAM instruction not found. ";
                }
            }

            return result;
        }

        class_types.read_function_output ReadVariables(LinkedList<string> InCode, class_types.m_file_data m_file_input, int current_line)// function to read a block of variables
        {


            class_types.read_function_output result;
            class_types.CodeResult TempCodeComment;
            class_types.m_variable var_temp = new class_types.m_variable();


            int wrong_spaces = 0;
            string IO_type = "";
            int colon_index, semicolon_index, colon_2_index = 0;



            result.lines_forward = 1;
            result.success = false;
            result.message = "";

            result.m_file = m_file_input;



            //comments "(* comment *)" removal and adding to var block comment
            TempCodeComment = RemoveComment(InCode.First());
            if (TempCodeComment.success == false)//check for malformed comments
            {
                result.success = false;
                result.message += "Error in comments at line " + (current_line + current_line ) + ". ";
                return result;
            }

            //var block's comment line is here
            string temp =TempCodeComment.comment;
            //if (TempCodeComment.comment != "")
            //    result.m_file.var_block_comments.AddLast(TempCodeComment.comment);


            InCode.RemoveFirst();
            InCode.AddFirst(TempCodeComment.code);

            //account for empty lines
            while (InCode.First() == "")
            {
                InCode.RemoveFirst();
                result.lines_forward++;
                if (InCode.Count <= 0) //in case the EOF is encountered while searching for non-empty lines
                {
                    result.success = false;
                    result.message += "Sudden end of file at line " + (current_line + result.lines_forward + 1) + ". ";
                    return result;
                }
            }

            while (InCode.First().Substring(wrong_spaces, 1) == " " && wrong_spaces < InCode.First().Length)//clean up wrong spaces
       
             wrong_spaces++;

            //code line clean, onto var block definition (IO type)

            int IO_case = -1;
            int before_retention = 0;
            class_types.m_variable var_temp_vector = new class_types.m_variable();


                if (InCode.First().Length > 8 && InCode.First().Substring(0, 9) == "VAR_INPUT")
                    IO_case = 0;
                else
                {
                    if (InCode.First().Length > 9 && InCode.First().Substring(0, 10) == "VAR_OUTPUT")
                        IO_case = 1;
                    else
                    {
                        if (InCode.First().Length > 9 && InCode.First().Substring(0, 10) == "VAR_GLOBAL")
                            IO_case = 2;
                        else
                        {
                            if (InCode.First().Length > 2 && InCode.First().Substring(0, 3) == "VAR")
                             IO_case = 2;
                        }

                    }
                }
                

                switch (IO_case)
                {
                    case 0:
                        IO_type = "input";
                        before_retention = 8;
                        break;
                    case 1:
                        IO_type = "output";
                        before_retention = 9;
                        break;
                    case 2:
                        IO_type = "global";
                        before_retention = 9;
                        break;
                }


            //Check for retention parameters
            if (InCode.First().Length > (before_retention + 7))//"RETAIN"
            {
                wrong_spaces = 0;
                while (InCode.First().Substring(wrong_spaces + before_retention + 1, 1) == " " && wrong_spaces < InCode.First().Length - before_retention - 1)//clean up wrong spaces
                    wrong_spaces++;
                if (InCode.First().Substring(wrong_spaces + before_retention + 1, 6) == "RETAIN")
                    var_temp.retain = true;
                else
                    var_temp.retain = false;
            }

            if (IO_type == "") //case no type of variable is understood
            {
                result.success = false;
                result.message = result.message + "Error: invalid variable IO option at line  " + (current_line + result.lines_forward + 1) + ". ";
                return result;
            }
            else
            {
                //complete var block's comment
                if (temp!="")
                    result.m_file.var_block_comments.AddLast(IO_type + " block's comment: " + temp);
            }


            InCode.RemoveFirst();
            result.lines_forward++;

            //account for empty lines
            while (InCode.First() == "")
            {
                InCode.RemoveFirst();
                result.lines_forward++;
                if (InCode.Count <= 0) //in case the EOF is encountered while searching for non-empty lines
                {
                    result.success = false;
                    result.message += "Sudden end of file at line " + (current_line + result.lines_forward + 1) + ". ";
                    return result;
                }
            }

            //vars declaration loop
            while (InCode.First() != "" && InCode.First().Substring(0, 7) != "END_VAR")
            {
                wrong_spaces = 0;
                colon_index = 0;
                colon_2_index = 0;
                semicolon_index = 0;
                while (InCode.First().Substring(wrong_spaces, 1) == " " && wrong_spaces < InCode.First().Length)//clean up wrong spaces
                    wrong_spaces++;

                if (InCode.First().Substring(wrong_spaces, 7) == "END_VAR")//end current variable block;
                {
                    result.success = true;
                    break;
                }
                else//here is the process to read variable information
                {
                    //comments "(* comment *)" removal and adding
                    var_temp.comment = RemoveComment(InCode.First()).comment; //write variable's comment
                    TempCodeComment = RemoveComment(InCode.First());
                    if (TempCodeComment.success == false)//check for malformed comments
                    {
                        result.success = false;
                        result.message += "Error in comments at line " + (current_line + result.lines_forward + 1) + ". ";
                        return result;
                    }
                    InCode.RemoveFirst();
                    InCode.AddFirst(RemoveSpaces(TempCodeComment.code));//code line clean from comments and empty spaces                  

                    //TODO pointer type variables are diferent in declaration, add those

                    if (!(InCode.First().Contains(":") && InCode.First().Contains(";")))//in case the declaration line is not valid
                    {
                        result.success = false;
                        result.message += "Error: invalid declaration at line  " + (current_line + result.lines_forward + 1) + ". ";
                        return result;
                    }
                    //the declaration line must have all of these elements in order to be valid

                    colon_index = InCode.First().IndexOf(":");
                    semicolon_index = InCode.First().IndexOf(";");

                    //check for consistency of elements' order in declaration
                    if (InCode.First().Contains(":="))//if we have two colons, then we have an initial value declared
                    {
                        colon_2_index = InCode.First().IndexOf(":=");
                        if (colon_2_index > semicolon_index || colon_index > colon_2_index || colon_index > semicolon_index)//smaller index protection against EOL error;
                        {
                            result.success = false;
                            result.message += "Error: invalid declaration at line  " + (current_line + result.lines_forward + 1) + ". ";
                            return result;
                        }
                    }
                    else
                    {
                        if (colon_index > semicolon_index)//smaller index protection against EOL error;
                        {
                            result.success = false;
                            result.message += "Error: invalid declaration at line  " + (current_line + result.lines_forward + 1) + ". ";
                            return result;
                        }
                    }                 
                    
                    //all is good, just read the values
                    var_temp.name = ReplaceIllegalChars(InCode.First().Substring(0, colon_index));
                    if (colon_2_index > 0)
                    {
                        var_temp.type = InCode.First().Substring(colon_index + 1, colon_2_index - colon_index - 1);
                        var_temp.value = InCode.First().Substring(colon_2_index + 2, semicolon_index - colon_2_index - 2);//write initial value
                        if (var_temp.type == "BOOL")//TODO: check this
                        {                           //Boolean values conversion from TRUE, FALSE to 1, 0;
                            if (var_temp.value == "TRUE")
                                var_temp.value = "1";
                            if (var_temp.value == "FALSE")
                                var_temp.value = "0";
                        }
                        var_temp.IO_type = IO_type;//write the variable's IO type
                        result.m_file.variables.AddLast(var_temp);                        
                    }
                    else//no initial value declared
                    {
                        var_temp.type = InCode.First().Substring(colon_index + 1, semicolon_index - colon_index - 1);
                        string default_value = "0"; //aplicable for most var types, just check the exceptions
                        
                        if (var_temp.type=="TIME")
                        {
                            var_temp.type = "INT";
                        }

                        if (var_temp.type == "CTU" || var_temp.type == "CTD" || var_temp.type == "TON") //FB variable, declare vector of variables required for FB call to work
                        {
                            switch (var_temp.type)
                            {
                                case "TON":
                                    TIMER_ON_flag = true;
                                    var_temp_vector = new class_types.m_variable();
                                    var_temp_vector.IO_type = IO_type;
                                    var_temp_vector.retain = false;
                                    var_temp_vector.comment = "";
                                    var_temp_vector.type = "INT";
                                    var_temp_vector.value = "[1,0,0,0,0,0]";
                                    var_temp_vector.name = var_temp.name;
                                    result.m_file.variables.AddLast(var_temp_vector);

                                    //FB identifier
                                    var_temp_vector.comment = "FB identifier";
                                    var_temp_vector.type = "STRING";
                                    var_temp_vector.value = "'TIMER_ON'";
                                    var_temp_vector.name = var_temp.name + "_FBID";
                                    result.m_file.variables.AddLast(var_temp_vector);//FB identifier
                                    break;

                                case "CTU":
                                    COUNTER_UP_flag = true;
                                    var_temp_vector = new class_types.m_variable();
                                    var_temp_vector.IO_type = IO_type;
                                    var_temp_vector.retain = false;
                                    var_temp_vector.comment = "";
                                    var_temp_vector.type = "INT";
                                    var_temp_vector.value = "[0,0,0,0,0,0]";
                                    var_temp_vector.name = var_temp.name;
                                    result.m_file.variables.AddLast(var_temp_vector);

                                    //FB identifier
                                    var_temp_vector.comment = "FB identifier";
                                    var_temp_vector.type = "STRING";
                                    var_temp_vector.value = "'COUNTER_UP'";
                                    var_temp_vector.name = var_temp.name + "_FBID";
                                    result.m_file.variables.AddLast(var_temp_vector);//FB identifier
                                    break;
                                case "CTD":
                                    COUNTER_DOWN_flag = true;
                                    var_temp_vector = new class_types.m_variable();
                                    var_temp_vector.IO_type = IO_type;
                                    var_temp_vector.retain = false;
                                    var_temp_vector.comment = "";
                                    var_temp_vector.type = "INT";
                                    var_temp_vector.value = "[0,0,0,0,0,0]";
                                    var_temp_vector.name = var_temp.name;
                                    result.m_file.variables.AddLast(var_temp_vector);

                                    //FB identifier
                                    var_temp_vector.comment = "FB identifier";
                                    var_temp_vector.type = "STRING";
                                    var_temp_vector.value = "'COUNTER_DOWN'";
                                    var_temp_vector.name = var_temp.name + "_FBID";
                                    result.m_file.variables.AddLast(var_temp_vector);//FB identifier
                                    break;
                                default:
                                    break;
                            }
                        }
                        else 
                        {
                            switch (var_temp.type) //for every simple type, there is a different defaul initial value
                            {
                                case "BOOL"://TODO ADD THE REMAINING VARIABLE TYPE'S DEFAULT VALUE
                                    default_value = "0";
                                    break;
                                case "STRING":
                                    default_value = "''";
                                    break;
                                case "CHAR":
                                    default_value = "''";
                                    break;
                                case "WORD":
                                    default_value = "0";
                                    break;
                                case "INT":
                                    default_value = "0";
                                    break;
                            }
                            var_temp.value=default_value;

                            var_temp.IO_type = IO_type;//write the variable's IO type
                            result.m_file.variables.AddLast(var_temp);
                        }
                    }
                    //check for approved variable types
                    if (!(var_temp.type == "TON" || var_temp.type == "CTD" || var_temp.type == "CTU" || var_temp.type == "WORD" || var_temp.type == "BOOL" || var_temp.type == "BYTE" || var_temp.type == "LWORD" || var_temp.type == "DWORD" || var_temp.type == "SINT" || var_temp.type == "INT" || var_temp.type == "DINT" || var_temp.type == "LINT" || var_temp.type == "USINT" || var_temp.type == "UINT" || var_temp.type == "UDINT" || var_temp.type == "ULINT" || var_temp.type == "REAL" || var_temp.type == "LREAL" || var_temp.type == "DATE" || var_temp.type == "TOD" || var_temp.type == "DT" || var_temp.type == "TIME" || var_temp.type == "STRING" || var_temp.type == "WSTRING"))
                    {
                        result.success = false;
                        result.message = result.message + "Error: invalid variable type declaration at line  " + (current_line + result.lines_forward + 1) + ". ";
                        return result;
                    }

                    result.success = true;

                    InCode.RemoveFirst();//onto next line
                    result.lines_forward++;
                }

            }

            //account for empty lines
            while (InCode.First() == "")
            {
                InCode.RemoveFirst();
                result.lines_forward++;
                if (InCode.Count <= 0) //in case the EOF is encountered while searching for non-empty lines
                {
                    result.success = false;
                    result.message += "Sudden end of file at line " + (current_line + result.lines_forward + 1) + ". ";
                    return result;
                }
            }

            wrong_spaces = 0;
            while (InCode.First().Substring(wrong_spaces, 1) == " " && wrong_spaces < InCode.First().Length) //clean up wrong spaces
                wrong_spaces++;

            //reached END_VAR instruction
            if (InCode.First().Substring(wrong_spaces, 7) == "END_VAR")
            {
                InCode.RemoveFirst();//onto next line, END_VAR marks the success of the variable declaration
                result.lines_forward++;
                result.success = true;

            }
            else
            {
                result.success = false;
                result.message += "Error: 'END_VAR' expected  at " + (current_line + result.lines_forward + 1) + ". ";
            }

            return result;
        }

        class_types.read_function_output ReadCode(LinkedList<string> InCode, class_types.m_file_data m_file_input, int current_line)// function to read and convert the code
        {
            class_types.read_function_output result;
            class_types.CodeResult TempCodeComment;

            int line_number = 0;
            result.lines_forward = 0;
            result.success = true;
            result.message = "";
            class_types.m_CodeLine workline;
            workline.code="";
            workline.comment="";
            string acumulator = "";
            bool instruction_ran=false;
            class_types.Instruction_and_Argument CodeAndArg;

            //Remove empty lines
            while (InCode.Contains(""))
                InCode.Remove("");


            string[] TempArray=new string[InCode.Count];

            TempArray = InCode.ToArray();


            result.m_file = m_file_input;
            line_number = 0;
  
            //here the first instruction is expected, if no valid instruction is found, return error
            while (line_number < TempArray.Length)
            {
                //workline.comment = "";
                if (TempArray[line_number].Length > 0)
                {
                    //comments "(* comment *)" removal and adding to var block comment
                    TempCodeComment.code = "";
                    TempCodeComment.comment = "";
                    TempCodeComment.success = false;
                    TempCodeComment = RemoveComment(TempArray[line_number]);
                    if (TempCodeComment.success == false)//check for malformed comments
                    {
                        result.success = false;
                        result.message += "Error in comments at line " + (current_line + line_number + 1) + ". ";
                        return result;
                    }
                    if (TempCodeComment.comment != "")//write comment to code line
                        workline.comment += TempCodeComment.comment + " || ";

                    TempArray[line_number] = TempCodeComment.code;
                    if (TempArray[line_number] == "")//case of no code and only comment
                    {
                        workline.code = "";
                        result.m_file.codelines.AddLast(workline);
                        workline.comment = "";
                    }
                }

                if (TempArray[line_number].Length < 1 || RemoveSpaces(TempArray[line_number]) == ";")
                    {
                        result.lines_forward++;
                        instruction_ran = true;
                        InCode.RemoveFirst();
                        TempArray = InCode.ToArray();
                    }
                    else
                    {

                        if (TempArray[line_number].Length >= 11 && TempArray[line_number].Substring(0, 11) == "END_PROGRAM") //END_PROGRAM line
                        {
                            if (result.m_file.codelines.Count <= 0)
                            {
                                result.success = false;
                                result.message += "Empty program. ";
                            }
                            else
                            {
                                result.success = true;
                                result.lines_forward = line_number;

                                for (int j = 0; j < result.lines_forward; j++)
                                    InCode.RemoveFirst();

                                return result;
                            }
                        }

                        try
                        {
                            int index_of_colon = 0;
                            index_of_colon = TempArray[line_number].IndexOf(":");

                            if (index_of_colon > 0 && !TempArray[line_number].Contains(":=")) //LABEL statement, produces 4 lines of code, calling goto.m
                            {
                                GOTO_flag = true;
                                workline.code = "";

                                if (workline.comment != "")
                                {
                                    workline.comment = TempArray[line_number].Substring(0, TempArray[line_number].IndexOf(':')) + " LABEL'S COMMENT: ";
                                    result.m_file.codelines.AddLast(workline);
                                }
                                workline.code = "";

                                workline.comment = "LABEL " +  ReplaceIllegalChars(TempArray[line_number].Substring(0, index_of_colon));

                                result.m_file.codelines.AddLast(workline);
                                workline.code = "";
                                workline.comment = "";
                                if (TempArray[line_number].Substring(index_of_colon) != "")
                                {

                                    if (RemoveSpaces(TempArray[line_number].Substring(index_of_colon)).Length==1 && RemoveSpaces(TempArray[line_number].Substring(index_of_colon))==";")
                                    {
                                        InCode.RemoveFirst();
                                        TempArray = InCode.ToArray();
                                        result.lines_forward++;
                                    }
                                    else
                                        TempArray[line_number] = TempArray[line_number].Substring(index_of_colon+1);
                                }
                                else
                                {
                                    InCode.RemoveFirst();
                                    TempArray = InCode.ToArray();
                                    result.lines_forward++;
                                }   
                            }
                        }
                        catch { }

                        CodeAndArg = SeparateInstFromArg(TempArray[line_number]);//create a code and argument reference, only if not END_PROGRAM or LABEL 

                        if (CodeAndArg.argument.Contains(".") && CodeAndArg.instruction != "CAL")//case of writing to special structured variable, such as CTU or TON
                        {
                            class_types.CodeResult clear_argument=ForwardToFBVar(CodeAndArg.argument,result.m_file);
                            if (clear_argument.success)
                            {
                                CodeAndArg.argument=clear_argument.code;
                                workline.comment+=clear_argument.comment;
                            }
                        
                        }                         

                        if (CodeAndArg.instruction == "EQ" || CodeAndArg.instruction == "GT" || CodeAndArg.instruction == "GE" || CodeAndArg.instruction == "NE" || CodeAndArg.instruction == "LE" || CodeAndArg.instruction == "LT")
                        {
                            string operation = "";

                            switch (CodeAndArg.instruction)
                            {
                                case "EQ":
                                    operation = "==";
                                    break;
                                case "GT":
                                    operation = ">";
                                    break;
                                case "GE":
                                    operation = ">=";
                                    break;
                                case "NE":
                                    operation = "~=";
                                    break;
                                case "LE":
                                    operation = "<=";
                                    break;
                                case "LT":
                                    operation = "<";
                                    break;
                            }

                            acumulator = "(" + acumulator + operation + " " + ReplaceIllegalChars(CodeAndArg.argument) + ")";

                        }

                        if (CodeAndArg.instruction.Length>2 && CodeAndArg.instruction.Substring(0, 3) == "RET")
                        {
                            workline.code = "return";
                            result.m_file.codelines.AddLast(workline);
                            workline.comment = "";
                            instruction_ran = true;
                            result.lines_forward++;
                        }

                        if (CodeAndArg.instruction == "CAL")
                        {
                            //identify the type of call
                            string FB_ID="";
                            string name = "";

                            if (CodeAndArg.argument.Contains("("))
                            {
                                for (int i = 0; i < result.m_file.variables.Count; i++)//find variable
                                {
                                    if (CodeAndArg.argument.Substring(0, CodeAndArg.argument.IndexOf("(")) + "_FBID" == result.m_file.variables.ElementAt(i).name)
                                    {
                                        FB_ID = result.m_file.variables.ElementAt(i).value;//retrieve FB_ID
                                        name = result.m_file.variables.ElementAt(i).name.Substring(0, result.m_file.variables.ElementAt(i).name.IndexOf("_FBID"));
                                        break;
                                    }

                                }
                            }

                            if (FB_ID != "")
                            {

                                LinkedList<string> identifiers = new LinkedList<string>();
                                LinkedList<class_types.CodeResult> parameter_vars = new LinkedList<class_types.CodeResult>();
                                
                                switch (FB_ID)
                                {
                                    case "'TIMER_ON'":
                                        TIMER_ON_flag = true;
                                        workline.code = "";
                                        if (workline.comment != "")
                                        {
                                            workline.comment = name + " FB call comment: " + workline.comment;
                                            result.m_file.codelines.AddLast(workline);
                                        }
                                        //read parameters

                                        //format of parameteres: IN:=VAR1,PT:= VAR2,CV=>VAROUT
                                        identifiers.Clear();                                        
                                        identifiers.AddLast("IN:=");
                                        identifiers.AddLast("PT:=");
                                        identifiers.AddLast("ET=>");

                                        parameter_vars.Clear();
                                        parameter_vars = ReadParametersFromIdentifier(CodeAndArg.argument, identifiers, result.m_file);
                                        workline.comment = "";

                                        if (parameter_vars.Count > 0 && parameter_vars.ElementAt(0).success)
                                        {
                                            workline.code += name + "(2)=" + parameter_vars.ElementAt(0).code + ";";
                                            if (parameter_vars.ElementAt(0).comment != "")
                                                workline.comment += parameter_vars.ElementAt(0).comment;
                                        }
                                        if (parameter_vars.Count > 1 && parameter_vars.ElementAt(1).success)
                                        {
                                            workline.code += name + "(4)=" + parameter_vars.ElementAt(1).code + ";";
                                            if (parameter_vars.ElementAt(1).comment != "")
                                                workline.comment += parameter_vars.ElementAt(0).comment;
                                        }
                                        workline.code += name + "=TIMER_ON(CLOCK_IN," + name + ");";
                                        if (parameter_vars.Count>2 && parameter_vars.ElementAt(2).success && parameter_vars.ElementAt(2).code != "0")//write CV value to designated variable
                                            workline.code += parameter_vars.ElementAt(2).code + "=" + name + "(3)";

                                        result.m_file.codelines.AddLast(workline);

                                        break;
                                    case "'COUNTER_DOWN'":
                                        COUNTER_DOWN_flag = true;
                                        workline.code = "";
                                        if (workline.comment != "")
                                        {
                                            workline.comment = name + " FB call comment: " + workline.comment;
                                            result.m_file.codelines.AddLast(workline);
                                        }

                                        //read parameters
                                        identifiers.Clear();
                                        identifiers.AddLast("LOAD:=");
                                        identifiers.AddLast("PV:=");
                                        identifiers.AddLast("CV=>");

                                        parameter_vars.Clear();
                                        parameter_vars = ReadParametersFromIdentifier(CodeAndArg.argument, identifiers, result.m_file);
                                        workline.comment = "";

                                        if (parameter_vars.Count > 0 && parameter_vars.ElementAt(0).success)
                                        {
                                            workline.code += name + "(5)=" + parameter_vars.ElementAt(0).code + ";";
                                            if (parameter_vars.ElementAt(0).comment != "")
                                                workline.comment += parameter_vars.ElementAt(0).comment;
                                        }
                                        if (parameter_vars.Count > 1 && parameter_vars.ElementAt(1).success)
                                        {
                                            workline.code += name + "(4)=" + parameter_vars.ElementAt(1).code + ";";
                                            if (parameter_vars.ElementAt(1).comment != "")
                                                workline.comment += parameter_vars.ElementAt(0).comment;
                                        }
                                        workline.code += name + "=COUNTER_DOWN(" + name + ");";

                                        if (parameter_vars.Count>2 && parameter_vars.ElementAt(2).success && parameter_vars.ElementAt(2).code != "0")//write CV value to designated variable
                                            workline.code += parameter_vars.ElementAt(2).code + "=" + name + "(3)";

                                        result.m_file.codelines.AddLast(workline);
                                        workline.comment = "";
                                        break;

                                    case "'COUNTER_UP'":
                                        COUNTER_UP_flag = true;
                                        workline.code = "";
                                        if (workline.comment != "")
                                        {
                                            workline.comment = name + " FB call comment: " + workline.comment;
                                            result.m_file.codelines.AddLast(workline);
                                            workline.comment = "";
                                        }

                                        //read parameters
                                        identifiers.Clear();
                                        identifiers.AddLast("RESET:=");
                                        identifiers.AddLast("PV:=");
                                        identifiers.AddLast("CV=>");

                                        parameter_vars.Clear();
                                        parameter_vars = ReadParametersFromIdentifier(CodeAndArg.argument, identifiers, result.m_file);
                                        workline.comment = "";

                                        if (parameter_vars.Count > 0 && parameter_vars.ElementAt(0).success)
                                        {
                                            workline.code += name + "(5)=" + parameter_vars.ElementAt(0).code + ";";
                                            if (parameter_vars.ElementAt(0).comment != "")
                                                workline.comment += parameter_vars.ElementAt(0).comment;
                                        }
                                        if (parameter_vars.Count > 1 && parameter_vars.ElementAt(1).success)
                                        {
                                            workline.code += name + "(4)=" + parameter_vars.ElementAt(1).code + ";";
                                            if (parameter_vars.ElementAt(1).comment != "")
                                                workline.comment += parameter_vars.ElementAt(0).comment;
                                        }
                                        workline.code += name + "=COUNTER_UP(" + name + ");";

                                        if (parameter_vars.Count>2 && parameter_vars.ElementAt(2).success && parameter_vars.ElementAt(2).code != "0")//write CV value to designated variable
                                            workline.code += parameter_vars.ElementAt(2).code + "=" + name + "(3);";

                                        result.m_file.codelines.AddLast(workline);
                                        workline.comment = "";
                                        break;
                                }
                            }
                            else
                            {
                                workline.code = CodeAndArg.argument.Substring(0,CodeAndArg.argument.IndexOf("(") ) + "(" + CodeAndArg.argument.Substring(CodeAndArg.argument.IndexOf("(") + 1, CodeAndArg.argument.IndexOf(")") - CodeAndArg.argument.IndexOf("(") - 1) + ");";
                                result.m_file.codelines.AddLast(workline);
                                workline.comment = "";
                                instruction_ran = true;
                            }

                        }

                        if (CodeAndArg.instruction == "JMPC" || CodeAndArg.instruction == "JMPCN" || CodeAndArg.instruction == "JMP")//JMP statements
                        {
                            GOTO_flag = true;
                            if (acumulator == "")
                            {
                                result.success = false;
                                result.message += "Error in code at line " + (current_line + line_number + 1) + ": JMPC instruction before any value is in the acumulator. ";
                                return result;
                            }

                            if (CodeAndArg.instruction == "JMP")
                            {
                                workline.comment = "";
                                workline.code = "goto('" + ReplaceIllegalChars(CodeAndArg.argument) + "');";
                                result.m_file.codelines.AddLast(workline);
                                workline.code = "return";
                                workline.comment = "Part of the goto functionality in Matlab, ignore";
                                result.m_file.codelines.AddLast(workline);
                            }
                            else
                            {
                                if (CodeAndArg.instruction == "JMPCN")
                                    workline.code = "if (" + acumulator + "==0)";
                                else
                                    workline.code = "if (" + acumulator + "==1)";
                                result.m_file.codelines.AddLast(workline);
                                
                                workline.comment = "";
                                workline.code = "    goto('" + ReplaceIllegalChars(CodeAndArg.argument) + "');";
                                result.m_file.codelines.AddLast(workline);
                                workline.code = "    return";
                                workline.comment = "Part of the goto functionality in Matlab, ignore";
                                result.m_file.codelines.AddLast(workline);
                                workline.comment = "";
                                workline.code = "end";
                                result.m_file.codelines.AddLast(workline);
                                workline.comment = "";
                                instruction_ran = true;

                            }
                        }
                    
                        if (CodeAndArg.instruction == "ST" || CodeAndArg.instruction == "ST(") //ST instruction found, begin backtracking from ElementAt(end_instruction_line) up to First()
                        {
                            if (acumulator == "")//In case ST is the first instruction encountered
                            {
                                result.success = false;
                                result.message += "Error in code at line " + (current_line + line_number + 1) + ": ST found before any LD.";
                                return result;
                            }
                            
                            //start writing the workline
                            workline.code = ReplaceIllegalChars(RemoveEndSpaces(CodeAndArg.argument)) + " = " + acumulator + ";";//If a acumulator like behaviour is needed, we use this. Examples: JMPC, EQ, etc..
                            result.m_file.codelines.AddLast(workline);
                            workline.comment = "";
                            workline.code = "";
                            result.lines_forward++;
                        }

                        if (CodeAndArg.instruction == "S" || CodeAndArg.instruction == "R")
                        {
                            workline.code = ReplaceIllegalChars(CodeAndArg.argument) + " = ";
                            if (CodeAndArg.instruction == "S")
                                workline.code += 1+";";
                            if (CodeAndArg.instruction == "R")
                                workline.code += 0+";";

                            result.m_file.codelines.AddLast(workline);
                            workline.comment = "";
                            workline.code = "";
                            instruction_ran = true;
                            result.lines_forward++;
                        }
                        
                        switch (CodeAndArg.instruction)//behaviour dependent on the instruction
                        {                              //TODO: complete instruction variations (negate and parenthesis) and add the remainder instructions
                            case ")":
                                acumulator += ")";
                                instruction_ran = true;
                                break;
                            case "LD":
                                acumulator = ReplaceIllegalChars(CodeAndArg.argument);
                                instruction_ran = true;
                                break;
                            case "LDN":
                                acumulator = "~" + ReplaceIllegalChars(CodeAndArg.argument);
                                instruction_ran = true;
                                break;
                            case "LD(":
                                acumulator = "(" + ReplaceIllegalChars(CodeAndArg.argument);
                                instruction_ran = true;
                                break;
                            case "LDN(":
                                acumulator = "(~" + ReplaceIllegalChars(CodeAndArg.argument);
                                instruction_ran = true;
                                break;
                            case "AND":
                                acumulator = "(" + acumulator;
                                acumulator += "&" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "AND(":
                                acumulator = "((" + acumulator;
                                acumulator += "&" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "&":
                                acumulator = "(" + acumulator;
                                acumulator += "&" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "&(":
                                acumulator = "((" + acumulator;
                                acumulator += "&" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "&~":
                                acumulator = "(" + acumulator;
                                acumulator += "&~" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "ANDN":
                                acumulator = "(" + acumulator;
                                acumulator += "&~" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "ORN":
                                acumulator = "(" + acumulator;
                                acumulator += "|~" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "ORN(":
                                acumulator = "(" + acumulator;
                                acumulator += "|~(" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "OR":
                                acumulator = "(" + acumulator;
                                acumulator += "|" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "OR(":
                                acumulator += "|(" + ReplaceIllegalChars(CodeAndArg.argument);
                                instruction_ran = true;
                                break;
                            case "XOR":
                                acumulator = "xor(" + acumulator + "," + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "XORN":
                                acumulator = "xor(" + acumulator + "," + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "ADD":
                                acumulator = "(" + acumulator;
                                acumulator += "+" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "ADD(":
                                acumulator = "((" + acumulator;
                                acumulator += "+" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "SUB":
                                acumulator = "(" + acumulator;
                                acumulator += "-" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "DIV":
                                acumulator = "(" + acumulator;
                                acumulator += "/" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                            case "MUL":
                                acumulator = "(" + acumulator;
                                acumulator += "*" + ReplaceIllegalChars(CodeAndArg.argument) + ")";
                                instruction_ran = true;
                                break;
                        }
                        if (instruction_ran)
                        {
                            InCode.RemoveFirst();
                            TempArray = InCode.ToArray();
                        }

                    }
                
            }
                   
                    result.lines_forward = line_number;

                    for (int i = 0; i < result.lines_forward; i++)//remove the processed lines
                        InCode.RemoveFirst();
                    
                    result.success = false;
                    result.message += "No END_PROGRAM instruction found. ";

                    return result;
        }

        class_types.SimpleResult writeToFile(string outpath, class_types.m_file_data m_file, string log)
        {
            class_types.SimpleResult result;
            result.success = false;
            result.message = "";
            string codeline = "";
            bool written = false;
            LinkedList<string> OutputText = new LinkedList<string>();
            LinkedList<string> OutputStartfile = new LinkedList<string>();
            LinkedList<string> Temp_File = new LinkedList<string>();

            if (!(Directory.Exists(PathToDirectory(outpath))))
                Directory.CreateDirectory(PathToDirectory(outpath));

            if (!IsFileEmpty(outpath))
            {
                DialogResult UserInput = MessageBox.Show("File already exists at " + outpath + " . \n Overwrite it?", "File already exists", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                //possible replys: "OK" and "Cancel"
                //MessageBox.Show(UserInput.ToString(),"Read it", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (!(UserInput.ToString() == "OK"))
                {

                    result.success = false;
                    result.message += "File already exists, user canceled overwrite. ";
                    return result;

                }
            }


            LinkedList<class_types.m_variable> sorted_vars = SortVariables(m_file);

            m_file.variables.Clear();

            for (int k = 0; k < sorted_vars.Count; k++)
                m_file.variables.AddLast(sorted_vars.ElementAt(k));


            if (GOTO_flag)
            {
                try
                {
                    Temp_File.Clear();
                    Temp_File = GetFileFromAsssembly("goto.m", "GOTO");

                    string Temp_File_Path = ReverseString(ReverseString(outpath).Substring(ReverseString(outpath).IndexOf("\\"))) + "goto.m";


                    TextWriter ztw = new StreamWriter(Temp_File_Path);

                    for (int i = 0; i < Temp_File.Count; i++)
                        ztw.WriteLine(Temp_File.ElementAt(i)); //write codeline to file
                    ztw.Close();// close the stream
                }
                catch { }
            }

            if (TIMER_ON_flag)
            {
                try
                {
                    Temp_File.Clear();
                    Temp_File = GetFileFromAsssembly("TIMER_ON.m", "POU");

                    string Temp_File_Path = ReverseString(ReverseString(outpath).Substring(ReverseString(outpath).IndexOf("\\"))) + "TIMER_ON.m";


                    TextWriter ztw = new StreamWriter(Temp_File_Path);

                    for (int i = 0; i < Temp_File.Count; i++)
                        ztw.WriteLine(Temp_File.ElementAt(i)); //write codeline to file
                    ztw.Close();// close the stream
                }
                catch { }
            }

            if (COUNTER_UP_flag)
            {
                try
                {
                    Temp_File.Clear();
                    Temp_File = GetFileFromAsssembly("COUNTER_UP.m", "POU");

                    string Temp_File_Path = ReverseString(ReverseString(outpath).Substring(ReverseString(outpath).IndexOf("\\"))) + "COUNTER_UP.m";


                    TextWriter ztw = new StreamWriter(Temp_File_Path);

                    for (int i = 0; i < Temp_File.Count; i++)
                        ztw.WriteLine(Temp_File.ElementAt(i)); //write codeline to file
                    ztw.Close();// close the stream
                }
                catch { }
            }
            if (COUNTER_DOWN_flag)
            {
                try
                {
                    Temp_File.Clear();
                    Temp_File = GetFileFromAsssembly("COUNTER_DOWN.m", "POU");

                    string Temp_File_Path = ReverseString(ReverseString(outpath).Substring(ReverseString(outpath).IndexOf("\\"))) + "COUNTER_DOWN.m";


                    TextWriter ztw = new StreamWriter(Temp_File_Path);

                    for (int i = 0; i < Temp_File.Count; i++)
                        ztw.WriteLine(Temp_File.ElementAt(i)); //write codeline to file
                    ztw.Close();// close the stream
                }
                catch { }
            }


            OutputStartfile.AddLast("%-This file declares all the variables used and sets their initial value---------");
            OutputStartfile.AddLast("%------Run this before running the main m file-----------------------------------");
            OutputStartfile.AddLast("%------------------------------------------------------------------------------");
            OutputStartfile.AddLast("");
            OutputStartfile.AddLast("%------------------------variables' initialization---------------------------------");


            codeline = "global ";

            for (int i = 0; i < m_file.variables.Count; i++)
            {
                //|| m_file.variables.ElementAt(i).IO_type == "output"
                if (m_file.variables.ElementAt(i).IO_type == "global" || m_file.variables.ElementAt(i).retain || m_file.variables.ElementAt(i).IO_type == "output")
                {
                    codeline += m_file.variables.ElementAt(i).name + " ";
                    written = true;
                }
            }

            if (written)
            {
                codeline = codeline.Substring(0, codeline.Length - 1);//clean last unnecessary comma
                written = false;

            }
            else
            {
                codeline = "";
            }

            OutputStartfile.AddLast(codeline);

            for (int i = 0; i < m_file.variables.Count; i++) //run all output variables for initialization
            {
                if (m_file.variables.ElementAt(i).IO_type == "global" || m_file.variables.ElementAt(i).retain || m_file.variables.ElementAt(i).IO_type == "output")
                {
                    OutputStartfile.AddLast(m_file.variables.ElementAt(i).name + "=" + m_file.variables.ElementAt(i).value + ";");
                }
            }
            codeline = "";

            //Global initialization
            OutputStartfile.AddLast("");


            OutputStartfile.AddLast("%------------------------------------------------------------------------------");
            string startfilepath = outpath.Substring(0, outpath.Length - 2) + "_startfile.m";

            TextWriter tw = new StreamWriter(startfilepath);

            for (int i = 0; i < OutputStartfile.Count; i++)
                tw.WriteLine(OutputStartfile.ElementAt(i)); //write codeline to file

            // close the stream
            tw.Close();


            //------------------------end of start file writing-----------------------------

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------
            // write the title m-file to the OutputText, including it's variables
            codeline = "function [OUTPUT_VECTOR]=" + outpath.Substring(outpath.LastIndexOf("\\") + 1, outpath.LastIndexOf('.') - outpath.LastIndexOf("\\") - 1) + "("; //write title;

            //write the outputs

            codeline += "CLOCK_IN"; //Clock in is always a variable
            for (int i = 0; i < m_file.variables.Count; i++)
            {
                if (m_file.variables.ElementAt(i).IO_type == "input")
                {
                    codeline += "," + m_file.variables.ElementAt(i).name;
                    written = true;
                }
            }


            codeline += ")"; //close parenthesis

            OutputText.AddLast(codeline);
            if (m_file.title.comment != "")
            {
                OutputText.AddLast("%"+m_file.title.comment);//add title comment line, if there is one
            }

            OutputText.AddLast("");

            codeline = "%Converted by IEC-Matlab converter by Andre' Pereira";
            OutputText.AddLast(codeline);
            DateTime localNow = DateTime.Now;
            codeline = "%" + localNow + " " + TimeZone.CurrentTimeZone.StandardName;
            OutputText.AddLast(codeline);

            OutputText.AddLast("");
            OutputText.AddLast("");
            //an empty line, between title and global variables' declaration



            codeline = "global ";

            for (int i = 0; i < m_file.variables.Count; i++)
            {
                //|| m_file.variables.ElementAt(i).IO_type == "output"
                if (m_file.variables.ElementAt(i).IO_type == "global" || m_file.variables.ElementAt(i).retain || m_file.variables.ElementAt(i).IO_type == "output")
                {
                    codeline += m_file.variables.ElementAt(i).name + " ";
                    written = true;
                }
            }

            if (written)
            {
                codeline = codeline.Substring(0, codeline.Length - 1);//clean last unnecessary comma
                written = false;

            }
            else
            {
                codeline = "";
            }

            OutputText.AddLast(codeline);





            OutputText.AddLast("");
            OutputText.AddLast("");


            if (m_file.var_block_comments.Count > 0)
            {
                OutputText.AddLast("");
                OutputText.AddLast("");
                //-------------------Start file writing-----------------------
                OutputText.AddLast("%--------------------------Var blocks' comments--------------------------------");
                for (int i = 0; i < m_file.var_block_comments.Count; i++)
                    OutputText.AddLast("%" + m_file.var_block_comments.ElementAt(i));

                OutputText.AddLast("%------------------------------------------------------------------------------");
                OutputText.AddLast("");
            }


            bool has_var_comments = false;

            for (int i = 0; i < m_file.variables.Count; i++) //add all variables' comments
            {
                if (m_file.variables.ElementAt(i).comment != "")
                {
                    has_var_comments = true;
                    break;
                }
            }

            if (has_var_comments)
            {
                OutputText.AddLast("%--------------------------Variables' comments---------------------------------");
                for (int i = 0; i < m_file.variables.Count; i++) //add all variables' comments
                {
                    if (m_file.variables.ElementAt(i).comment != "")
                        OutputText.AddLast("%" + m_file.variables.ElementAt(i).IO_type + " " + m_file.variables.ElementAt(i).name + "'s comment: " + m_file.variables.ElementAt(i).comment);
                }
                OutputText.AddLast("%------------------------------------------------------------------------------");

                //Var blocks comments
                OutputText.AddLast("");


            }
            OutputText.AddLast("");

            string outputvector = "OUTPUT_VECTOR=[";

            for (int i = 0; i < m_file.variables.Count; i++)
            {
                if (m_file.variables.ElementAt(i).IO_type == "output")
                {
                    outputvector += m_file.variables.ElementAt(i).name + "*1,";
                    written = true;
                }
            }
            if (written)
            {
                outputvector = outputvector.Substring(0, outputvector.Length - 1);//clean last unnecessary comma
                written = false;
            }
            outputvector += "];";







            for (int i = 0; i < m_file.codelines.Count; i++)
            {
                string line_construction = "";
                if (m_file.codelines.ElementAt(i).code != "")
                {
                    //Special, write outputs before calling Return
                    
                    if (m_file.codelines.ElementAt(i).code.Contains("return") )
                    {
                        if (m_file.codelines.ElementAt(i).comment != "Part of the goto functionality in Matlab, ignore")
                            OutputText.AddLast(outputvector+ " % AUTOMATIC: Write output vector before calling return");
                                    
                            
                    }
                    line_construction = m_file.codelines.ElementAt(i).code;

                   
                }


                if (m_file.codelines.ElementAt(i).comment != "")
                {
                    if (m_file.codelines.ElementAt(i).code != "")
                        line_construction += " % " + m_file.codelines.ElementAt(i).comment;
                    else
                        line_construction += "% " + m_file.codelines.ElementAt(i).comment;
                }

                OutputText.AddLast(line_construction);
            }

            //write output_vector

            OutputText.AddLast("");
            OutputText.AddLast("%Output generation");



            OutputText.AddLast(outputvector);


            // create a writer and open the file
            tw = new StreamWriter(outpath);

            for (int i = 0; i < OutputText.Count; i++)
                tw.WriteLine(OutputText.ElementAt(i)); //write codeline to file

            // close the stream
            tw.Close();
            result.success = true;

            return result;
        }




    }
}
