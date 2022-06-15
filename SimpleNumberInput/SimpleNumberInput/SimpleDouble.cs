using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SimpleNumberInput
{
    public class SimpleDouble : SimpleNumberInput<double>
    {
        public SimpleDouble()
        {
            this.MaxValue = double.MaxValue;
            this.MinValue = double.MinValue;
        }

        protected override void changeFromSource(DependencyPropertyChangedEventArgs e)
        {
            if (!isSettingValue)
            {
                changeTextInternal(e.NewValue.ToString());
            }
            else
            {
                if (tmpValue != (double)e.NewValue)
                {
                    changeTextInternal(e.NewValue.ToString());
                }
            }
        }

        protected override void textChangeHandler()
        {
            double d;
            var m = Regex.Match(this.Text.Trim(), @"[-+]?[0-9]*\.?[0-9]*");
            if (!Double.TryParse(m.Value, out d))
            {
                d = 0;
                isOverLimit(d, ref this.tmpValue);
                changeTextInternal(this.tmpValue.ToString());
            }
            else
            {
                if (isOverLimit(d, ref this.tmpValue))
                {
                    changeTextInternal(this.tmpValue.ToString());
                }
                else
                {
                    changeTextInternal(m.Value);
                }
            }
            this.setValueInternal(this.tmpValue);
            if (this.tmpValue != this.Value)
            {
                changeTextInternal(Value.ToString());
            }
        }

        protected bool isOverLimit(double v, ref double r)
        {
            if (v > this.MaxValue)
            {
                r = this.MaxValue;
                return true;
            }
            if (v < this.MinValue)
            {
                r = this.MinValue;
                return true;
            }
            r = v;
            return false;
        }


        override protected void mouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            double d = this.Value;
            if (e.Delta > 0)
            {
                d += this.Increment;

            }
            if (e.Delta < 0)
            {
                d -= this.Increment;
            }
            if (isOverLimit(d, ref this.tmpValue))
            {
                changeTextInternal(this.tmpValue.ToString());
            }
            this.Value = d;
        }
    }
}
