using Blazored.Toast.Services;
using McHealthCare.Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.JSInterop;

namespace McHealthCare.Web.Extentions.CS
{
    public static class Helper
    {
        public static List<string> LocationTypes = new()
        {
            "Internal Location"
        };
        public static List<string> DepartmentCategories = new()
        {
            "Department",
            "Unit",
        };

        public static List<string> EmployeeTypes = new()
        {
            "Employee",
            "Pre Employee",
            "Nurse",
            "Doctor",
        };

        public static readonly List<string> IdentityTypes =
        [
            "KTP",
            "Paspor",
            "SIM",
            "VISA",
        ];

        public static readonly List<string> MartialStatuss =
        [
            "Single",
            "Married",
            "Divorced",
            "Widowed",
            "Separated",
            "Unmarried"
        ];

        public static List<string> Classification = new()
        {
            "Easy",
            "Medium",
            "Hard"
        };

        public static List<string> TypesInsurance = new List<string>
        {
            "Swasta",
            "Negeri",
        };

        public static List<string> TypesHealthCare = new List<string>
        {
            "Clinic"
        };
        public static List<string> URLS =
        [
            // Clinic Services
            "clinic-service/general-consultation-services",
            "clinic-service/medical-checkups",
            "clinic-service/procedure-rooms",
            "clinic-service/acccidents",
            "clinic-service/reporting-and-analytics",

            // Queues
            "queue/queue-counters",
            "queue/queue-displays",
            "queue/kiosk-departments",
            "queue/kiosk-configurations",

            // BPJS
            "bpjs/bpjs-classifications",
            "bpjs/system-parameters",

            // BPJS Configurations
            "bpjs-integration/physicians",
            "bpjs-integration/services",
            "bpjs-integration/medical-procedures",
            "bpjs-integration/awareness",
            "bpjs-integration/diagnoses",
            "bpjs-integration/providers",
            "bpjs-integration/allergies",
            "bpjs-integration/drugs",

            // Patiens
            "patient/patients",
            "patient/family-relations",
            "patient/insurance-policies",

            // Pharmacies
            "pharmacy/presciptions",
            "pharmacy/medicament-groups",
            "pharmacy/signas",
            "pharmacy/medicine-dosages",
            "pharmacy/active-components",
            "pharmacy/drug-forms",
            "pharmacy/drug-routes",
            "pharmacy/reporting-pharmacy",

            // Inventories
            "inventory/products",
            "inventory/product-categories",
            "inventory/inventory-adjusments",
            "inventory/internal-transfers",
            "inventory/goods-receipts",
            "inventory/uom-categories",
            "inventory/uoms",
            "inventory/locations",
            "inventory/reporting-inventories",

            // Employees
            "employee/employees",
            "employee/sick-leave-managements",
            "employee/claim-managements",
            "employee/departments",
            "employee/job-positions",

            // Medicals
            "medical/practitioners",
            "medical/doctor-schedules",
            "medical/doctor-schedule-slots",
            "medical/insurances",
            "medical/specialities",
            "medical/diagnoses",
            "medical/procedures",
            "medical/disease-categories",
            "medical/chronic-diagnoses",
            "medical/health-centers",
            "medical/building-and-locations",
            "medical/services",
            "medical/nursing-diagnoses",
            "medical/lab-tests",
            "medical/sample-types",
            "medical/lab-uoms",
            "medical/projects",
            "medical/buildings",

            // Configurations
            "configuration/users",
            "configuration/groups",
            "configuration/menus",
            "configuration/email-templates",
            "configuration/email-settings",
            "configuration/companies",
            "configuration/countries",
            "configuration/provinces",
            "configuration/cities",
            "configuration/districts",
            "configuration/sub-districts",
            "configuration/religions",
            "configuration/occupationals",
        ];
        public static void NavigateToUrl(this NavigationManager NavigationManager, string relativeUrl, bool forceLoad = false)
        {
            var absoluteUrl = $"{NavigationManager.BaseUri}{relativeUrl}";
            NavigationManager.NavigateTo(absoluteUrl, forceLoad);
        }
        public static async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jSRuntime, IFileExportService file, string fileName, List<ExportFileData> data, string? name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        public static void ShowInfoSubmittingForm(this IToastService toastService, string message = "Please ensure that all fields marked in red are filled in before submitting the form.") => toastService.ShowInfo(message);
        public static void ShowSuccessSaved(this IToastService toastService, string entity) => toastService.ShowSuccess($"The {entity} record has been saved successfully.");
        public static void ShowSuccessUpdated(this IToastService toastService, string entity) => toastService.ShowSuccess($"The {entity} record has been updated successfully.");

        public static void ShowErrorImport(this IToastService ToastService, int row, int col, string val)
        {
            ToastService.ShowInfo($"Data \"{val}\" in row {row} and column {col} is invalid");
        }

        public static void HandleException(this Exception ex, IToastService toastService)
        {
            string errorMessage = "An error occurred while saving data.";

            if (ex.InnerException is SqlException sqlException)
            {
                switch (sqlException.Number)
                {
                    case 547:
                        errorMessage = "Data cannot be deleted because it is associated with another entity.";
                        break;
                    // Add more cases as needed for specific SQL error numbers
                    default:
                        errorMessage = "An error occurred in the database, Code: " + sqlException.ErrorCode;
                        break;
                }
            }
            else
            {
                errorMessage = "An error occurred";
            }

            Log.Error(
                  "\n\n" +
                  "==================== START ERROR ====================" + "\n" +
                  "Message =====> " + ex.Message + "\n" +
                  "Inner Message =====> " + ex.InnerException?.Message + "\n" +
                  "Stack Trace =====> " + ex.StackTrace?.Trim() + "\n" +
                  "==================== END ERROR ====================" + "\n"
                  );

            toastService.ClearErrorToasts();
            toastService.ShowError(errorMessage);
        }
    }
}