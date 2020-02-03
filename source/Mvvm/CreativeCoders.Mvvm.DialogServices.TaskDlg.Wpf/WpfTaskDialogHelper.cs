using System.Collections.Generic;
using System.Linq;
using TaskDialogInterop;

namespace CreativeCoders.Mvvm.DialogServices.TaskDlg.Wpf
{
    internal static class WpfTaskDialogHelper
    {
        public static VistaTaskDialogIcon ToVistaTaskDialogIcon(TaskDlgIcon taskDlgIcon)
        {
            return taskDlgIcon switch
            {
                TaskDlgIcon.Shield => VistaTaskDialogIcon.Shield,
                TaskDlgIcon.Information => VistaTaskDialogIcon.Information,
                TaskDlgIcon.Error => VistaTaskDialogIcon.Error,
                TaskDlgIcon.Warning => VistaTaskDialogIcon.Warning,
                TaskDlgIcon.None => VistaTaskDialogIcon.None,
                _ => VistaTaskDialogIcon.None
            };
        }

        public static TaskDialogOptions ToTaskDialogOptions(TaskDlgSettings settings)
        {
            var options = TaskDialogOptions.Default;

            options.MainIcon = ToVistaTaskDialogIcon(settings.MainIcon);
            options.Content = settings.Message;
            options.Title = settings.Title;
            options.Owner = settings.Owner;
            options.MainInstruction = settings.MainInstruction;
            options.ExpandedInfo = settings.ExpandedInfo;
            options.FooterText = settings.FooterText;
            options.VerificationText = settings.VerificationText;
            options.CustomButtons = settings.CustomButtons != null && settings.CustomButtons.Length > 0
                ? settings.CustomButtons
                : ToCustomButtons(settings.CommonButtons);
            options.CommandButtons = settings.CommandButtons;
            options.RadioButtons = settings.RadioButtons;
            options.DefaultButtonIndex = settings.DefaultButtonIndex;
            options.CustomMainIcon = settings.CustomMainIcon;
            options.ExpandedByDefault = settings.ExpandedByDefault;
            options.ExpandToFooter = settings.ExpandToFooter;
            options.VerificationByDefault = settings.VerificationByDefault;
            options.FooterIcon = ToVistaTaskDialogIcon(settings.FooterIcon);
            options.CustomFooterIcon = settings.CustomFooterIcon;

            return options;
        }

        public static TaskDlgResult ToTaskDlgResult(TaskDialogResult result, TaskDlgSettings settings)
        {
            var taskDlgButton = TaskDlgButtons.Ok;
            if (settings.CustomButtons == null || settings.CustomButtons.Length == 0)
            {
                var commonButtons = GetCommonButtons(settings.CommonButtons);
                if (result.CustomButtonResult >= 0 && result.CustomButtonResult < commonButtons.Count)
                {
                    if (result.CustomButtonResult != null)
                        taskDlgButton = commonButtons[result.CustomButtonResult.Value];
                }
            }
            var taskDlgResult = new TaskDlgResult(result.VerificationChecked, result.RadioButtonResult,
                result.CommandButtonResult, result.CustomButtonResult, taskDlgButton);

            return taskDlgResult;
        }

        public static TaskDlgButtons ToTaskDlgButtons(TaskDialogSimpleResult simpleResult)
        {
            return simpleResult switch
            {
                TaskDialogSimpleResult.Ok => TaskDlgButtons.Ok,
                TaskDialogSimpleResult.Cancel => TaskDlgButtons.Cancel,
                TaskDialogSimpleResult.Retry => TaskDlgButtons.Retry,
                TaskDialogSimpleResult.Yes => TaskDlgButtons.Yes,
                TaskDialogSimpleResult.No => TaskDlgButtons.No,
                TaskDialogSimpleResult.Close => TaskDlgButtons.Close,
                TaskDialogSimpleResult.None => TaskDlgButtons.None,
                TaskDialogSimpleResult.Command => TaskDlgButtons.None,
                TaskDialogSimpleResult.Custom => TaskDlgButtons.None,
                _ => TaskDlgButtons.None
            };
        }

        private static IList<TaskDlgButtons> GetCommonButtons(TaskDlgButtons buttons)
        {
            var commonButtons = new List<TaskDlgButtons>();
            if (buttons.HasFlag(TaskDlgButtons.Ok))
            {
                commonButtons.Add(TaskDlgButtons.Ok);
            }
            if (buttons.HasFlag(TaskDlgButtons.Yes))
            {
                commonButtons.Add(TaskDlgButtons.Yes);
            }
            if (buttons.HasFlag(TaskDlgButtons.No))
            {
                commonButtons.Add(TaskDlgButtons.No);
            }
            if (buttons.HasFlag(TaskDlgButtons.Retry))
            {
                commonButtons.Add(TaskDlgButtons.Retry);
            }
            if (buttons.HasFlag(TaskDlgButtons.Cancel))
            {
                commonButtons.Add(TaskDlgButtons.Cancel);
            }
            if (buttons.HasFlag(TaskDlgButtons.Close))
            {
                commonButtons.Add(TaskDlgButtons.Close);
            }
            return commonButtons;
        }

        private static string GetTaskDlgButtonCaption(TaskDlgButtons button)
        {
            return button switch
            {
                TaskDlgButtons.Ok => TaskDlgButtonCaptions.Ok,
                TaskDlgButtons.Yes => TaskDlgButtonCaptions.Yes,
                TaskDlgButtons.No => TaskDlgButtonCaptions.No,
                TaskDlgButtons.Retry => TaskDlgButtonCaptions.Retry,
                TaskDlgButtons.Cancel => TaskDlgButtonCaptions.Cancel,
                TaskDlgButtons.Close => TaskDlgButtonCaptions.Close,
                TaskDlgButtons.None => string.Empty,
                _ => string.Empty
            };
        }

        public static string[] ToCustomButtons(TaskDlgButtons buttons)
        {
            var customButtons = GetCommonButtons(buttons).Select(GetTaskDlgButtonCaption);
            
            return customButtons.ToArray();
        }
    }
}
