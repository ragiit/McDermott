using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace McDermott.Application.Extentions
{
    public static class EnumHelper
    {
        public static T EnumGetValue<T>(this Enum enumValue)
        {
            return (T)Convert.ChangeType(enumValue, typeof(T));
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.GetName() ?? enumValue.ToString();
        }

        public static TEnum? GetEnumByDisplayName<TEnum>(string displayName) where TEnum : struct, Enum
        {
            var type = typeof(TEnum);
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<DisplayAttribute>();
                if (attribute != null && attribute.Name == displayName)
                {
                    return (TEnum)field.GetValue(null);
                }
            }
            return null;
        }

        public enum EnumStatusGeneralConsultanServiceAnc
        {
            [Display(Name = "Open")]
            Open = 0,

            [Display(Name = "Closed")]
            Closed = 1,
        }

        public enum EnumBpjsWebServiceTemporary
        {
            [Display(Name = "Failed")]
            Failed = 0,

            [Display(Name = "Pending")]
            Pending = 1,

            [Display(Name = "Success")]
            Success = 2,
        }

        public enum EnumStatusCounter
        {
            [Display(Name = "Open")]
            Open = 0,

            [Display(Name = "On Process")]
            OnProcess = 1,

            [Display(Name = "Stop")]
            Stop = 2,
        }

        public enum EnumPageMode
        {
            [Display(Name = "create")]
            Create = 1,

            [Display(Name = "edit")]
            Update = 2,

            [Display(Name = "delete")]
            Delete = 3,
        }

        public enum EnumWellness
        {
            [Display(Name = "Draft")]
            Draft = 1,

            [Display(Name = "Active")]
            Active = 2,

            [Display(Name = "Completed")]
            Completed = 3,

            [Display(Name = "Inactive")]
            Inactive = 0
        }

        public enum EnumStatusInventoryAdjustment
        {
            [Display(Name = "Draft")]
            Draft = 1,

            [Display(Name = "Start Inventory")]
            InProgress = 2,

            [Display(Name = "Canceled")]
            Cancel = 3,

            [Display(Name = "Invalidate")]
            Invalidate = 0,
        }

        public enum EnumStatusVaccination
        {
            [Display(Name = "Scheduled")]
            Scheduled = 1,

            [Display(Name = "In Progress")]
            InProgress = 2,

            [Display(Name = "Done")]
            Done = 3,
        }

        public enum EnumStatusAccident
        {
            [Display(Name = "Draft")]
            Draft = 1,

            [Display(Name = "Medical Treatment")]
            MedicalTreatment = 2,

            [Display(Name = "Hospitalization Referral")]
            HospitalizationReferral = 3,

            [Display(Name = "Patient Control Monitoring")]
            PatientControlMonitoring = 4,

            [Display(Name = "Done")]
            Done = 5
        }

        public enum EnumStatusMCU
        {
            [Display(Name = "Draft")]
            Draft = 1,

            [Display(Name = "Employee Test")]
            EmployeeTest = 2,

            [Display(Name = "HR Candidat")]
            HRCandidat = 3,

            [Display(Name = "Examination")]
            Examination = 4,

            [Display(Name = "Result")]
            Result = 5,

            [Display(Name = "Done")]
            Done = 6
        }

        public enum EnumStatusGeneralConsultantServiceProcedureRoom
        {
            [Display(Name = "Draft")]
            Draft = 1,

            [Display(Name = "In-Progress")]
            InProgress = 2,

            [Display(Name = "Finish")]
            Finish = 3
        }

        public enum EnumStatusGeneralConsultantService
        {
            [Display(Name = "Canceled")]
            Canceled = 0,

            [Display(Name = "Planned")]
            Planned = 1,

            [Display(Name = "Confirmed")]
            Confirmed = 2,

            [Display(Name = "Nurse Station")]
            NurseStation = 3,

            [Display(Name = "Waiting")]
            Waiting = 4,

            [Display(Name = "Procedure Room")]
            ProcedureRoom = 5,

            [Display(Name = "Physician")]
            Physician = 6,

            [Display(Name = "Finished")]
            Finished = 7,

            [Display(Name = "Midwife")]
            Midwife = 8,
        }

        public enum EnumStatusSickLeave
        {
            [Display(Name = "not action")]
            NotAction = 0,

            [Display(Name = "not send")]
            NotSend = 1,

            [Display(Name = "send")]
            Send = 2,

            [Display(Name = "not printed")]
            NotPrinted = 3,

            [Display(Name = "printed")]
            Printed = 4,
        }

        public enum EnumStatusInternalTransfer
        {
            [Display(Name = "draft")]
            Draft = 0,

            [Display(Name = "request")]
            Request = 1,

            [Display(Name = "approve request")]
            ApproveRequest = 2,

            [Display(Name = "waiting")]
            Waiting = 4,

            [Display(Name = "ready")]
            Ready = 3,

            [Display(Name = "done")]
            Done = 5,

            [Display(Name = "cancel")]
            Cancel = 6
        }

        public enum EnumStatusGoodsReceipt
        {
            [Display(Name = "draft")]
            Draft = 0,

            [Display(Name = "process")]
            Process = 1,

            [Display(Name = "done")]
            Done = 2,

            [Display(Name = "cancel")]
            Cancel = 3,
        }

        public enum EnumStatusPharmacy
        {
            [Display(Name = "draft")]
            Draft = 0,

            [Display(Name = "Send To Pharmacy")]
            SendToPharmacy = 1,

            [Display(Name = "Received")]
            Received = 2,

            [Display(Name = "Processed")]
            Processed = 3,

            [Display(Name = "Done")]
            Done = 4,

            [Display(Name = "Cancel")]
            Cancel = 5,
        }

        public enum EnumWorksDays
        {
            [Display(Name = "Days")]
            Days = 0,

            [Display(Name = "Weeks")]
            Weeks = 1,

            [Display(Name = "Months")]
            Months = 2,

            [Display(Name = "Years")]
            Years = 3,
        }

        public enum EnumStatusMaintenance
        {
            [Display(Name = "Request")]
            Request = 0,

            [Display(Name = "InProgress")]
            InProgress = 1,

            [Display(Name = "Repaired")]
            Repaired = 2,

            [Display(Name = "Scrap")]
            Scrap = 3,

            [Display(Name = "Done")]
            Done = 4,

            [Display(Name = "Canceled")]
            Canceled = 5
        }

        public enum EnumStatusEducationProgram
        {
            [Display(Name = "Draft")]
            Draft = 0,

            [Display(Name = "Active")]
            Active = 1,

            [Display(Name = "InActive")]
            InActive = 2,

            [Display(Name = "Done")]
            Done = 3,

            [Display(Name = "Cancel")]
            Cancel = 4,
        }

        public enum EnumBenefitStatus
        {
            [Display(Name = "Draft")]
            Draft = 0,

            [Display(Name = "Active")]
            Active = 1,

            [Display(Name = "InActive")]
            InActive = 2,
        }

        public enum EnumClaimRequestStatus
        {
            [Display(Name = "Draft")]
            Draft = 0,

            [Display(Name = "Done")]
            Done = 1,
        }
    }
}