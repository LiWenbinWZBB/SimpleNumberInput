using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleNumberInput
{    
    public abstract class SimpleNumberInput<T> : TextBox where T : struct
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value", typeof(T), typeof(SimpleNumberInput<T>),
                new FrameworkPropertyMetadata(default(T),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PropertyChangedCallback,
                    CoerceValueCallback
                    ));

        public T Value
        {
            get => (T)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(
                "Increment", typeof(T), typeof(SimpleNumberInput<T>),
                new FrameworkPropertyMetadata(default(T),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    ));
        public T Increment
        {
            get => (T)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(
                "MaxValue", typeof(T), typeof(SimpleNumberInput<T>),
                new FrameworkPropertyMetadata(default(T),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    ));
        public T MaxValue
        {
            get => (T)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(
                "MinValue", typeof(T), typeof(SimpleNumberInput<T>),
                new FrameworkPropertyMetadata(default(T),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    ));
        public T MinValue
        {
            get => (T)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        protected bool isSettingValue;
        protected bool isSettingText;

        protected T tmpValue;

        //内部设置数值，不需要更新界面
        protected void setValueInternal(T v)
        {
            this.isSettingValue = true;
            try
            {
                this.Value = v;
            }
            finally
            {
                isSettingValue = false;
            }
        }

        public static void PropertyChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {

            SimpleNumberInput<T> c = (SimpleNumberInput<T>)d;
            c.changeFromSource(e);
        }

        protected abstract void changeFromSource(DependencyPropertyChangedEventArgs e);


        protected void changeTextInternal(string s)
        {
            isSettingText = true;
            try
            {
                this.Text = s;
            }
            finally
            {
                isSettingText = false;
            }
        }

        public static object CoerceValueCallback(DependencyObject d, object baseValue)
        {
            return (T)baseValue;
        }

        protected void UIChangeHandler(object sender, TextChangedEventArgs e)
        {
            if (!isSettingText)
            {
                textChangeHandler();
            }
        }

        protected abstract void textChangeHandler();


        protected abstract void mouseWheelHandler(object sender, MouseWheelEventArgs e);


        public SimpleNumberInput()
        {
            isSettingValue = false;
            isSettingText = false;
            this.TextChanged += UIChangeHandler;
            this.MouseWheel += mouseWheelHandler;
        }
    }
}
