﻿using System;
using Eto.Drawing;
using Eto.Forms;
using Kuriimu2.EtoForms.Support;
using Serilog.Core;
using Serilog.Events;

namespace Kuriimu2.EtoForms.Logging
{
    class RichTextAreaSink : ILogEventSink
    {
        private readonly RichTextArea _richTextArea;

        public RichTextAreaSink(RichTextArea richTextArea)
        {
            _richTextArea = richTextArea;
        }

        public void Emit(LogEvent logEvent)
        {
            var selectedColor = SelectColor(logEvent.Level);
            var createdLogMessage = logEvent.RenderMessage();

            Application.Instance.Invoke(() => LogInternal(selectedColor, createdLogMessage));
        }

        private Color SelectColor(LogEventLevel logLevel)
        {
            switch (logLevel)
            {
                case LogEventLevel.Information:
                    return Themer.Instance.GetTheme().LogInfoColor;

                case LogEventLevel.Warning:
                    return Themer.Instance.GetTheme().LogWarningColor;

                case LogEventLevel.Error:
                    return Themer.Instance.GetTheme().LogErrorColor;

                case LogEventLevel.Fatal:
                    return Themer.Instance.GetTheme().LogFatalColor;

                default:
                    return Themer.Instance.GetTheme().LogDefaultColor;
            }
        }

        private void LogInternal(Color logColor, string message)
        {
            var position = _richTextArea.Text.Length;

            _richTextArea.Buffer.Insert(_richTextArea.Text.Length, message + Environment.NewLine);
            _richTextArea.Buffer.SetForeground(new Range<int>(position, _richTextArea.Text.Length), logColor);
        }
    }
}
