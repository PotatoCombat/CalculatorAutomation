using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace CalculatorAutomation
{
    public class CalculatorProcess : BaseProcess
    {
        private static readonly string ProcessName = "Calculator";
        private static readonly string ExecutableFilename = "Calc.exe";

        private Process _process;

        // Container element
        public AutomationElement processContainer;

        public AutomationElement titleBar;
        public AutomationElement menuBar;
        public AutomationElement pane;

        public AutomationElement resultTextBox;

        public ExpandCollapsePattern viewMenu;
        public ExpandCollapsePattern editMenu;
        public ExpandCollapsePattern helpMenu;

        private Dictionary<string, ExpandCollapsePattern> _menuItemsByName;

        private Dictionary<string, InvokePattern> _buttonsByName;

        public string Result
        {
            get
            {
                return (string)resultTextBox.GetCurrentPropertyValue(AutomationElement.NameProperty);
            }
        }

        public override void Run()
        {
            Console.WriteLine($"Initializing Process...");
            InitializeProcess();
            Console.WriteLine($"Initializing Window Elements...");
            InitializeWindowElements();
            Console.WriteLine($"Initializing Menu Items...");
            InitializeMenuItems();
            Console.WriteLine($"Initializing Pane Buttons...");
            InitializePaneButtons();
        }

        public override void Dispose()
        {
            _process.CloseMainWindow();
            _process.Dispose();
        }

        //TODO: Convert button names into an enumeration
        public CalculatorProcess InvokeButton(string name)
        {
            _buttonsByName[name].Invoke();
            return this;
        }

        public CalculatorProcess InvokeButtonWithDelay(string name, int milliseconds = 200)
        {
            _buttonsByName[name].Invoke();
            Thread.Sleep(milliseconds);
            return this;
        }

        public CalculatorProcess ExpandMenuItemWithDelay(string name, int milliseconds = 200)
        {
            _menuItemsByName[name].Expand();
            Thread.Sleep(milliseconds);
            return this;
        }

        public CalculatorProcess CollapseMenuItemWithDelay(string name, int milliseconds = 200)
        {
            _menuItemsByName[name].Collapse();
            Thread.Sleep(milliseconds);
            return this;
        }

        public CalculatorProcess InvokeMenuButtonWithDelay(string name, int milliseconds = 200)
        {
            AutomationElement element = new AutomationQuery(menuBar)
                .AddName(name)
                .FindFirst(TreeScope.Descendants);

            AutomationUtility.GetInvokePattern(element).Invoke();
            Thread.Sleep(milliseconds);
            return this;
        }

        public void Evaluate()
        {
            InvokeButton("Equals");
        }

        public void Clear()
        {
            InvokeButton("Clear");
        }

        private void InitializeProcess()
        {
            _process = Process.Start(ExecutableFilename);

            processContainer = new AutomationQuery(AutomationElement.RootElement)
              .AddName(ProcessName)
              .FindFirst(TreeScope.Children);
        }

        private void InitializeWindowElements()
        {
            titleBar = new AutomationQuery(processContainer)
                .AddControlType(ControlType.TitleBar)
                .FindFirst(TreeScope.Children);

            menuBar = new AutomationQuery(processContainer)
                .AddControlType(ControlType.MenuBar)
                .FindFirst(TreeScope.Children);

            pane = new AutomationQuery(processContainer)
                .AddControlType(ControlType.Pane)
                .FindFirst(TreeScope.Children);
        }

        private void InitializeMenuItems()
        {
            _menuItemsByName = new AutomationQuery(menuBar)
                .AddControlType(ControlType.MenuItem)
                .FindAll(TreeScope.Descendants)
                .ToDictionary(
                    k => (string)k.GetCurrentPropertyValue(AutomationElement.NameProperty),
                    v => (ExpandCollapsePattern)v.GetCurrentPattern(ExpandCollapsePattern.Pattern));
        }

        private void InitializePaneButtons()
        {
            _buttonsByName = new AutomationQuery(pane)
                .AddControlType(ControlType.Button)
                .FindAll(TreeScope.Descendants)
                .ToDictionary(
                    k => (string)k.GetCurrentPropertyValue(AutomationElement.NameProperty),
                    v => (InvokePattern)v.GetCurrentPattern(InvokePattern.Pattern));
        }
    }
}
