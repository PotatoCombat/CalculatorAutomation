using CalculatorAutomation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    public class CalculatorProcessTests
    {
        public CalculatorProcess calculator;
    
        public void Setup()
        {
            calculator = new CalculatorProcess();
            calculator.Run();
        }

        public void TearDown()
        {
            calculator.Dispose();
        }

        public void Test_ChangeResultTextBox()
        {
            /*Console.WriteLine($"The value in the resultTextBox is: {resultTextBox.GetCurrentPropertyValue(AutomationElement.NameProperty)}");
            // Manually change the number within 5 seconds
            Thread.Sleep(2000);
            Console.WriteLine($"The value in the resultTextBox is: {resultTextBox.GetCurrentPropertyValue(AutomationElement.NameProperty)}");*/
        }

        public void Test_InvokeButtons()
        {
            Console.WriteLine($"The value in the resultTextBox is: {calculator.Result}");

            calculator.InvokeButtonWithDelay("1")
                .InvokeButtonWithDelay("2")
                .InvokeButtonWithDelay("3")
                .InvokeButtonWithDelay("4")
                .InvokeButtonWithDelay("5");

            calculator.InvokeButtonWithDelay("Add");

            calculator.InvokeButtonWithDelay("1")
                .InvokeButtonWithDelay("2")
                .InvokeButtonWithDelay("3")
                .InvokeButtonWithDelay("4");

            calculator.Evaluate();

            Thread.Sleep(1000);

/*            Console.WriteLine($"The value in the resultTextBox is: {calculator.Result}");
*/        }

        public void Test_SelectMenuItems()
        {
            /*editMenu.Expand();
            Thread.Sleep(2000);
            InvokeMenuItem("Copy");
            Thread.Sleep(2000);
            editMenu.Collapse();*/
        }
    }
}
