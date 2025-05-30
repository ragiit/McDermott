﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace McHealthCare.Extentions
{
    public static class EnumHelper
    {
        public static List<GenderItem> GenderValues => Enum.GetValues(typeof(EnumGender))
                .Cast<EnumGender>()
                .Select(g => new GenderItem
                {
                    Id = (int)g,
                    Name = g.GetDisplayName()
                })
                .ToList();

        public class GenderItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

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

        public enum EnumRole
        {
            [Display(Name = "HR")]
            HR = 1,

            [Display(Name = "User")]
            User = 2,

            [Display(Name = "Admin")]
            Admin = 3,

            [Display(Name = "Employee")]
            Employee = 4,

            [Display(Name = "MCU")]
            MCU = 5,

            [Display(Name = "Pharmacy")]
            Pharmacy = 6,

            [Display(Name = "Patient")]
            Patient = 7,

            [Display(Name = "Practitioner")]
            Practitioner = 8,
        }

        public enum EnumGender
        {
            [Display(Name = "Male")]
            Male = 1,

            [Display(Name = "Female")]
            Female = 2,
        }

        public enum EnumDoctorType
        {
            [Display(Name = "Physicion")]
            Physicion = 1,

            [Display(Name = "Nurse")]
            Nurse = 2,
        }

        public enum EnumPageMode
        {
            [Display(Name = "new")]
            Create = 1,

            [Display(Name = "edit")]
            Update = 2,

            [Display(Name = "delete")]
            Delete = 3,
        }

        public enum EnumTypeReceiveData
        {
            [Display(Name = "Create")]
            Create = 1,

            [Display(Name = "Update")]
            Update = 2,

            [Display(Name = "Delete")]
            Delete = 3,
        }

        public enum EnumStatusInventoryAdjustment
        {
            [Display(Name = "Draft")]
            Draft = 1,

            [Display(Name = "In-Progress")]
            InProgress = 2,

            [Display(Name = "Canceled")]
            Cancel = 3,

            [Display(Name = "Invalidate")]
            Invalidate = 0,
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
            [Display(Name = "Planned")]
            Planned = 1,

            [Display(Name = "Confirmed")]
            Confirmed = 2,

            [Display(Name = "Nurse Station")]
            NurseStation = 3,

            [Display(Name = "Waiting")]
            Waiting = 4,

            [Display(Name = "Physician")]
            Physician = 6,

            [Display(Name = "Finished")]
            Finished = 7,

            [Display(Name = "Canceled")]
            Canceled = 0,

            [Display(Name = "Procedure Room")]
            ProcedureRoom = 5
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

        public enum EnumStatusReceiving
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
    }
}