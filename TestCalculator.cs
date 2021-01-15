/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace CalculatorAutomation
{
    public class Program
    {
        private BaseProcess _process;

        public Program(BaseProcess process)
        {
            this._process = process;
        }

        public void Run()
        {
            Console.WriteLine("Opening program");
            this._process.Run();

            // Allow 2 seconds to view changes in the Console Log
            Thread.Sleep(2000);

            Console.WriteLine("Closing program");
            this._process.Dispose();

            // Allow 1 seconds to view changes in the Console Log
            Thread.Sleep(1000);
        }

        public static void Main(string[] args)
        {
            Program program = new Program(new CalculatorProcess());
            program.Run();
        }
    }

    public interface IRunnable
    {
        void Run();
    }

    public abstract class BaseProcess : IRunnable, IDisposable
    {
        public abstract void Run();
        public abstract void Dispose();
    }

    public class CalculatorProcess : BaseProcess
    {
        private static readonly string ProcessName = "Calculator";
        private static readonly string ExecutableFilename = "Calc.exe";

        private Process _process;

        // Container element
        public AutomationElement processContainer;

        public AutomationElement resultTextBox;

        public ExpandCollapsePattern viewMenu;
        public ExpandCollapsePattern editMenu;
        public ExpandCollapsePattern helpMenu;

        private Dictionary<string, InvokePattern> buttonsByName;

        public override void Run()
        {
            Initialize();
            InvokeButton("1");
            Test_ChangeResultTextBox();
            Test_InvokeButtons();
            Test_SelectMenuItems();
        }

        public override void Dispose()
        {
            _process.CloseMainWindow();
            _process.Dispose();
        }

        private void Initialize()
        {
            Console.WriteLine($"Initializing...");
            _process = Process.Start(ExecutableFilename);

            Console.WriteLine($"Finding running process: {ProcessName}");
            processContainer = new AutomationQuery(AutomationElement.RootElement)
              .AddName(ProcessName)
              .FindFirst(TreeScope.Children);

            Console.WriteLine($"Identifying interface elements...");
            resultTextBox = new AutomationQuery(processContainer)
              .AddId("150")
              .FindFirst(TreeScope.Descendants);

            viewMenu = new AutomationQuery(processContainer)
                .AddName("View")
                .FindFirstExpandCollapsePattern(TreeScope.Descendants);
            
            editMenu = new AutomationQuery(processContainer)
                .AddName("Edit")
                .FindFirstExpandCollapsePattern(TreeScope.Descendants);

            helpMenu = new AutomationQuery(processContainer)
                .AddName("Help")
                .FindFirstExpandCollapsePattern(TreeScope.Descendants);

            buttonsByName = new Dictionary<string, InvokePattern>();
            string[] buttonNames =
            {
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Equals", "Clear", "Add", "Subtract", "Multiply", "Divide",
            };

            foreach (string buttonName in buttonNames)
            {
                buttonsByName[buttonName] = new AutomationQuery(processContainer)
                    .AddClassName("Button")
                    .AddName(buttonName)
                    .FindFirstInvokePattern(TreeScope.Descendants);
            }
        }

        //TODO: Convert button names into an enumeration
        public void InvokeButton(string name)
        {
            *//*AutomationElement element = new AutomationQuery(processContainer)
                .AddClassName("Button")
                .AddName(name)
                .FindFirst(TreeScope.Descendants);

            AutomationUtils.FindInvokePattern(element).Invoke();*//*

            buttonsByName[name].Invoke();
        }

        public void InvokeButtonWithDelay(string name, int milliseconds = 200)
        {
            buttonsByName[name].Invoke();
            Thread.Sleep(milliseconds);
        }

        public void InvokeMenuItem(string name)
        {
            AutomationElement element = new AutomationQuery(processContainer)
                .AddName(name)
                .FindFirst(TreeScope.Descendants);

            AutomationUtils.FindInvokePattern(element).Invoke();
        }

        public void Evaluate()
        {
            InvokeButton("Equals");
        }

        public void Clear()
        {
            InvokeButton("Clear");
        }

        private void Test_ChangeResultTextBox()
        {
            Console.WriteLine($"The value in the resultTextBox is: {resultTextBox.GetCurrentPropertyValue(AutomationElement.NameProperty)}");
            // Manually change the number within 5 seconds
            Thread.Sleep(2000);
            Console.WriteLine($"The value in the resultTextBox is: {resultTextBox.GetCurrentPropertyValue(AutomationElement.NameProperty)}");
        }

        private void Test_InvokeButtons()
        {
            Console.WriteLine($"The value in the resultTextBox is: {resultTextBox.GetCurrentPropertyValue(AutomationElement.NameProperty)}");
            // Manually change the number within 5 seconds
            Thread.Sleep(2000);
            InvokeButtonWithDelay("1");
            InvokeButtonWithDelay("2");
            InvokeButtonWithDelay("3");
            InvokeButtonWithDelay("4");
            InvokeButtonWithDelay("5");
            InvokeButtonWithDelay("Add");
            InvokeButtonWithDelay("1");
            InvokeButtonWithDelay("2");
            InvokeButtonWithDelay("3");
            InvokeButtonWithDelay("4");
            Evaluate();
            Thread.Sleep(1000);
            Console.WriteLine($"The value in the resultTextBox is: {resultTextBox.GetCurrentPropertyValue(AutomationElement.NameProperty)}");
        }

        private void Test_SelectMenuItems()
        {
            editMenu.Expand();
            Thread.Sleep(2000);
            InvokeMenuItem("Copy");
            Thread.Sleep(2000);
            editMenu.Collapse();
        }
    }

    public class AutomationQuery
    {
        private AutomationElement _rootElement;
        private List<Condition> _conditions;

        public AutomationQuery(AutomationElement rootElement)
        {
            this._rootElement = rootElement;
            this._conditions = new List<Condition>();
        }

        public AutomationQuery AddPropertyCondition(PropertyCondition propertyCondition)
        {
            this._conditions.Add(propertyCondition);
            return this;
        }

        public AutomationQuery AddClassName(string className)
        {
            return AddPropertyCondition(new PropertyCondition(AutomationElement.ClassNameProperty, className));
        }
        
        public AutomationQuery AddName(string name)
        {
            return AddPropertyCondition(new PropertyCondition(AutomationElement.NameProperty, name));
        }

        public AutomationQuery AddId(string id)
        {
            return AddPropertyCondition(new PropertyCondition(AutomationElement.AutomationIdProperty, id));
        }

        public AutomationQuery AddControlType(ControlType controlType)
        {
            return AddPropertyCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType));
        }

        public InvokePattern GetInvokePattern(AutomationElement element)
        {
            return (InvokePattern)element.GetCurrentPattern(InvokePattern.Pattern);
        }

        public AutomationElement FindFirst(TreeScope scope, int maxRetries = 20)
        {
            AutomationElement foundElement;
            int numRetries = 0;

            Condition condition = JoinConditions();

            do
            {
                foundElement = _rootElement.FindFirst(scope, condition);
                numRetries++;
                Thread.Sleep(100);
            }
            while (foundElement == null && numRetries < maxRetries);

            *//*if (foundElement == null)
            {
                throw new NullReferenceException($"Could not find the required element.");
            }*//*

            return foundElement;
        }

        public InvokePattern FindFirstInvokePattern(TreeScope scope, int maxRetries = 20)
        {
            AutomationElement element = FindFirst(scope, maxRetries);
            return (InvokePattern)element.GetCurrentPattern(InvokePattern.Pattern);
        }

        public ExpandCollapsePattern FindFirstExpandCollapsePattern(TreeScope scope, int maxRetries = 20)
        {
            AutomationElement element = FindFirst(scope, maxRetries);
            return (ExpandCollapsePattern)element.GetCurrentPattern(ExpandCollapsePattern.Pattern);
        }

        private Condition JoinConditions()
        {
            Condition condition = Condition.TrueCondition;
            foreach (Condition nextCondition in _conditions)
            {
                condition = new AndCondition(condition, nextCondition);
            }
            return condition;
        }
    }

    public class AutomationUtils
    {
        public static InvokePattern FindInvokePattern(AutomationElement element)
        {
            return (InvokePattern)element.GetCurrentPattern(InvokePattern.Pattern);
        }

        public static ExpandCollapsePattern FindExpandCollapsePattern(AutomationElement element)
        {
            return (ExpandCollapsePattern)element.GetCurrentPattern(ExpandCollapsePattern.Pattern);
        }
    }
}
*/