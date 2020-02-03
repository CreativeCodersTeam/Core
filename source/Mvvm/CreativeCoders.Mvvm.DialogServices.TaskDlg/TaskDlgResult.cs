using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.DialogServices.TaskDlg
{
    [PublicAPI]
    public class TaskDlgResult
    {
        public TaskDlgResult(bool? verificationChecked, int? radioButtonResult, int? commandButtonResult,
            int? customButtonResult, TaskDlgButtons commonButtonResult)
        {
            VerificationChecked = verificationChecked;
            RadioButtonResult = radioButtonResult;
            CommandButtonResult = commandButtonResult;
            CustomButtonResult = customButtonResult;
            CommonButtonResult = commonButtonResult;
        }

        public bool? VerificationChecked { get; }

        public int? RadioButtonResult { get; }

        public int? CommandButtonResult { get; }

        public int? CustomButtonResult { get; }

        public TaskDlgButtons CommonButtonResult { get; }
    }
}
