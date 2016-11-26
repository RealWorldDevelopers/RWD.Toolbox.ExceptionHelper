

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RWD.Toolbox.ExceptionHelper.WinForm
{
    public class ExceptionHelper
    {

        private ExceptionDisplay ErrorDisplayForm = new ExceptionDisplay();

        private bool _display = true;
        public bool Display
        {
            get { return _display; }
            set { _display = value; }
        }

        private string _displayTitle = "Error";
        public string DisplayTitle
        {
            get { return _displayTitle; }
            set { _displayTitle = value; }
        }
        
        public bool AllowManualReporting { get; set; }

        public bool IncludeInnerExceptions { get; set; }

        public void DisplayErrorToUser(Exception e, string clipboardData)
        {
            var defaultSource = "Unknown Source";
            var defaultErrorAsString = e.ToString();

            //put in users clipboard
            if (AllowManualReporting)
                Clipboard.SetText(clipboardData);

            //treview
            if (Display)
            {
                ErrorDisplayForm = new ExceptionDisplay();
                ErrorDisplayForm.Title = DisplayTitle;
                ErrorDisplayForm.ReportingEnabled = AllowManualReporting;
                ErrorDisplayForm.RootNode = BuildExceptionTreeNode(e);
                ErrorDisplayForm.ExceptionSource = defaultSource;
                ErrorDisplayForm.ExceptionString = defaultErrorAsString;
                ErrorDisplayForm.ShowDialog();
            }

            //clear users clipboard
            Clipboard.Clear();
        }

        public TreeNode BuildExceptionTreeNode(Exception e)
        {
            TreeNode SubNode = new TreeNode();
            TreeNode ChildNode = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(e.Message))
                {
                    SubNode.Text = "Message: " + e.Message.Replace(Environment.NewLine, " ").Trim();
                }

                ChildNode = new TreeNode("ExceptionType: " + e.GetType().FullName.Trim());
                SubNode.Nodes.Add(ChildNode);

                // TODO handle specific exceptions here
                if (e is FileNotFoundException)
                {
                    var ex = e as FileNotFoundException;
                    if (!string.IsNullOrWhiteSpace(ex.FileName))
                    {
                        ChildNode = new TreeNode("FileName: " + ex.FileName.Trim());
                        SubNode.Nodes.Add(ChildNode);
                    }
                }

                if (e is ArgumentNullException)
                {
                    var ex = e as ArgumentNullException;
                    if (!string.IsNullOrWhiteSpace(ex.ParamName))
                    {
                        ChildNode = new TreeNode("ParamName: " + ex.ParamName.Trim());
                        SubNode.Nodes.Add(ChildNode);
                    }

                }

                if (!string.IsNullOrWhiteSpace(e.Source))
                {
                    ChildNode = new TreeNode("Source: " + e.Source.Trim());
                    SubNode.Nodes.Add(ChildNode);
                }
                if (e.TargetSite != null)
                {
                    ChildNode = new TreeNode("TargetSite: " + e.TargetSite.ToString().Trim());
                    SubNode.Nodes.Add(ChildNode);
                }
                if (!string.IsNullOrWhiteSpace(e.StackTrace))
                {
                    var sts = e.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    ChildNode = new TreeNode("StackTrace");
                    SubNode.Nodes.Add(ChildNode);

                    foreach (var item in sts)
                    {
                        var nNode = new TreeNode(item.Trim());
                        ChildNode.Nodes.Add(nNode);
                    }
                }
                if (e.Data.Keys.Count > 0)
                {
                    ChildNode = new TreeNode("Data");
                    SubNode.Nodes.Add(ChildNode);

                    foreach (var key in e.Data.Keys)
                    {
                        var nNode = new TreeNode(key + ": " + e.Data[key]);
                        ChildNode.Nodes.Add(nNode);
                    }
                }
                if (e.InnerException != null && IncludeInnerExceptions)
                {
                    ChildNode = new TreeNode("InnerException");
                    ChildNode.Nodes.Add(BuildExceptionTreeNode(e.InnerException));
                    SubNode.Nodes.Add(ChildNode);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ErrorHandling.ErrorHandle.BuildExceptionTreeNode()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return SubNode;

        }

        public string BuildExceptionString(Exception e, string tab = "")
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                string parentTab = tab;
                string childTab = parentTab + "\t";
                string grandChildTab = childTab + "\t";

                if (!string.IsNullOrWhiteSpace(e.Message))
                    sb.AppendLine(parentTab + "Message: " + e.Message.Replace(Environment.NewLine, " ").Trim());

                sb.AppendLine(childTab + "ExceptionType: " + e.GetType().FullName.Trim());

                if (!string.IsNullOrWhiteSpace(e.Source))
                    sb.AppendLine(childTab + "Source: " + e.Source.Trim());

                if (e.TargetSite != null)
                    sb.AppendLine(childTab + "TargetSite: " + e.TargetSite.ToString().Trim());

                if (!string.IsNullOrWhiteSpace(e.StackTrace))
                {
                    var sts = e.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    sb.AppendLine(childTab + "StackTrace");
                    foreach (var item in sts)
                    {
                        sb.AppendLine(grandChildTab + item.Trim());
                    }
                }

                // TODO handle specific exceptions here
                if (e is FileNotFoundException)
                {
                    var ex = e as FileNotFoundException;
                    if (!string.IsNullOrWhiteSpace(ex.FileName))
                        sb.AppendLine(childTab + "FileName: " + ex.FileName.Trim());
                }

                if (e is ArgumentNullException)
                {
                    var ex = e as ArgumentNullException;
                    if (!string.IsNullOrWhiteSpace(ex.ParamName))
                        sb.AppendLine(childTab + "ParamName: " + ex.ParamName.Trim());
                }

                if (e.Data.Keys.Count > 0)
                {
                    sb.AppendLine(childTab + "Data");
                    foreach (var key in e.Data.Keys)
                    {
                        sb.AppendLine(grandChildTab + key + ": " + e.Data[key]);
                    }
                }

                if (e.InnerException != null && IncludeInnerExceptions)
                {
                    sb.AppendLine(childTab + "InnerException:");
                    sb.AppendLine(BuildExceptionString(e.InnerException, childTab + "\t"));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ErrorHandling.ErrorHandle.BuildExceptionString()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return sb.ToString();

        }

        public string BuildExceptionXml(Exception e, bool recurse = false)
        {
            string errorAsXml = string.Empty;

            try
            {
                if (!recurse)
                    errorAsXml = "<?xml version=\"1.0\"?><Exception>";

                if (!string.IsNullOrWhiteSpace(e.Message))
                {
                    errorAsXml += "<Message>" + System.Security.SecurityElement.Escape(e.Message.Replace(Environment.NewLine, " ").Trim()) + "</Message>";
                }

                errorAsXml += "<ExceptionType>" + System.Security.SecurityElement.Escape(e.GetType().FullName.Trim()) + "</ExceptionType>";

                // TODO handle specific exceptions here
                if (e is FileNotFoundException)
                {
                    var ex = e as FileNotFoundException;
                    if (!string.IsNullOrWhiteSpace(ex.FileName))
                    {
                        errorAsXml += "<FileName>" + System.Security.SecurityElement.Escape(ex.FileName.Trim()) + "</FileName>";
                    }
                }

                if (e is ArgumentNullException)
                {
                    var ex = e as ArgumentNullException;
                    if (!string.IsNullOrWhiteSpace(ex.ParamName))
                    {
                        errorAsXml += "<ParamName>" + System.Security.SecurityElement.Escape(ex.ParamName.Trim()) + "</ParamName>";
                    }
                }

                if (!string.IsNullOrWhiteSpace(e.Source))
                {
                    errorAsXml += "<Source>" + System.Security.SecurityElement.Escape(e.Source.Trim()) + "</Source>";
                }

                if (e.TargetSite != null)
                {
                    errorAsXml += "<TargetSite>" + System.Security.SecurityElement.Escape(e.TargetSite.ToString().Trim()) + "</TargetSite>";
                }

                if (!string.IsNullOrWhiteSpace(e.StackTrace))
                {
                    errorAsXml += "<StackTrace>";
                    var sts = e.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in sts)
                    {
                        var nNode = new TreeNode(item.Trim());
                        errorAsXml += "<Location>" + System.Security.SecurityElement.Escape(item.Trim()) + "</Location>";
                    }
                    errorAsXml += "</StackTrace>";
                }

                if (e.Data.Keys.Count > 0)
                {
                    errorAsXml += "<Data>";
                    foreach (var key in e.Data.Keys)
                    {
                        errorAsXml += "<item>";
                        errorAsXml += "<key>" + System.Security.SecurityElement.Escape(key.ToString().Trim()) + "</key> ";
                        errorAsXml += "<value>" + System.Security.SecurityElement.Escape(e.Data[key].ToString().Trim()) + "</value> ";
                        errorAsXml += "</item>";
                    }
                    errorAsXml += "</Data>";
                }

                if (e.InnerException != null && IncludeInnerExceptions)
                {
                    errorAsXml += "<InnerException>";
                    errorAsXml += BuildExceptionXml(e.InnerException, true);
                    errorAsXml += "</InnerException>";
                }

                if (!recurse)
                    errorAsXml += "</Exception>";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ErrorHandling.ErrorHandle.BuildExceptionTreeNode()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return errorAsXml;

        }

        public string BuildExceptionJson(Exception e)
        {
            // TODO use Nuget JSON.Net
            // use http://www.newtonsoft.com/json/help/html/ConvertingJSONandXML.htm

            return string.Empty;
        }

        public string BuildExceptionHtml(Exception e)
        {
            // TODO use my snippets

            return string.Empty;
        }


    }
}
