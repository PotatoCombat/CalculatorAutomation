using System.Windows.Automation;

namespace CalculatorAutomation
{
    public class AutomationUtility
    {
        public static InvokePattern GetInvokePattern(AutomationElement element)
        {
            return GetPattern<InvokePattern>(element, InvokePattern.Pattern);
        }

        public static ExpandCollapsePattern GetExpandCollapsePattern(AutomationElement element)
        {
            return GetPattern<ExpandCollapsePattern>(element, ExpandCollapsePattern.Pattern);
        }

        private static T GetPattern<T>(AutomationElement element, AutomationPattern pattern)
        {
            return (T)element.GetCurrentPattern(pattern);
        }
    }
}
