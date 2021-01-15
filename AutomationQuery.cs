using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace CalculatorAutomation
{
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

            do
            {
                foundElement = _rootElement.FindFirst(scope, this.JoinConditions());
                numRetries++;
                Thread.Sleep(100);
            }
            while (foundElement == null && numRetries < maxRetries);

            /*if (foundElement == null)
            {
                throw new NullReferenceException($"Could not find the required element.");
            }*/

            return foundElement;
        }

        /*public InvokePattern FindFirstInvokePattern(TreeScope scope)
        {
            AutomationElement element = FindFirst(scope);
            return (InvokePattern)element.GetCurrentPattern(InvokePattern.Pattern);
        }

        public ExpandCollapsePattern FindFirstExpandCollapsePattern(TreeScope scope)
        {
            AutomationElement element = FindFirst(scope);
            return (ExpandCollapsePattern)element.GetCurrentPattern(ExpandCollapsePattern.Pattern);
        }*/

        public T FindFirstWithPattern<T>(TreeScope scope, AutomationPattern pattern)
        {
            AutomationElement element = FindFirst(scope);
            return (T)element.GetCurrentPattern(pattern);
        }

        public IEnumerable<AutomationElement> FindAll(TreeScope scope)
        {
            return _rootElement.FindAll(scope, this.JoinConditions()).Cast<AutomationElement>();
        }

        public IEnumerable<T> FindAllWithPattern<T>(TreeScope scope, AutomationPattern pattern)
        {
            return this.FindAll(scope).Select(x => x.GetCurrentPattern(pattern)).Cast<T>();
        }

        /*public ExpandCollapsePattern FindAllExpandCollapsePatterns(TreeScope scope)
        {
            AutomationElement element = FindFirst(scope);
            return (ExpandCollapsePattern)element.GetCurrentPattern(ExpandCollapsePattern.Pattern);
        }*/

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
}
