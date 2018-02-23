// <copyright file="IntegerTextBox.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Helper;

namespace Controls
{
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    [Description("A text box that only accepts integer values.")]
    public class IntegerTextBox : TextBox, IIntegerComponent
    {
        public event EventHandler ValueChanged;

        private const int FallbackValue = 0;
        private const bool FallbackAllowHex = false;
        private const bool FallbackAllowNegative = false;
        private const CharacterCasing FallbackCharacterCasing = CharacterCasing.Upper;

        private bool _allowHex = FallbackAllowHex;
        private bool _allowNegative = FallbackAllowNegative;
        private int _value = FallbackValue;

        [Category("Editor")]
        [DefaultValue(FallbackAllowHex)]
        [Description("Determines whether the control reads hexadecimal values or decimal.")]
        public bool AllowHex
        {
            get
            {
                return _allowHex;
            }

            set
            {
                _allowHex = value;
                SetValue(Value);
            }
        }

        [Category("Editor")]
        [DefaultValue(FallbackAllowNegative)]
        [Description("Determines whether negative numbers are valid input.")]
        public bool AllowNegative
        {
            get
            {
                return _allowNegative;
            }

            set
            {
                _allowNegative = value;
                SetValue(_value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private NumberStyles NumberStyle
        {
            get
            {
                return (AllowHex ? NumberStyles.AllowHexSpecifier : NumberStyles.None) |
                    (AllowNegative ? NumberStyles.AllowLeadingSign : NumberStyles.None);
            }
        }

        [Category("Editor")]
        [DefaultValue(FallbackValue)]
        [Description("The value written to the text box.")]
        public int Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        [Category("Behavior")]
        [DefaultValue(FallbackCharacterCasing)]
        [Description("Indicates if all characters should be left alone or converted to uppercase or lowercase.")]
        public new CharacterCasing CharacterCasing
        {
            get { return base.CharacterCasing; }
            set { base.CharacterCasing = value; }
        }

        public IntegerTextBox()
        {
            Text = SR.GetString(FallbackValue);
            CharacterCasing = FallbackCharacterCasing;
        }

        private void SetValue(int value)
        {
            // Make value positive if negative is not allowed.
            if (!AllowNegative && value < 0)
            {
                value = Math.Abs(value);
            }

            // Set the value.
            _value = value;

            // Parse the value.
            Text = SR.GetString(Value, AllowHex ? "X" : String.Empty);
            OnValueChanged(EventArgs.Empty);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            ParseNumericKeyPress(e);
            base.OnKeyPress(e);
        }

        protected virtual void ParseNumericKeyPress(KeyPressEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Only accept valid number characters and formatters
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                return;
            }
            else if (AllowNegative && e.KeyChar == '-' && SelectionStart == 0 && !Text.Contains("-"))
            {
                return;
            }
            else if (AllowHex && e.KeyChar >= 'a' && e.KeyChar <= 'f')
            {
                return;
            }
            else if (AllowHex && e.KeyChar >= 'A' && e.KeyChar <= 'F')
            {
                return;
            }
            else if (e.KeyChar == '\b')
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Save original value.

            // Parse new value.
            if (Int32.TryParse(Text, NumberStyle, CultureInfo.InvariantCulture, out var textValue))
            {
                // Only raise event if value changed.
                if (Value != textValue)
                {
                    Value = textValue;
                }
            }

            base.OnTextChanged(e);
        }
    }
}
