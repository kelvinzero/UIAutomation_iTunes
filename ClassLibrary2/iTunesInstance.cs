using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace ClassLibrary1
{
    public class iTunesInstance : IDisposable{




        public iTunesInstance()
        {
            _iTunesProcess = Process.Start("C:\\Program Files\\iTunes\\iTunes.exe");

            int count = 0;

            do{
                _iTunesAutomationElement = AutomationElement.RootElement.FindFirst
                    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "iTunes"));
                ++count;
                Thread.Sleep(100);
            }
            while (_iTunesAutomationElement == null && count < 50);

            if (_iTunesAutomationElement == null)
                throw new InvalidOperationException("iTunesAutomationElement null");

            _searchBarAutomationElement = _iTunesAutomationElement.FindFirst
                (TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Search Library"));

            

            if (_searchBarAutomationElement == null)
                throw new InvalidOperationException("searchBarAutomationElement is null");


        }

        public AutomationElement getSearchBarElement()
        {
            return _searchBarAutomationElement;
        }
        
        public object searchBarText
        {
            get
            {
                return _searchBarAutomationElement.GetCurrentPropertyValue
                    (AutomationElement.NameProperty);
            }
            set
            {
                string val = value.ToString();
                InvokePattern invokePattern = _searchBarAutomationElement
                    .GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                invokePattern.Invoke();

                // Pause before sending keyboard input.
                Thread.Sleep(100);

                // Delete existing content in the control and insert new content.
                SendKeys.SendWait("^{HOME}");   // Move to start of control
                SendKeys.SendWait("^+{END}");   // Select everything
                SendKeys.SendWait("{DEL}");     // Delete selection
                SendKeys.SendWait(val);
                SendKeys.SendWait("{DOWN}");
                SendKeys.SendWait("\n");

                // insertTextSearchBar(_searchBarAutomationElement, val);
            }
        }

        public bool isSelected(string val)
        {
            AutomationElement songItem = _iTunesAutomationElement.FindFirst
                (TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "play targeted, Sidewalks (feat. Kendrick Lamar), Time 3:51, The Weeknd, Album Starboy, Genre R&B/Soul"));
            if(songItem == null)
            {
                MessageBox.Show("Song not selected");
                return false;
            }
            MessageBox.Show("Song selected successfully");
            return true;
        }
        public InvokePattern GetInvokePattern(AutomationElement element)
        {
            return element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
        }


        public void Dispose()
        {
            _iTunesProcess.Dispose();
        }

        /**
         * https://msdn.microsoft.com/en-us/library/ms750582(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
         */
        private void insertTextSearchBar(AutomationElement element,
                                    string value)
        {
            try
            {
                // Validate arguments / initial setup
                if (value == null)
                    throw new ArgumentNullException(
                        "String parameter must not be null.");

                if (element == null)
                    throw new ArgumentNullException(
                        "AutomationElement parameter must not be null");

                // A series of basic checks prior to attempting an insertion.
                //
                // Check #1: Is control enabled?
                // An alternative to testing for static or read-only controls 
                // is to filter using 
                // PropertyCondition(AutomationElement.IsEnabledProperty, true) 
                // and exclude all read-only text controls from the collection.
                if (!element.Current.IsEnabled)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId.ToString()
                        + " is not enabled.\n\n");
                }

                // Check #2: Are there styles that prohibit us 
                //           from sending text to this control?
                if (!element.Current.IsKeyboardFocusable)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId.ToString()
                        + "is read-only.\n\n");
                }


                // Once you have an instance of an AutomationElement,  
                // check if it supports the ValuePattern pattern.
                object valuePattern = null;

                // Control does not support the ValuePattern pattern 
                // so use keyboard input to insert content.
                //
                // NOTE: Elements that support TextPattern 
                //       do not support ValuePattern and TextPattern
                //       does not support setting the text of 
                //       multi-line edit or document controls.
                //       For this reason, text input must be simulated
                //       using one of the following methods.
                //       
                if (!element.TryGetCurrentPattern(
                    ValuePattern.Pattern, out valuePattern))
                {

                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    // Pause before sending keyboard input.
                    Thread.Sleep(100);

                    // Delete existing content in the control and insert new content.
                    SendKeys.SendWait("^{HOME}");   // Move to start of control
                    SendKeys.SendWait("^+{END}");   // Select everything
                    SendKeys.SendWait("{DEL}");     // Delete selection
                    SendKeys.SendWait(value);
                }
                // Control supports the ValuePattern pattern so we can 
                // use the SetValue method to insert content.
                else
                {
                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    ((ValuePattern)valuePattern).SetValue(value);
                }
            }
            catch (ArgumentNullException exc)
            {
                ;
            }
            catch (InvalidOperationException exc)
            {
                ;
            }
            finally
            {
                ;
            }
        }


        Process _iTunesProcess;
        AutomationElement _iTunesAutomationElement;
        AutomationElement _searchBarAutomationElement;
    }
}
